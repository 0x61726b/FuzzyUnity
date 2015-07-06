//
//  UpsightBinding.m
//  Upsight
// 
//  This is the C layer of global methods that is directly linked to the methods in the Upsight.cs file via the DllImport statements
//
//  Created by Mike Desaro on 4/8/14.
//

#import "UpsightManager.h"
#import "PlayhavenSDK.h"
#import "PHAPIRequest.h"


// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil

// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

#define RequiresInit( _x_ ) if( !_checkForInitializationAndAbortIfNotInitialized() ) return _x_;

#define UPSIGHT_PLUGIN_VERSION @"ios-unity-3.0.4"



BOOL _checkForInitializationAndAbortIfNotInitialized()
{
	if( [UpsightManager sharedManger].appSecret && [UpsightManager sharedManger].appToken )
		return YES;
	
	NSLog( @"You must call init with your app token and secret before calling this method" );
	
	return NO;
}


NSDictionary *DictionaryWithUTF8String(const char *nullTerminatedCString)
{
	NSDictionary *dictionary = nil;
	
	if (NULL != nullTerminatedCString)
	{
		dictionary = (NSDictionary*)[UpsightManager objectFromJsonString:GetStringParam( nullTerminatedCString )];

		if (![dictionary isKindOfClass:[NSDictionary class]])
		{
			NSLog( @"%s [ERROR] Cannot create a dictionary from string: %s", __PRETTY_FUNCTION__, nullTerminatedCString );
			dictionary = nil;
		}
	}
	
	return dictionary;
}


// Public API
const char * _usGetPluginVersion()
{
	return MakeStringCopy( UPSIGHT_PLUGIN_VERSION );
}


void _usInit( const char* appToken, const char* appSecret )
{
	[PHPushProvider sharedInstance].applicationToken = [UpsightManager sharedManger].appToken = GetStringParam( appToken );
	[PHPushProvider sharedInstance].applicationSecret = [UpsightManager sharedManger].appSecret = GetStringParam( appSecret );
	[PHPushProvider sharedInstance].delegate = [UpsightManager sharedManger];
	
	[PHAPIRequest setPluginIdentifier:UPSIGHT_PLUGIN_VERSION];
}


void _usRequestAppOpen()
{
	RequiresInit();
	[[UpsightManager sharedManger] requestAppOpen];
}


void _usSendContentRequest( const char* placement, BOOL showsOverlayImmediately, BOOL shouldAnimate, const char* dimensions )
{
	RequiresInit();

	[[UpsightManager sharedManger] sendContentRequestWithPlacement:GetStringParam( placement )
										   showsOverlayImmediately:showsOverlayImmediately
										   			 shouldAnimate:shouldAnimate dimensions:DictionaryWithUTF8String( dimensions )];
}


void _usSendContentRequestWithContentUnitID( const char* contentUnitID, const char* messageID, BOOL showsOverlayImmediately, BOOL shouldAnimate, const char* dimensions, const char* campaignID )
{
	RequiresInit();
	[[UpsightManager sharedManger] sendContentRequestWithContentUnitID:GetStringParam( contentUnitID )
															 messageID:GetStringParam( messageID )
											   showsOverlayImmediately:showsOverlayImmediately
												   		 shouldAnimate:shouldAnimate
                                                            dimensions:DictionaryWithUTF8String( dimensions )
                                                            campaignID:GetStringParam( campaignID )];
}


void _usPreloadContentRequest( const char* placement, const char* dimensions )
{
	RequiresInit();
	[[UpsightManager sharedManger] preloadContentRequest:GetStringParam( placement ) dimensions:DictionaryWithUTF8String( dimensions )];
}


void _usSendMetadataRequest( const char* placement )
{
	RequiresInit();
	[[UpsightManager sharedManger] sendMetadataRequest:GetStringParam( placement )];
}


BOOL _usGetOptOutStatus()
{
	return [PHAPIRequest optOutStatus];
}


void _usSetOptOutStatus( BOOL optOutStatus )
{
	[PHAPIRequest setOptOutStatus:optOutStatus];
}


void _usTrackInAppPurchase( const char* productID, int quantity, int resolutionType, unsigned char* receiptData, int receiptDataLength )
{
	RequiresInit();
	
	NSData *receipt = nil;
	if( receiptDataLength > 0 )
		receipt = [NSData dataWithBytes:(void*)receiptData length:receiptDataLength];

	[[UpsightManager sharedManger] trackIAPWithProductID:GetStringParam( productID )
												quantity:quantity
										  resolutionType:(PHPurchaseResolutionType)resolutionType
											 receiptData:receipt];
}


void _usReportCustomEvent( const char* properties )
{
	RequiresInit();
	
	NSDictionary *propertiesDict = DictionaryWithUTF8String( properties );
	if( nil == propertiesDict )
	{
		NSLog( @"aborting tracking event. could not create dictionary from parsed data" );
		return;
	}
	
	[[UpsightManager sharedManger] trackCustomEventWithProperties:propertiesDict];
}


void _usUnregisterForPushNotifications()
{
	[[PHPushProvider sharedInstance] unregisterForPushNotifications];
}


void _usSetShouldOpenContentRequestsFromPushNotifications( BOOL shouldOpen )
{
	[UpsightManager sharedManger].shouldOpenContentRequestsFromPushNotifications = shouldOpen;
}


void _usSetShouldOpenUrlsFromPushNotifications( BOOL shouldOpen )
{
	[UpsightManager sharedManger].shouldOpenUrlsFromPushNotifications = shouldOpen;
}


BOOL _usToggleStatusBarHidden()
{
	[[UIApplication sharedApplication] setStatusBarHidden:![UIApplication sharedApplication].statusBarHidden];
	return [UIApplication sharedApplication].statusBarHidden;
}


void _usLog( const char * message )
{
	NSComparisonResult comparisonResult = [[[UIDevice currentDevice] systemVersion] compare:@"7.0" options:NSNumericSearch];
    
    if( comparisonResult == NSOrderedSame || comparisonResult == NSOrderedDescending )
		NSLog( @"%@", GetStringParam( message ) );
}

