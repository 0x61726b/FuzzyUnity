//
//  KontagentBinding.m
//  Kontagent
//
//  Created by Mike Desaro on 12/3/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "Kontagent.h"
#import "Kontagent_DynamicData.h"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil



KTParamMap *mapFromKontagentString( NSString *str );
KTParamMap *mapFromKontagentString( NSString *str )
{
	if( !str )
		return nil;
	
	KTParamMap *map = [[[KTParamMap alloc] init] autorelease];
	
	//key1||value1|||key2||value2|||key3||value3
	NSArray *kvPairs = [str componentsSeparatedByString:@"|||"];
	
	for( NSString *item in kvPairs )
	{
		NSArray *kvArr = [item componentsSeparatedByString:@"||"];
		if( kvArr.count == 2 )
			[map put:[kvArr objectAtIndex:0] value:[kvArr objectAtIndex:1]];
	}
	
	return map;
}

static char* MakeStringCopy(const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

#pragma mark - Unity Plugin APIs
void _enableDebug( BOOL enableDebug) {
    if (enableDebug) {
        [Kontagent enableDebug];
    } else {
        [Kontagent disableDebug];
    }
}

void _kontagentSetSecureHttpConnection( BOOL enabled )
{
    return [Kontagent setSecureHttpConnection:enabled];
}


bool _kontagentIsSecureHttpConnectionEnabled()
{
    return [Kontagent isSecureHttpConnectionEnabled];
}

void _kontagentStartSessionNew(const char *apiKey, BOOL enableTestMode, const char *aSenderId,
                                                   BOOL shouldSendAPA,
                                                   const char *apiKeyForTimezone, const char *apiKeyTimezoneOffset,
                                                   const char *customID, const char *fbAppId, bool enableAcquisitionTracking) {
    NSInteger mode = enableTestMode ? kKontagentSDKMode_TEST : kKontagentSDKMode_PRODUCTION;
	if( enableTestMode )
		[Kontagent enableDebug];
    
    NSMutableDictionary* aConfiguration = [[[NSMutableDictionary alloc] init] autorelease];
    [aConfiguration setObject:GetStringParam(apiKey) forKey:KT_SESSION_API_KEY_CONFIG_KEY];
    
    if (GetStringParamOrNil(aSenderId)) {
        [aConfiguration setObject:GetStringParam(aSenderId) forKey:KT_SESSION_SENDER_ID_CONFIG_KEY];
    }
    
    [aConfiguration setObject:[NSNumber numberWithInteger:mode] forKey:KT_SESSION_MODE_CONFIG_KEY];
    [aConfiguration setObject:[NSNumber numberWithBool:shouldSendAPA] forKey:KT_SESSION_APA_CONFIG_KEY];

//    if (GetStringParamOrNil(apiKeyForTimezone)) {
//        [aConfiguration setObject:GetStringParam(apiKeyForTimezone) forKey:KT_SESSION_API_KEY_FOR_TIMEZONE_CONFIG_KEY];
//    }
//    if (GetStringParamOrNil(apiKeyTimezoneOffset)) {
//        [aConfiguration setObject:GetStringParam(apiKeyTimezoneOffset) forKey:KT_SESSION_API_KEY_TIMEZONE_OFFSET_CONFIG_KEY];
//    }
    if (GetStringParamOrNil(customID)) {
        [aConfiguration setObject:GetStringParam(customID) forKey:KT_SESSION_CUSTOM_ID_CONFIG_KEY];
    }
    if (GetStringParamOrNil(fbAppId)) {
        [aConfiguration setObject:GetStringParam(fbAppId) forKey:KT_SESSION_FB_APP_ID_CONFIG_KEY];
    }
    
    [aConfiguration setObject:[NSNumber numberWithBool:shouldSendAPA] forKey:KT_SESSION_ENABLE_ACQUISITION_TRACKING];
    
    // start the Kontagent session
    [Kontagent startSession:aConfiguration];
}

const char* _kontagentDynamicValueForKey( const char * key, const char * backup )
{
    NSString *value = [Kontagent dynamicValueForKey:GetStringParam(key)
                                        backupValue:GetStringParam(backup)];
    return MakeStringCopy([value UTF8String]);
}

void _kontagentStopSession()
{
	[Kontagent stopSession];
}

void _kontagentApplicationAdded(const char * optionalParams )
{
	[Kontagent applicationAdded:mapFromKontagentString( GetStringParamOrNil( optionalParams ) )];
}

void _kontagentCustomEvent( const char * event, const char * optionalParams )
{
	[Kontagent customEvent:GetStringParam( event ) optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentRevenueTracking( int value, const char * optionalParams )
{
	[Kontagent revenueTracking:value optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentSendDeviceInformation( const char * optionalParams )
{
	[Kontagent sendDeviceInformation:mapFromKontagentString( GetStringParamOrNil( optionalParams ) )];
}

const char* _kontagentLibraryVersion()
{
	return MakeStringCopy([[Kontagent libraryVersion] UTF8String]);
}

int _currentMaxQueueSizeForSessionApiKey(const char* apiKey)
{
    return [Kontagent currentMaxQueueSizeForSessionApiKey:GetStringParam(apiKey)];
}

void _changeMaxQueueSizeForSessionApiKey(int newQueueSize, const char* apiKey)
{
    [Kontagent changeMaxQueueSize:newQueueSize forSessionApiKey:GetStringParam(apiKey)];
}

const char* _kontagentDefaultApiKey()
{
	return MakeStringCopy([[NSString stringWithUTF8String:"30b78d544b1c4be3a5df80f3b2123a67"] UTF8String]);
}

int _kontagentIsUnitTestsBuild()
{
    int theUnitTestBuild = 0;
#ifdef UNIT_TEST_BUILD
    theUnitTestBuild = 1;
#endif
	return theUnitTestBuild;
}

const char* _kontagentGetSenderID(const char* apiKey)
{
    return MakeStringCopy([[Kontagent getSenderId:GetStringParamOrNil(apiKey)] UTF8String]);
}

#pragma mark - Deprecated methods
void _kontagentPageRequest( const char * optionalParams )
{
	[Kontagent pageRequest:mapFromKontagentString( GetStringParamOrNil( optionalParams ) )];
}

void _kontagentMessageSent( const char * message, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent messageSent:GetStringParam( message ) trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParamOrNil( optionalParams ) )];
}

void _kontagentMessageResponse( BOOL applicationInstalled, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent messageResponse:applicationInstalled trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentGoalCount( int goalCountId, int goalCountValue )
{
	[Kontagent goalCount:goalCountId value:goalCountValue];
}

void _kontagentInviteSent( const char * recipientUIDs, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent inviteSent:GetStringParam( recipientUIDs ) trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ))];
}

void _kontagentInviteResponse( BOOL applicationInstalled, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent inviteResponse:applicationInstalled trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentStreamPost( const char * type, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent streamPost:GetStringParam( type ) trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ))];
}

