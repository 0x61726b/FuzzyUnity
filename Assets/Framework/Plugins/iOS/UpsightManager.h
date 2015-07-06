//
//  UpsightManager.h
//  Upsight
//
//  Created by Mike Desaro on 4/8/14.
//  Copyright (c) 2014 prime31. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PlayHavenSDK.h"


// this enum is used to tag requests so that when the delegate is called we know what request it is being called for
typedef enum {
	UpsightRequestTypeOpen,
	UpsightRequestTypeContent,
	UpsightRequestTypePreloadContent,
	UpsightRequestTypeMetaDataRequest,
	UpsightRequestTypeTrackIAP,
	UpsightRequestTypeReportCustomEvent
} UpsightRequestType;


@interface UpsightManager : NSObject <PHAPIRequestDelegate, PHPublisherContentRequestDelegate, PHPushProviderDelegate>
@property (nonatomic, copy) NSString *appToken;
@property (nonatomic, copy) NSString *appSecret;
@property (nonatomic) BOOL shouldOpenContentRequestsFromPushNotifications;
@property (nonatomic) BOOL shouldOpenUrlsFromPushNotifications;


+ (UpsightManager*)sharedManger;

+ (NSString*)jsonStringFromObject:(NSObject*)object;

+ (NSObject*)objectFromJsonString:(NSString*)json;


- (void)requestAppOpen;

- (void)sendContentRequestWithPlacement:(NSString*)placement showsOverlayImmediately:(BOOL)showsOverlayImmediately shouldAnimate:(BOOL)shouldAnimate dimensions:(NSDictionary *)dimensions;

- (void)sendContentRequestWithContentUnitID:(NSString*)contentUnitID messageID:(NSString*)messageID showsOverlayImmediately:(BOOL)showsOverlayImmediately shouldAnimate:(BOOL)shouldAnimate dimensions:(NSDictionary *)dimensions campaignID:(NSString*)campaignID;

- (void)preloadContentRequest:(NSString*)placement dimensions:(NSDictionary *)dimensions;

- (void)sendMetadataRequest:(NSString*)placement;

- (void)trackIAPWithProductID:(NSString*)productID quantity:(int)quantity resolutionType:(PHPurchaseResolutionType)resolutionType receiptData:(NSData*)receiptData;

- (void)trackCustomEventWithProperties:(NSDictionary*)properties;

@end
