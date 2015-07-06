using UnityEngine;
using System.Collections.Generic;


#if UNITY_ANDROID
public enum UpsightAndroidPurchaseResolution
{
	Unset,
	Bought,
	Cancelled,
	Invalid,
	Owned,
	Error
}

public class Upsight
{
	private static AndroidJavaObject _plugin;

	static Upsight()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		UpsightManager.noop();

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.upsight.unity.UpsightPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}


	/// <summary>
	/// Android only. Sets the current log level for the Upsight SDK.
	/// </summary>
	/// <param name="logLevel">Log level.</param>
	public static void setLogLevel( UpsightLogLevel logLevel )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setLogLevel", logLevel.ToString() );
	}


	/// <summary>
	/// Gets the plugin version
	/// </summary>
	/// <returns>The plugin version.</returns>
	public static string getPluginVersion()
	{
		if( Application.platform != RuntimePlatform.Android )
			return "UnityEditor";

		return _plugin.Call<string>( "getPluginVersion" );
	}


	/// <summary>
	/// Initializes the plugin. gcmProjectNumber is optional and only required if you are using push notifications.
	/// </summary>
	/// <param name="appToken">App token.</param>
	/// <param name="appSecret">App secret.</param>
	/// <param name="gcmProjectNumber">Gcm project number.</param>
	public static void init( string appToken, string appSecret, string gcmProjectNumber = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "init", appToken, appSecret, gcmProjectNumber );
	}


	/// <summary>
	/// Requests an app open. This should be called every time your game is opened. It will automatically check for any received
	/// push notifications and fire off the appropriate event if your app was launched from a push. The openRequestSucceeded/FailedEvent
	/// will also fire letting you know if the operation succeeded.
	/// </summary>
	public static void requestAppOpen()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "requestAppOpen" );
	}


	/// <summary>
	/// Sends a content request optionally showing the loading overlay immediately and animating the display. In addition you can assign custom dimensions
	/// to the content request. Dimensions is a means to define new user segments that can be used to target content to specific users. By setting your own
	/// dimensions on a content request you get ability to distinguish requests coming from different users. For example, as a dimension you can use current
	/// user location, gender, etc.. For more information on custom dimensions feature, see a documentation on the Upsight Dashboard.
	/// </summary>
	/// <param name="placement">Placement.</param>
	/// <param name="showsOverlayImmediately">If set to <c>true</c> shows overlay immediately.</param>
	/// <param name="shouldAnimate">If set to <c>true</c> should animate.</param>
	/// <param name="dimensions">Custom dimensions that are assigned to the content request. When setting a dimension you need to know the following rules:
	/// - Dimension value must have string, numeric or boolean type.
	/// - The Upsight's server stores all custom dimensions received from each device.
	/// - You can remove a dimension for a given key on the server, by passing null as a dimension value.
	/// - Sending the same key with different value will update the custom dimension stored server-side.
	/// </param>
	public static void sendContentRequest( string placementID, bool showsOverlayImmediately, bool shouldAnimate = true, Dictionary<string,object> dimensions = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		AndroidJavaObject jsonObject = dictionaryToJavaHashMap (dimensions);
		_plugin.Call( "sendContentRequest", placementID, showsOverlayImmediately, shouldAnimate, jsonObject );
	}

	public static AndroidJavaObject dictionaryToJavaHashMap(Dictionary<string,object> dictionary)
	{
		AndroidJavaObject jsonObject = null;
		if (dictionary != null)
		{
			var jsonParserClass = new AndroidJavaClass("net.minidev.json.parser.JSONParser");
			int jsonParserMode = jsonParserClass.GetStatic<int>("MODE_JSON_SIMPLE");

			string jsonString = MiniJSON.Json.Serialize (dictionary);
			var jsonParser = new AndroidJavaObject("net.minidev.json.parser.JSONParser", jsonParserMode);
			jsonObject = jsonParser.Call<AndroidJavaObject>("parse", jsonString);
		}
		return jsonObject;
	}

	/// <summary>
	/// Preloads a content request. Results in the contentPreloadSucceeded/FailedEvent firing.
	/// You can later call sendContentRequest to send it.
	/// </summary>
	/// <param name="placement">Placement.</param>
	/// <param name="dimensions">Custom dimensions that are assigned to the content request. For more
	/// information on custom dimensions see sendContentRequest: method documentation. If you preload
	/// a content request to improve its responsiveness you should pass the same dimensions to this
	/// method as the ones passed to subsequent call of sendContentRequest: method.
	/// </param>
	public static void preloadContentRequest( string placementID, Dictionary<string,object> dimensions = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		AndroidJavaObject jsonObject = dictionaryToJavaHashMap (dimensions);
		_plugin.Call( "preloadContentRequest", placementID, jsonObject );
	}


	/// <summary>
	/// Sends a request to get the badge count for the given placement
	/// </summary>
	/// <param name="placement">Placement.</param>
	public static void getContentBadgeNumber( string placementID )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "sendMetadataRequest", placementID );
	}


	/// <summary>
	/// Gets the opt out status.
	/// </summary>
	/// <returns>the opt out status</returns>
	public static bool getOptOutStatus()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;

		return _plugin.Call<bool>( "getOptOutStatus" );
	}


	/// <summary>
	/// Sets the opt out status.
	/// </summary>
	/// <param name="optOutStatus">If set to <c>true</c> opt out status.</param>
	public static void setOptOutStatus( bool optOutStatus )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setOptOutStatus", optOutStatus );
	}


	/// <summary>
	/// Tracks an in app purchase.
	/// </summary>
	/// <param name="productID">Product I.</param>
	/// <param name="quantity">Quantity.</param>
	/// <param name="resolutionType">Resolution type.</param>
	/// <param name="receiptData">Receipt data.</param>
	public static void trackInAppPurchase( string sku, int quantity, UpsightAndroidPurchaseResolution resolutionType, double price, string orderId, string store )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "trackInAppPurchase", sku, quantity, (int)resolutionType, price, orderId, store );
	}


	/// <summary>
	/// Reports a custom event
	/// </summary>
	/// <param name="properties">Properties.</param>
	public static void reportCustomEvent( Dictionary<string,object> properties )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "reportCustomEvent", MiniJSON.Json.Serialize( properties ) );
	}


	#region Push Notifications

	/// <summary>
	/// Registers the current device for push notifications.
	/// </summary>
	public static void registerForPushNotifications()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "registerForPushNotifications" );
	}


	/// <summary>
	/// Deregisters the current device for push notifications.
	/// </summary>
	public static void deregisterForPushNotifications()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "deregisterForPushNotifications" );
	}


	/// <summary>
	/// iOS only. Sets a flag indicating whether a push notification with content should be automatically opened.
	/// </summary>
	/// <param name="shouldOpen">If set to <c>true</c> should open.</param>
	public static void setShouldOpenContentRequestsFromPushNotifications( bool shouldOpen )
	{}


	/// <summary>
	/// iOS only. Sets a flag indicating whether a push notification with a URL should be automatically opened.
	/// </summary>
	/// <param name="shouldOpen">If set to <c>true</c> should open.</param>
	public static void setShouldOpenUrlsFromPushNotifications( bool shouldOpen )
	{}

	#endregion

}
#endif