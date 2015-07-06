using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class KontagentBinding
{

#if UNITY_ANDROID
    private static AndroidJavaObject _plugin;
    static KontagentBinding()
    {
        // find the plugin instance
        using( var pluginClass = new AndroidJavaClass( "com.prime31.KontagentPlugin" ) )
            _plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
    }

    public static AndroidJavaObject mapFromDictionary( Dictionary<string,string> dict )
    {
        if( dict == null )
            dict = new Dictionary<string,string>();

        // create a hashMap and stick our dictionary values in there
        var hashMap = new AndroidJavaObject( "java.util.HashMap" );

        // creates a seperate local frame for the JNI references that can be cleared to prevent JNI table overflow issues
        // the parameter (50) represents at least how many local references can be created
        AndroidJNI.PushLocalFrame(50);

        // cache the "put" method
        var putMethod = AndroidJNIHelper.GetMethodID( hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;" );

        var arguments = new object[2];
        foreach( var entry in dict )
        {
            using( var key = new AndroidJavaObject( "java.lang.String", entry.Key ) )
            {
                using( var val = new AndroidJavaObject( "java.lang.String", entry.Value ) )
                {
                    arguments[0] = key;
                    arguments[1] = val;
                    AndroidJNI.CallObjectMethod( hashMap.GetRawObject(), putMethod, AndroidJNIHelper.CreateJNIArgArray( arguments ) );
                }
            } // end using
        } // end foreach

        // remove the local frame we created earlier thereby clearing all the references
        // we don't care about result so we pass in zero (null) pointer
        AndroidJNI.PopLocalFrame(IntPtr.Zero);

        return hashMap;
    }
#endif

    #if UNITY_IPHONE
    private static string dictionaryToString( Dictionary<string,string> dict )
    {
        if( dict == null )
            return null;

        var kvPairs = new List<string>();

        foreach( var kvPair in dict )
            kvPairs.Add( string.Format( "{0}||{1}", kvPair.Key, kvPair.Value ) );

        return string.Join( "|||", kvPairs.ToArray() );
    }

    //iOS dll imports

    [DllImport("__Internal")]
    private static extern void _enableDebug (bool enableDebug);

    [DllImport("__Internal")]
    private static extern void _kontagentSetSecureHttpConnection(bool enable);

    [DllImport("__Internal")]
    private static extern bool _kontagentIsSecureHttpConnectionEnabled();

    [DllImport("__Internal")]
    private static extern void _kontagentStartSessionNew(string apiKey, bool enableTestMode, string senderId,
        bool shouldSendAPA, string apiKeyForTimezone, string apiKeyTimezoneOffset, string customID, string fbAppId, bool enableAcquisitionTracking);

    [DllImport("__Internal")]
    private static extern void _kontagentStopSession();

    [DllImport("__Internal")]
    private static extern string _kontagentDynamicValueForKey( string key, string backup );

    [DllImport("__Internal")]
    private static extern void _kontagentPageRequest( string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentMessageSent( string message, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentMessageResponse( bool applicationInstalled, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentApplicationAdded( string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentCustomEvent( string eventName, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentGoalCount( int goalCountId, int goalCountValue );

    [DllImport("__Internal")]
    private static extern void _kontagentInviteSent( string recipientUIDs, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentInviteResponse( bool applicationInstalled, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentRevenueTracking( int value, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentStreamPost( string type, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentStreamPostResponse( bool applicationInstalled, string type, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentUndirectedCommunicationClick( bool applicationInstalled, string type, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentUndirectedCommunicationClickWithTrackingTag( bool applicationInstalled, string type, string trackingTag, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentEmailSent( string recipientUIDs, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentEmailResponse( bool applicationInstalled, string trackingId, string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentUserInformation( string optionalParams );

    [DllImport("__Internal")]
    private static extern void _kontagentSendDeviceInformation( string optionalParams );

    [DllImport("__Internal")]
    private static extern string _kontagentLibraryVersion();

    [DllImport("__Internal")]
    private static extern string _kontagentDefaultApiKey();

    [DllImport("__Internal")]
    private static extern int _kontagentIsUnitTestsBuild();

    [DllImport("__Internal")]
    private static extern int _currentMaxQueueSizeForSessionApiKey(string apiKey);

    [DllImport("__Internal")]
    private static extern void _changeMaxQueueSizeForSessionApiKey(int newQueueSize, string apiKey);

    [DllImport("__Internal")]
    private static extern string _kontagentGetSenderID(string apiKey);

    #endif

    public static KontagentSession createSession(String apiKey, String senderId, Boolean isOnTestMode, Boolean doSendApplicationAdded, String facebookApplicationId) {
        #if  UNITY_ANDROID
        AndroidJavaObject jSession = _plugin.Call<AndroidJavaObject> ("createSession", apiKey, senderId, isOnTestMode, doSendApplicationAdded, facebookApplicationId);
        return new KontagentSession (jSession);
        #else
        return null;
        #endif
    }

    public static void enableDebug()
    {
        #if UNITY_IPHONE
        _enableDebug(true);
        #elif UNITY_ANDROID
        _plugin.Call( "enableDebug", true );
        #endif
    }

    public static void disableDebug()
    {
        #if UNITY_IPHONE
        _enableDebug(false);
        #elif UNITY_ANDROID
        _plugin.Call( "enableDebug", false );
        #endif
    }

    // Initializes the Kontagent session and optionally sets test mode on
    public static void startSession( string apiKey, bool enableTestMode )
    {
        startSession(apiKey, enableTestMode, null);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId)
    {
        startSession(apiKey, enableTestMode, senderId, null);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId, bool shouldSendAPA)
    {
        startSession(apiKey, enableTestMode, senderId, shouldSendAPA, null, null, null);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId, string fbAppID)
    {
        startSession(apiKey, enableTestMode, senderId, true, null, null, null, fbAppID);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId,
        bool shouldSendAPA, string apiKeyForTimezone, string apiKeyTimezoneOffset, string customID )
    {
        startSession(apiKey, enableTestMode, senderId, shouldSendAPA,
            apiKeyForTimezone, apiKeyTimezoneOffset, customID, null);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId,
        bool shouldSendAPA, string apiKeyForTimezone, string apiKeyTimezoneOffset, string customID, string fbAppID )
    {
        startSession(apiKey, enableTestMode, senderId, shouldSendAPA,
            apiKeyForTimezone, apiKeyTimezoneOffset, customID, fbAppID, false);
    }

    public static void startSession( string apiKey, bool enableTestMode, string senderId,
        bool shouldSendAPA, string apiKeyForTimezone, string apiKeyTimezoneOffset, string customID, string fbAppID, bool enableAcquisitionTracking )
    {
        #if UNITY_IPHONE
        _kontagentStartSessionNew(apiKey, enableTestMode, senderId, shouldSendAPA, apiKeyForTimezone, apiKeyTimezoneOffset,
            customID, fbAppID, enableAcquisitionTracking);
        #elif UNITY_ANDROID
        _plugin.Call( "startSession",
            apiKey==null?"":apiKey,
            enableTestMode ? 1 : 0,
            senderId==null?"":senderId,
            shouldSendAPA ? 1 : 0,
            apiKeyForTimezone==null?"":apiKeyForTimezone,
            apiKeyTimezoneOffset==null?"":apiKeyTimezoneOffset,
            customID,
            fbAppID==null?"":fbAppID);
        #endif
    }

    // Stops the current session
    public static void stopSession()
    {

        #if UNITY_IPHONE
            _kontagentStopSession();
        #elif UNITY_ANDROID
            _plugin.Call( "stopSession" );
        #endif
    }

    // Submits a page request. optionalParams can be null or a Dictionary of key/value pairs
    [Obsolete("Will be removed in next Plugin release")]
    public static void pageRequest( Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentPageRequest( dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "pageRequest", mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a messageSent request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void messageSent( string message, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentMessageSent( message, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
                trackingId = string.Empty;
            int recipientId = Convert.ToInt32(message);
            _plugin.Call( "messageSent", recipientId, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a messageResponse request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void messageResponse( bool applicationInstalled, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentMessageResponse( applicationInstalled, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
            trackingId = string.Empty;

            _plugin.Call( "messageResponse", applicationInstalled ? 1 : 0, trackingId, mapFromDictionary( optionalParams ) );
    #endif

    }

    // Posts the application added message to the server. optionalParams can be null or a Dictionary of key/value pairs
    public static void applicationAdded( Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentApplicationAdded( dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "applicationAdded", mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a custom event to the server. optionalParams can be null or a Dictionary of key/value pairs
    public static void customEvent( string eventName, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentCustomEvent( eventName, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "customEvent", eventName, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a goalCount request to the server
    public static void goalCount( int goalCountId, int goalCountValue )
    {
    #if UNITY_IPHONE
            _kontagentGoalCount( goalCountId, goalCountValue );
    #elif UNITY_ANDROID
            _plugin.Call( "goalCount", goalCountId, goalCountValue );
    #endif
    }

    // Posts an inviteSent request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void inviteSent( string recipientUIDs, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentInviteSent( recipientUIDs, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            int recipientId = Convert.ToInt32(recipientUIDs);
            _plugin.Call( "inviteSent", recipientId, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts an inviteResponse request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void inviteResponse( bool applicationInstalled, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentInviteResponse( applicationInstalled, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "inviteResponse", applicationInstalled ? 1 : 0, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a revenueTracking event to the server. optionalParams can be null or a Dictionary of key/value pairs.
    public static void revenueTracking( int value, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentRevenueTracking( value, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "revenueTracking", value, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a streamPost request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void streamPost( string type, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentStreamPost( type, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
            trackingId = string.Empty;

            _plugin.Call( "streamPost", type, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a streamPostResponse request to theserver. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void streamPostResponse( bool applicationInstalled, string type, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentStreamPostResponse( applicationInstalled, type, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
            trackingId = string.Empty;

            _plugin.Call( "streamPostResponse", applicationInstalled ? 1 : 0, type, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts an undirectedCommunicationClick to the server. optionalParams can be null or a Dictionary of key/value pairs.
    [Obsolete("Will be removed in next Plugin release")]
    public static void undirectedCommunicationClick( bool applicationInstalled, string type, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentUndirectedCommunicationClick( applicationInstalled, type, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "undirectedCommunicationClick", applicationInstalled ? 1 : 0, type, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts an undirectedCommunicationClickWithTrackingTag requst to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void undirectedCommunicationClickWithTrackingTag( bool applicationInstalled, string type, string trackingTag, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
        _kontagentUndirectedCommunicationClickWithTrackingTag( applicationInstalled, type, trackingTag, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
        Debug.LogWarning("_kontagentUndirectedCommunicationClickWithTrackingTag is not yet implemented on Android");
    #endif
    }

    // Posts an emailSent request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void emailSent( string recipientUIDs, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentEmailSent( recipientUIDs, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
            trackingId = string.Empty;
            _plugin.Call( "notificationEmailSent", recipientUIDs, trackingId, mapFromDictionary( optionalParams ) );

    #endif
    }

    // Posts an emailResponse request to the server. trackingId and optionalParams can both be null.
    [Obsolete("Will be removed in next Plugin release")]
    public static void emailResponse( bool applicationInstalled, string trackingId, Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentEmailResponse( applicationInstalled, trackingId, dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            if( trackingId == null )
            trackingId = string.Empty;

            _plugin.Call( "notificationEmailResponse", applicationInstalled ? 1 : 0, trackingId, mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a userInformation request to the server
    [Obsolete("Will be removed in next Plugin release")]
    public static void userInformation( Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentUserInformation( dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "userInformation", mapFromDictionary( optionalParams ) );
    #endif
    }

    // Posts a sendDeviceInformation request to the server. optionalParams can be null or a Dictionary of key/value pairs.
    public static void sendDeviceInformation( Dictionary<string,string> optionalParams )
    {
    #if UNITY_IPHONE
            _kontagentSendDeviceInformation( dictionaryToString( optionalParams ) );
    #elif UNITY_ANDROID
            _plugin.Call( "sendDeviceInformation", mapFromDictionary( optionalParams ) );
    #endif
    }

    public static string libraryVersion()
    {
        string theVersion = "n/a";
    #if UNITY_IPHONE
        theVersion = _kontagentLibraryVersion();
    #elif UNITY_ANDROID
        theVersion = _plugin.Call<string>( "libraryVersion" );
    #endif

        return theVersion;
    }

    public static int currentMaxQueueSizeForSessionApiKey(string apiKey)
    {
        int maxQueueSize = 0;
        #if UNITY_IPHONE
        maxQueueSize = _currentMaxQueueSizeForSessionApiKey(apiKey);
        #elif UNITY_ANDROID
        maxQueueSize = _plugin.Call<int>( "currentMaxQueueSizeForSessionApiKey", apiKey );
        #endif
        return maxQueueSize;
    }

    public static void changeMaxQueueSizeForSessionApiKey(int newQueueSize, string apiKey)
    {
        #if UNITY_IPHONE
        _changeMaxQueueSizeForSessionApiKey(newQueueSize, apiKey);
        #elif UNITY_ANDROID
        _plugin.Call( "changeMaxQueueSizeForSessionApiKey", newQueueSize, apiKey);
        #endif
    }

    public static string defaultApiKey()
    {
        string theApiKey = "036bb229bacb4f7e953a7a31d2c88217";
    #if UNITY_IPHONE
        theApiKey = _kontagentDefaultApiKey();
    #elif UNITY_ANDROID
        theApiKey = _plugin.Call<string>("defaultApiKey");
    #endif
        return theApiKey;
    }

    public static bool isUnitTestsBuild()
    {
        bool theUnitTestBuild = false;
    #if UNITY_IPHONE
        theUnitTestBuild = (_kontagentIsUnitTestsBuild() == 1);
    #elif UNITY_ANDROID
        theUnitTestBuild = (_plugin.Call<int>("isUnitTestsBuild") == 1);
    #endif

        return theUnitTestBuild;
    }

    public static string getSenderID(string apiKey)
    {
        string senderID = null;
        #if UNITY_IPHONE
            senderID = _kontagentGetSenderID(apiKey);
        #elif UNITY_ANDROID
            senderID = _plugin.Call<string>("getSenderId", apiKey);
        #endif
        return senderID;
    }

    public static void setSecureHttpConnectionEnabled(bool isEnabled)
    {
        #if UNITY_IPHONE
            _kontagentSetSecureHttpConnection( isEnabled );
        #elif UNITY_ANDROID
            _plugin.Call("setSecureHttpConnectionEnabled", isEnabled);
        #endif
    }

    public static bool isSecureHttpConnectionEnabled()
    {
        bool result = false;
        #if UNITY_IPHONE
            result = _kontagentIsSecureHttpConnectionEnabled();
        #elif UNITY_ANDROID
            result = _plugin.Call<bool> ("isSecureHttpConnectionEnabled");
        #endif
        return result;
    }

    public static void startHeartbeatTimer()
    {
        #if UNITY_ANDROID
            _plugin.Call("startHeartbeatTimer");
        #endif
    }

    public static void stopHeartbeatTimer()
    {
        #if UNITY_ANDROID
            _plugin.Call("stopHeartbeatTimer");
        #endif
    }

    public static string dynamicValueForKey(string key, string backup)
    {
        string result = null;
        #if UNITY_IPHONE
            result = _kontagentDynamicValueForKey( key, backup);
        #endif
        return result;
    }
}