void _kontagentStreamPostResponse( BOOL applicationInstalled, const char * type, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent streamPostResponse:applicationInstalled
							 type:GetStringParam( type )
					   trackingId:trackingIdString
				   optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentUndirectedCommunicationClick( BOOL applicationInstalled, const char * type, const char * optionalParams )
{
	[Kontagent undirectedCommunicationClick:applicationInstalled type:GetStringParam( type ) optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentUndirectedCommunicationClickWithTrackingTag( BOOL applicationInstalled, const char * type, const char * trackingTag, const char * trackingId, const char * optionalParams )
{
	[Kontagent undirectedCommunicationClick:applicationInstalled
							 type:GetStringParam( type )
					   trackingTag:GetStringParam( trackingTag )
				   optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentEmailSent( const char * recipientUIDs, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent emailSent:GetStringParam( recipientUIDs ) trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ))];
}

void _kontagentEmailResponse( BOOL applicationInstalled, const char * trackingId, const char * optionalParams )
{
	NSString *trackingIdString = GetStringParamOrNil( trackingId );
	if( !trackingIdString )
		trackingIdString = [Kontagent generateUniqueTrackingTag];
	
	[Kontagent emailResponse:applicationInstalled trackingId:trackingIdString optionalParams:mapFromKontagentString( GetStringParam( optionalParams ) )];
}

void _kontagentUserInformation( const char * optionalParams )
{
	[Kontagent userInformation:mapFromKontagentString( GetStringParamOrNil( optionalParams ) )];
}