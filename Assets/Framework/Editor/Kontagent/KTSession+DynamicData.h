//
//  KTSession+DynamicData.h
//  Kontagent
//
//  Created by Dana Smith on 2013-08-21.
//  Copyright (c) 2013 Kontagent. All rights reserved.
//

#import "Kontagent.h"

@interface KTSession ()

/** Returns the value paired with the provided key. The value and associated key are stored on the Kontagent server and are configurable through the dashboard.
 
 If no value for this key has been retrieved from the server, a copy of aBackupValue is returned.
 In this case, if the SDK mode is set to kKontagentSDKMode_TEST, the key and value are also sent to the server where they can subsequently be changed.
 It can take up to 5 minutes after first launching the App for values to be available from the server. It can take up to an hour for changes made on the server to propagate to the mobile device.
 
 @param key A unique key identifying the text to be retrieved from the server.
 @param backupValue The text to be returned if the key does not exist on the server
 @return The value of the text associated with aKey on the dashboard, or a copy of aBackupValue
 */
- (NSString*)dynamicValueForKey:(NSString*)key backupValue:(NSString*)backupValue;

/** Enables INFO: messages that detail the run-time operation of the Dynamic Content system
 
 @param enabled YES to turn verbose mode on, NO to turn it off
 */
- (void)verboseDynamicContentLogging:(BOOL)enabled;

@end
