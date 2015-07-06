using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;


#if UNITY_IPHONE || UNITY_ANDROID
public enum UpsightLogLevel
{
	SUPPRESS,
	VERBOSE,
	DEBUG,
	INFO,
	WARN,
	ERROR,
	ASSERT
}
#endif


#if UNITY_IPHONE
public enum UpsightIosPurchaseResolution
{
	Buy,
	Cancel,
	Error,
	Failure
}


public class Upsight
{
	static Upsight()
	{
		UpsightManager.noop();
#if UNITY_EDITOR
		Debug.Log( "Note: the Upsight plugin is a native code plugin. It will only return data, fire events and show content when run on an actual iOS or Android device." );
#endif
	}


	/// <summary>
	/// Android only. Provided here as a convenience for writing cross-platform code.
	/// </summary>
	/// <param name="logLevel">Log level.</param>
	public static void setLogLevel( UpsightLogLevel logLevel )
	{}


	[DllImport("__Internal")]
	private static extern string _usGetPluginVersion();

	/// <summary>
	/// Gets the plugin version
	/// </summary>
	/// <returns>The plugin version.</returns>
	public static string getPluginVersion()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _usGetPluginVersion();

		return "UnityEditor";
	}


	[DllImport("__Internal")]
	private static extern void _usInit( string appToken, string appSecret );

	/// <summary>
	/// Initializes the plugin. The gcmProjectNumber is Android only and is here so you only have to write code once.
	/// </summary>
	/// <param name="appToken">App token.</param>
	/// <param name="appSecret">App secret.</param>
	/// <param name="gcmProjectNumber">Gcm project number.</param>
	public static void init( string appToken, string appSecret, string gcmProjectNumber = null )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usInit( appToken, appSecret );
	}


	[DllImport("__Internal")]
	private static extern void _usRequestAppOpen();

	/// <summary>
	/// Requests an app open. This should be called every time your game is opened. It will automatically check for any received
	/// push notifications and fire off the appropriate event if your app was launched from a push. The openRequestSucceeded/FailedEvent
	/// will also fire letting you know if the operation succeeded.
	/// </summary>
	public static void requestAppOpen()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usRequestAppOpen();
	}


	[DllImport("__Internal")]
	private static extern void _usSendContentRequest( string placement, bool showsOverlayImmediately, bool shouldAnimate, string dimensions = null );

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
	public static void sendContentRequest( string placement, bool showsOverlayImmediately, bool shouldAnimate = true, Dictionary<string,object> dimensions = null)
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSendContentRequest( placement, showsOverlayImmediately, shouldAnimate, null != dimensions ? MiniJSON.Json.Serialize( dimensions ) : null);
	}


	[DllImport("__Internal")]
	private static extern void _usSendContentRequestWithContentUnitID( string contentUnitID, string messageID, bool showsOverlayImmediately, bool shouldAnimate, string dimensions = null, string campaignID = null );

	/// <summary>
	/// iOS only. Sends a content request with the contentUnitID and messageID received from a push notification (pushNotificationWithContentReceivedEvent). In addition you can specify custom
	/// dimensions to the content request. For more information on custom dimensions see sendContentRequest: method documentation.
	/// </summary>
	/// <param name="contentUnitID">ContentUnitID</param>
	/// <param name="messageID">Message I.</param>
	/// <param name="showsOverlayImmediately">If set to <c>true</c> shows overlay immediately.</param>
	/// <param name="dimensions">Custom dimensions that are assigned to the content request. For more information on custom dimensions see sendContentRequest: method documentation.</param>
	public static void sendContentRequestWithContentUnitID( string contentUnitID, string messageID, bool showsOverlayImmediately, bool shouldAnimate = true, Dictionary<string,object> dimensions = null, string campaignID = null )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSendContentRequestWithContentUnitID( contentUnitID, messageID, showsOverlayImmediately, shouldAnimate, null != dimensions ? MiniJSON.Json.Serialize( dimensions ) : null, campaignID );
	}


	[DllImport("__Internal")]
	private static extern void _usPreloadContentRequest( string placement, string dimensions = null );

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
	public static void preloadContentRequest( string placement, Dictionary<string,object> dimensions = null )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usPreloadContentRequest( placement, null != dimensions ? MiniJSON.Json.Serialize( dimensions ) : null );
	}


	[DllImport("__Internal")]
	private static extern void _usSendMetadataRequest( string placement );

	/// <summary>
	/// Sends a request to get the badge count for the given placement
	/// </summary>
	/// <param name="placement">Placement.</param>
	public static void getContentBadgeNumber( string placement )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSendMetadataRequest( placement );
	}


	[DllImport("__Internal")]
	private static extern bool _usGetOptOutStatus();

	/// <summary>
	/// Gets the opt out status.
	/// </summary>
	/// <returns>the opt out status</returns>
	public static bool getOptOutStatus()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _usGetOptOutStatus();

		return false;
	}


	[DllImport("__Internal")]
	private static extern void _usSetOptOutStatus( bool optOutStatus );

	/// <summary>
	/// Sets the opt out status.
	/// </summary>
	/// <param name="optOutStatus">If set to <c>true</c> opt out status.</param>
	public static void setOptOutStatus( bool optOutStatus )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSetOptOutStatus( optOutStatus );
	}


	[DllImport("__Internal")]
	private static extern void _usTrackInAppPurchase( string productID, int quantity, int resolutionType, byte[] receiptData, int receiptDataLength );

	/// <summary>
	/// Tracks an in app purchase.
	/// </summary>
	/// <param name="productID">Product I.</param>
	/// <param name="quantity">Quantity.</param>
	/// <param name="resolutionType">Resolution type.</param>
	/// <param name="receiptData">Receipt data.</param>
	public static void trackInAppPurchase( string productID, int quantity, UpsightIosPurchaseResolution resolutionType, byte[] receiptData = null )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			if( receiptData != null )
				_usTrackInAppPurchase( productID, quantity, (int)resolutionType, receiptData, receiptData.Length );
			else
				_usTrackInAppPurchase( productID, quantity, (int)resolutionType, null, 0 );
		}
	}


	[DllImport("__Internal")]
	private static extern void _usReportCustomEvent( string properties );

	/// <summary>
	/// Reports a custom event
	/// </summary>
	/// <param name="properties">Properties.</param>
	public static void reportCustomEvent( Dictionary<string,object> properties )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usReportCustomEvent( null != properties ? MiniJSON.Json.Serialize( properties ) : null );
	}


	#region Push Notifications

	/// <summary>
	/// Registers the current device for push notifications.
	/// </summary>
	public static void registerForPushNotifications()
	{
		NotificationServices.RegisterForRemoteNotificationTypes( RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound );
	}


	[DllImport("__Internal")]
	private static extern void _usUnregisterForPushNotifications();

	/// <summary>
	/// Deregisters the current device for push notifications.
	/// </summary>
	public static void deregisterForPushNotifications()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usUnregisterForPushNotifications();
	}


	[DllImport("__Internal")]
	private static extern void _usSetShouldOpenContentRequestsFromPushNotifications( bool shouldOpen );

	/// <summary>
	/// iOS only. Sets a flag indicating whether a push notification with content should be automatically opened.
	/// </summary>
	/// <param name="shouldOpen">If set to <c>true</c> should open.</param>
	public static void setShouldOpenContentRequestsFromPushNotifications( bool shouldOpen )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSetShouldOpenContentRequestsFromPushNotifications( shouldOpen );
	}


	[DllImport("__Internal")]
	private static extern void _usSetShouldOpenUrlsFromPushNotifications( bool shouldOpen );

	/// <summary>
	/// iOS only. Sets a flag indicating whether a push notification with a URL should be automatically opened.
	/// </summary>
	/// <param name="shouldOpen">If set to <c>true</c> should open.</param>
	public static void setShouldOpenUrlsFromPushNotifications( bool shouldOpen )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_usSetShouldOpenUrlsFromPushNotifications( shouldOpen );
	}

	#endregion

}
#endif