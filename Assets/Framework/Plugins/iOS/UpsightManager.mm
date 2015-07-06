//
//  UpsightManager.mm
//  Upsight
//
//  Created by Mike Desaro on 4/8/14.
//  Copyright (c) 2014 prime31. All rights reserved.
//

#import "UpsightManager.h"
#include "AppDelegateListener.h"


static char const * const kUnityManagerName = "UpsightManager";


void UnitySendMessage( const char * className, const char * methodName, const char * param );

// we wrap the UnitySendMessage calls here just in case for some reason a NULL char* gets passed through. This will prevent
// Unity from choking when Mono calls strlen on the char* in the actual UnitySendMessage method.
void SafeUnitySendMessage( const char * className, const char * methodName, const char * param )
{
	if( className == NULL )
		className = kUnityManagerName;
	
	if( methodName == NULL )
		methodName = "";
	
	if( param == NULL )
		param = "";
	
	UnitySendMessage( className, methodName, param );
}



@interface UpsightManager()
@property (nonatomic, retain) NSMutableDictionary *preloadedContentRequests;
@property (nonatomic, retain) NSDictionary *remoteNotificationDictionary;
@property (nonatomic, copy) NSString *pushMessageID;
@property (nonatomic, copy) NSString *pushContentID;
@property (nonatomic, copy) NSString *pushCampaignID;
@end


@implementation UpsightManager

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSObject, Class and Private Methods

+ (void)load
{
	// wire up our AppDelegate listener early so that we don't miss any notifications from Unity
	UnityRegisterAppDelegateListener( (id)[self sharedManger] );
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(applicationDidFinishLaunchingNotification:)
												 name:UIApplicationDidFinishLaunchingNotification
											   object:nil];
}


+ (UpsightManager*)sharedManger
{
	static dispatch_once_t pred;
	static UpsightManager *_sharedInstance = nil;
	dispatch_once( &pred, ^{ _sharedInstance = [[self alloc] init]; } );
	return _sharedInstance;
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		self.preloadedContentRequests = [NSMutableDictionary dictionary];
		self.shouldOpenContentRequestsFromPushNotifications = self.shouldOpenUrlsFromPushNotifications = YES;
	}
	return self;
}


+ (BOOL)isValidJsonObject:(NSObject*)object
{
	return [NSJSONSerialization isValidJSONObject:object];
}


// gets a string from an NSDictionary or NSArray
+ (NSString*)jsonStringFromObject:(NSObject*)object
{
	if( ![self isValidJsonObject:object] || !object )
	{
		// if we don't have a valid JSON object we will default to an empty object which will prevent a catastropic error
		NSLog( @"the object that is being serialized [%@] is not a valid JSON object", object );
		return @"{}";
	}
	
	NSError *error = nil;
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:0 error:&error];
	if( jsonData && !error )
		return [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	else
		NSLog( @"jsonData was null, error: %@", [error localizedDescription] );
	
	return @"{}";
}


// gets an NSDictionary or NSArray from a JSON string
+ (NSObject*)objectFromJsonString:(NSString*)json
{
	NSError *error = nil;
	NSData *data = [NSData dataWithBytes:json.UTF8String length:json.length];
    NSObject *object = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingAllowFragments error:&error];
	
	if( error )
		NSLog( @"failed to deserialize JSON: %@ with error: %@", json, error );
	
	return object;
}


