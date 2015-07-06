using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class KontagentSession
{
    #if UNITY_ANDROID
	private AndroidJavaObject jSession;

	public KontagentSession (AndroidJavaObject jSession)
	{
		this.jSession = jSession;
	}

	public bool start()
	{
		return this.jSession.Call<bool> ("start");
	}

	public void stop()
	{
		this.jSession.Call("stop");
	}

	public void setSecureHttpConnectionEnabled(bool isEnabled)
	{
		this.jSession.Call ("setSecureHttpConnectionEnabled", isEnabled);
	}

	public bool isSecureHttpConnectionEnabled()
	{
		return this.jSession.Call<bool> ("isSecureHttpConnectionEnabled");
	}

	public void startHeartbeatTimer() {
		this.jSession.Call("startHeartbeatTimer");
	}

	public void stopHeartbeatTimer() {
		this.jSession.Call("stopHeartbeatTimer");
	}

	public void applicationAdded( Dictionary<string,string> optionalParams )
	{
		this.jSession.Call( "applicationAdded", KontagentBinding.mapFromDictionary( optionalParams ) );
	}

	public void customEvent( string eventName, Dictionary<string,string> optionalParams )
	{
		this.jSession.Call( "customEvent", eventName, KontagentBinding.mapFromDictionary( optionalParams ) );
	}

	// Posts a revenueTracking event to the server. optionalParams can be null or a Dictionary of key/value pairs.
	public void revenueTracking( int value, Dictionary<string,string> optionalParams )
	{
		using(AndroidJavaObject integerObject = new AndroidJavaObject("java.lang.Integer", value)) {
			this.jSession.Call( "revenueTracking", integerObject, KontagentBinding.mapFromDictionary( optionalParams ) );
		}
	}

	// Posts a sendDeviceInformation request to the server. optionalParams can be null or a Dictionary of key/value pairs.
	public void sendDeviceInformation( Dictionary<string,string> optionalParams )
	{
		this.jSession.Call( "sendDeviceInformation", KontagentBinding.mapFromDictionary( optionalParams ) );
	}
	#endif
}