// we store off the messageID and contentID here so the user can show any content received via push when they want to
- (void)extractContentFromNotificationDictionary:(NSDictionary*)userInfoDict
{
	// this next bit is a huge hack due to messageID and contentID not being public in the PHPublisherContentRequest class
	// and the delegate method is not passed either of them.
	NSNumber *messageIDNumber = [userInfoDict objectForKey:@"mi"];
    if( messageIDNumber && [messageIDNumber isKindOfClass:[NSNumber class]] )
    {
		NSString *messageID = [messageIDNumber stringValue];
		NSString *contentID = [[userInfoDict objectForKey:@"ci"] description];
        NSString *campaignID = [[userInfoDict objectForKey:@"cmi"] description];
		
		if( contentID )
		{
			// if we have both values store them. when the delegate method is called we send them back to Unity
			NSLog( @"found the MessageID and ContentID from the push notification" );
			self.pushMessageID = messageID;
			self.pushContentID = contentID;
            self.pushCampaignID = campaignID;
		}
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSNotifications

+ (void)applicationDidFinishLaunchingNotification:(NSNotification*)note
{
	if( note.userInfo )
	{
		NSLog( @"found userInfo in the launch notification. going to check for remote notification: %@", note.userInfo );
		NSDictionary *remoteNotificationDictionary = [note.userInfo objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
		if( remoteNotificationDictionary )
		{
			NSLog( @"found a remove notification at launch" );
			[UpsightManager sharedManger].remoteNotificationDictionary = remoteNotificationDictionary;
			[[UpsightManager sharedManger] extractContentFromNotificationDictionary:remoteNotificationDictionary];
		}
	}
}


- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification
{
	// not sure why Unity sends the device token as the userInfo dictionary...
	NSLog( @"didRegisterForRemoteNotificationsWithDeviceToken: %@", notification.userInfo );
	[[PHPushProvider sharedInstance] registerAPNSDeviceToken:(NSData*)notification.userInfo];
	
	// handle any pending remote notifications
	if( self.remoteNotificationDictionary )
	{
		NSLog( @"handling pending remote notification from launch" );
		[[PHPushProvider sharedInstance] handleRemoteNotificationWithUserInfo:self.remoteNotificationDictionary];
		self.remoteNotificationDictionary = nil;
	}
}


- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification
{
	NSLog( @"didFailToRegisterForRemoteNotificationsWithError: %@", notification.userInfo );
}


- (void)didReceiveRemoteNotification:(NSNotification*)notification
{
	NSLog( @"didReceiveRemoteNotification" );
	
	[self extractContentFromNotificationDictionary:notification.userInfo];
	[[PHPushProvider sharedInstance] handleRemoteNotificationWithUserInfo:notification.userInfo];
}


- (void)onOpenURL:(NSNotification*)notification
{
	NSLog( @"application:openURL: %@", notification.userInfo );
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Public

- (void)requestAppOpen
{
	PHPublisherOpenRequest *req = [PHPublisherOpenRequest requestForApp:self.appToken secret:self.appSecret];
	req.delegate = self;
	req.hashCode = (int)UpsightRequestTypeOpen;
	[req send];
}


- (void)sendContentRequestWithPlacement:(NSString*)placement showsOverlayImmediately:(BOOL)showsOverlayImmediately shouldAnimate:(BOOL)shouldAnimate dimensions:(NSDictionary *)dimensions
{
	PHPublisherContentRequest *req = nil;
	
	// check our preload cache first
	if( [[self.preloadedContentRequests allKeys] containsObject:placement] )
	{
		NSLog( @"cache hit. preloaded content being shown now" );
		req = self.preloadedContentRequests[placement];
		[self.preloadedContentRequests removeObjectForKey:placement];
	}
	else
	{
		req = [PHPublisherContentRequest requestForApp:self.appToken
												secret:self.appSecret
											 placement:placement
											  delegate:self];
	}
	req.showsOverlayImmediately = showsOverlayImmediately;
	req.animated = shouldAnimate;
	[req addDimensionsFromDictionary:dimensions];
	req.hashCode = (int)UpsightRequestTypeContent;
	[req send];
}


- (void)sendContentRequestWithContentUnitID:(NSString*)contentUnitID
								  messageID:(NSString*)messageID
					showsOverlayImmediately:(BOOL)showsOverlayImmediately
						 	 shouldAnimate:(BOOL)shouldAnimate
                                 dimensions:(NSDictionary *)dimensions
                                 campaignID:(NSString*)campaignID
{
    if (0 == campaignID.length) {
        campaignID = nil;
    }
	PHPublisherContentRequest *req = [PHPublisherContentRequest requestForApp:self.appToken
																	   secret:self.appSecret
																contentUnitID:contentUnitID
																	messageID:messageID
                                                                   campaignID:campaignID];
	req.delegate = self;
	req.showsOverlayImmediately = showsOverlayImmediately;
	req.animated = shouldAnimate;
	[req addDimensionsFromDictionary:dimensions];
	req.hashCode = (int)UpsightRequestTypeContent;
	[req send];
}


- (void)preloadContentRequest:(NSString*)placement dimensions:(NSDictionary *)dimensions
{
	PHPublisherContentRequest *req = [PHPublisherContentRequest requestForApp:self.appToken
																	   secret:self.appSecret
																	placement:placement
																	 delegate:self];
	[req addDimensionsFromDictionary:dimensions];
	req.hashCode = (int)UpsightRequestTypePreloadContent;
	[req preload];
}


- (void)sendMetadataRequest:(NSString*)placement
{
	PHPublisherMetadataRequest *req = [PHPublisherMetadataRequest requestForApp:self.appToken
																		 secret:self.appSecret
																	  placement:placement
																	   delegate:self];
	req.hashCode = UpsightRequestTypeMetaDataRequest;
	[req send];
}


- (void)trackIAPWithProductID:(NSString*)productID quantity:(int)quantity resolutionType:(PHPurchaseResolutionType)resolutionType receiptData:(NSData*)receiptData
{
	PHPublisherIAPTrackingRequest *req = [PHPublisherIAPTrackingRequest requestForApp:self.appToken
																			   secret:self.appSecret
																			  product:productID
																			 quantity:quantity
																		   resolution:resolutionType
																		  receiptData:receiptData];
	req.hashCode = UpsightRequestTypeTrackIAP;
	req.delegate = self;
	[req send];
}


- (void)trackCustomEventWithProperties:(NSDictionary*)properties
{
	PHEvent *event = [PHEvent eventWithProperties:properties];
	PHEventRequest *req = [PHEventRequest requestForApp:self.appToken secret:self.appSecret event:event];
	req.hashCode = UpsightRequestTypeReportCustomEvent;
	req.delegate = self;
	[req send];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - PHPushProviderDelegate

- (BOOL)pushProvider:(PHPushProvider*)aProvider shouldSendRequest:(PHPublisherContentRequest*)aRequest
{
	if( self.pushMessageID && self.pushContentID )
	{
		// send these back to Unity then nil them out so they only get used once
		NSDictionary *dict = @{ @"messageID": self.pushMessageID,
                                @"contentUnitID": self.pushContentID,
                                @"campaignID": (nil != self.pushCampaignID) ? self.pushCampaignID: [NSNull null]};

        SafeUnitySendMessage( kUnityManagerName, "pushNotificationWithContentReceived", [UpsightManager jsonStringFromObject:dict].UTF8String );
		
		self.pushMessageID = self.pushContentID = nil;
	}

	if( self.shouldOpenContentRequestsFromPushNotifications )
	{
		aRequest.delegate = self;
		aRequest.hashCode = UpsightRequestTypeContent;
	}
	
	return self.shouldOpenContentRequestsFromPushNotifications;
}


- (BOOL)pushProvider:(PHPushProvider*)aProvider shouldOpenURL:(NSURL*)anURL
{
	SafeUnitySendMessage( kUnityManagerName, "pushNotificationWithUrlReceived", anURL.absoluteString.UTF8String );
	return self.shouldOpenUrlsFromPushNotifications;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - PHAPIRequestDelegate

- (void)request:(PHAPIRequest*)request didSucceedWithResponse:(NSDictionary*)responseData
{
	if( responseData )
		NSLog( @"request:didSucceedWithResponse: %@", responseData );
	
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypeOpen:
		{
			NSDictionary *dataDict = responseData ? responseData : [NSDictionary dictionary];
			SafeUnitySendMessage( kUnityManagerName, "openRequestSucceeded", [UpsightManager jsonStringFromObject:dataDict].UTF8String );
			break;
		}
		case UpsightRequestTypeMetaDataRequest:
		{
			NSDictionary *dataDict = responseData ? responseData : [NSDictionary dictionary];
			SafeUnitySendMessage( kUnityManagerName, "metadataRequestSucceeded", [UpsightManager jsonStringFromObject:dataDict].UTF8String );
			break;
		}
		case UpsightRequestTypeTrackIAP:
		{
			SafeUnitySendMessage( kUnityManagerName, "trackInAppPurchaseSucceeded", "" );
			break;
		}
		case UpsightRequestTypeReportCustomEvent:
		{
			SafeUnitySendMessage( kUnityManagerName, "reportCustomEventSucceeded", "" );
			break;
		}
		default:
		{
			NSLog( @"request:didSucceedWithResponse: received an unknown requestType: %i", request.hashCode );
		}
	}
}


- (void)request:(PHAPIRequest*)request didFailWithError:(NSError*)error
{
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypeOpen:
		{
			SafeUnitySendMessage( kUnityManagerName, "openRequestFailed", error.localizedDescription.UTF8String );
			break;
		}
		case UpsightRequestTypeMetaDataRequest:
		{
			SafeUnitySendMessage( kUnityManagerName, "metadataRequestFailed", error.localizedDescription.UTF8String );
			break;
		}
		case UpsightRequestTypeTrackIAP:
		{
			SafeUnitySendMessage( kUnityManagerName, "trackInAppPurchaseFailed", error.localizedDescription.UTF8String );
			break;
		}
		case UpsightRequestTypeReportCustomEvent:
		{
			SafeUnitySendMessage( kUnityManagerName, "reportCustomEventFailed", error.localizedDescription.UTF8String );
			break;
		}
        case UpsightRequestTypeContent:
        {
            PHPublisherContentRequest *contentResponse = (PHPublisherContentRequest *)request;
            NSDictionary *responseDict = @{ @"placement": contentResponse.placement ? contentResponse.placement : @"PushNotification",
                                            @"error": error.localizedDescription };
            SafeUnitySendMessage( kUnityManagerName, "contentRequestFailed", [UpsightManager jsonStringFromObject:responseDict].UTF8String );
            break;
        }
        case UpsightRequestTypePreloadContent:
        {
            PHPublisherContentRequest *contentResponse = (PHPublisherContentRequest *)request;
            NSDictionary *responseDict = @{ @"placement": contentResponse.placement ? contentResponse.placement : @"PushNotification",
                                            @"error": error.localizedDescription };
            SafeUnitySendMessage( kUnityManagerName, "contentPreloadFailed", [UpsightManager jsonStringFromObject:responseDict].UTF8String );
            break;
        }
		default:
		{
			NSLog( @"request:didFailWithError: received an unknown requestType: %i", request.hashCode );
		}
	}
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - PHPublisherContentRequestDelegate

- (void)requestDidGetContent:(PHPublisherContentRequest*)request
{
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypeContent:
		{
			NSString *placement = request.placement ? request.placement : @"PushNotification";
			SafeUnitySendMessage( kUnityManagerName, "contentRequestLoaded", placement.UTF8String );
			break;
		}
		case UpsightRequestTypePreloadContent:
		{
			// store off the preloaded request for later use
			[self.preloadedContentRequests setObject:request forKey:request.placement];
			SafeUnitySendMessage( kUnityManagerName, "contentPreloadSucceeded", request.placement.UTF8String );
			break;
		}
		default:
		{
			NSLog( @"requestDidGetContent: received an unknown requestType: %i", request.hashCode );
		}
	}
}


- (void)request:(PHPublisherContentRequest*)request contentWillDisplay:(PHContent*)content
{
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypeContent:
		{
			NSString *placement = request.placement ? request.placement : @"PushNotification";
			SafeUnitySendMessage( kUnityManagerName, "contentWillDisplay", placement.UTF8String );
			break;
		}
		default:
		{
			NSLog( @"request:contentWillDisplay: received an unknown requestType: %i", request.hashCode );
		}
	}
}


- (void)request:(PHPublisherContentRequest*)request contentDidDisplay:(PHContent*)content
{
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypeContent:
		{
			NSString *placement = request.placement ? request.placement : @"PushNotification";
			SafeUnitySendMessage( kUnityManagerName, "contentDidDisplay", placement.UTF8String );
			break;
		}
		default:
		{
			NSLog( @"request:contentDidDisplay: received an unknown requestType: %i", request.hashCode );
		}
	}
}


- (void)request:(PHPublisherContentRequest*)request contentDidDismissWithType:(PHPublisherContentDismissType*)type
{
	// we need to translate the type to something without the "PHPublisher" prefix for easier reading on the Unity side
	NSString *dismissType = [type stringByReplacingOccurrencesOfString:@"PHPublisher" withString:@""];

	NSDictionary *responseDict = @{ @"placement": request.placement ? request.placement : @"PushNotification",
									@"dismissType": dismissType };
	
	switch( (UpsightRequestType)request.hashCode )
	{
		case UpsightRequestTypePreloadContent:
		case UpsightRequestTypeContent:
		{
			SafeUnitySendMessage( kUnityManagerName, "contentDismissed", [UpsightManager jsonStringFromObject:responseDict].UTF8String );
			break;
		}
		default:
		{
			NSLog( @"request:contentDidDismissWithType: received an unknown requestType: %i", request.hashCode );
		}
	}
}


- (void)request:(PHPublisherContentRequest*)request unlockedReward:(PHReward*)reward
{
	// we use a string here for quanity because for some reason the Android SDK does so
	NSDictionary *responseDict = @{ @"quantity": [NSString stringWithFormat:@"%li", (long)reward.quantity],
									@"receipt": reward.receipt,
									@"name": reward.name };
	SafeUnitySendMessage( kUnityManagerName, "unlockedReward", [UpsightManager jsonStringFromObject:responseDict].UTF8String );
}


- (void)request:(PHPublisherContentRequest*)request makePurchase:(PHPurchase*)purchase
{
	// we use a string here for quanity because for some reason the Android SDK does so
	NSDictionary *responseDict = @{ @"productIdentifier": purchase.productIdentifier,
									@"quantity": [NSString stringWithFormat:@"%li", (long)purchase.quantity],
									@"receipt": purchase.receipt };
	SafeUnitySendMessage( kUnityManagerName, "makePurchase", [UpsightManager jsonStringFromObject:responseDict].UTF8String );
}

@end
