using UnityEngine;
using System;
using System.Collections.Generic;
using MiniJSON;


#if UNITY_IPHONE || UNITY_ANDROID
public class UpsightManager : MonoBehaviour
{
	#region Constructor and Lifecycle

	static UpsightManager()
	{
		// try/catch this so that we can warn users if they try to stick this script on a GO manually
		try
		{
			// create a new GO for our manager
			var go = new GameObject( "UpsightManager" );
			go.AddComponent<UpsightManager>();
			DontDestroyOnLoad( go );
		}
		catch( UnityException )
		{
			Debug.LogWarning( "It looks like you have the UpsightManager on a GameObject in your scene. Please remove the script from your scene." );
		}
	}


	// used to ensure the UpsightManager will always be in the scene to avoid SendMessage logs if the user isn't using any events
	public static void noop(){}

	#endregion


	/// <summary>
	/// Fired when an open request succeeds. Includes a Dictionary of all data returned.
	/// </summary>
	public static event Action<Dictionary<string,object>> openRequestSucceededEvent;

	/// <summary>
	/// Fired when an open request fails. Includes the error.
	/// </summary>
	public static event Action<string> openRequestFailedEvent;

	/// <summary>
	/// Fired when content will be displayed. Includes the placement.
	/// </summary>
	public static event Action<string> contentWillDisplayEvent;

	/// <summary>
	/// iOS only. Fired after content is displayed. Includes the placement.
	/// </summary>
	public static event Action<string> contentDidDisplayEvent;

	/// <summary>
	/// Fired when a content request loads. Includes the placement.
	/// </summary>
	public static event Action<string> contentRequestLoadedEvent;

	/// <summary>
	/// Fired when a content request fails. Includes the placement and error.
	/// </summary>
	public static event Action<string,string> contentRequestFailedEvent;

	/// <summary>
	/// Fired when a content preload succeeds. Includes the placementID.
	/// </summary>
	public static event Action<string> contentPreloadSucceededEvent;

	/// <summary>
	/// Fired when a content preload fails. Includes the placement and error.
	/// </summary>
	public static event Action<string,string> contentPreloadFailedEvent;

	/// <summary>
	/// Fired when a call to getContentBadgeNumber succeeds. Includes the badge count.
	/// </summary>
	public static event Action<int> badgeCountRequestSucceededEvent;

	/// <summary>
	/// Fired when a getContentBadgeNumber request fails or if the provided placement does not contain a badge count.
	/// </summary>
	public static event Action<string> badgeCountRequestFailedEvent;

	/// <summary>
	/// Fired when tracking an IAP succeeds
	/// </summary>
	public static event Action trackInAppPurchaseSucceededEvent;

	/// <summary>
	/// Fired when tracking an IAP fails. Includes the error.
	/// </summary>
	public static event Action<string> trackInAppPurchaseFailedEvent;

	/// <summary>
	/// Fired when the reporting of a custom event succeeds
	/// </summary>
	public static event Action reportCustomEventSucceededEvent;

	/// <summary>
	/// Fired when the reporting of a custom event fails. Includes the error.
	/// </summary>
	public static event Action<string> reportCustomEventFailedEvent;

	/// <summary>
	/// Fired whenever content is dismissed. The parameters are the placementID and the dismiss type
	/// </summary>
	public static event Action<string,string> contentDismissedEvent;

	/// <summary>
	/// Fired when a request to make a purchase occurs
	/// </summary>
	public static event Action<UpsightPurchase> makePurchaseEvent;

	/// <summary>
	/// Android only. Fired when a data opt-in occurs. Includes a Dictionary of all returned keys and values.
	/// </summary>
	public static event Action<Dictionary<string,object>> dataOptInEvent;

	/// <summary>
	/// Fired when a reward is unlocked
	/// </summary>
	public static event Action<UpsightReward> unlockedRewardEvent;

	/// <summary>
	/// iOS only. Fired when a push notification that contains content is received. The paramters are
	/// the messageID and the contentUnitID. These can be passed to the sendContentRequestWithContentUnitID method
	/// to display the content.
	/// </summary>
	public static event Action<string,string,string> pushNotificationWithContentReceivedEvent;

	/// <summary>
	/// iOS only. Fired when a push notification that contains a URL is received.
	/// </summary>
	public static event Action<string> pushNotificationWithUrlReceivedEvent;



	#region Private methods called from native code

	void openRequestSucceeded( string json )
	{
		if( openRequestSucceededEvent != null )
			openRequestSucceededEvent( Json.Deserialize( json ) as Dictionary<string,object> );
	}


	void openRequestFailed( string error )
	{
		if( openRequestFailedEvent != null )
			openRequestFailedEvent( error );
	}


	void contentWillDisplay( string placementID )
	{
		if( contentWillDisplayEvent != null )
			contentWillDisplayEvent( placementID );
	}


	void contentDidDisplay( string placementID )
	{
		if( contentDidDisplayEvent != null )
			contentDidDisplayEvent( placementID );
	}


	void contentRequestLoaded( string placementID )
	{
		if( contentRequestLoadedEvent != null )
			contentRequestLoadedEvent( placementID );
	}


	void contentRequestFailed( string json )
	{
		if( contentRequestFailedEvent != null )
		{
			var dict = Json.Deserialize( json ) as Dictionary<string,object>;
			if( dict != null && dict.ContainsKey( "error" ) && dict.ContainsKey( "placement" ) )
				contentRequestFailedEvent( dict["placement"].ToString(), dict["error"].ToString() );
		}
	}


	void contentPreloadSucceeded( string placementID )
	{
		if( contentPreloadSucceededEvent != null )
			contentPreloadSucceededEvent( placementID );
	}


	void contentPreloadFailed( string json )
	{
		if( contentPreloadFailedEvent != null )
		{
			var dict = Json.Deserialize( json ) as Dictionary<string,object>;
			if( dict != null && dict.ContainsKey( "error" ) && dict.ContainsKey( "placement" ) )
				contentPreloadFailedEvent( dict["placement"].ToString(), dict["error"].ToString() );
		}
	}


	void metadataRequestSucceeded( string json )
	{
		if( badgeCountRequestSucceededEvent != null )
		{
			// extract the badge count if there is one
			var res = Json.Deserialize( json ) as Dictionary<string,object>;
			if( res != null && res.ContainsKey( "notification" ) )
			{
				var notificationDict = res["notification"] as Dictionary<string,object>;
				if( notificationDict.ContainsKey( "type" ) && notificationDict.ContainsKey( "value" ) )
				{
					badgeCountRequestSucceededEvent( int.Parse( notificationDict["value"].ToString() ) );
					return;
				}
			}
		}

		// if we get here we found no badge count so fire the error handler
		badgeCountRequestFailedEvent( "No badge count could be found for the placement" );
	}


	void metadataRequestFailed( string error )
	{
		if( badgeCountRequestFailedEvent != null )
			badgeCountRequestFailedEvent( error );
	}


	void trackInAppPurchaseSucceeded( string empty )
	{
		if( trackInAppPurchaseSucceededEvent != null )
			trackInAppPurchaseSucceededEvent();
	}


	void trackInAppPurchaseFailed( string error )
	{
		if( trackInAppPurchaseFailedEvent != null )
			trackInAppPurchaseFailedEvent( error );
	}


	void reportCustomEventSucceeded( string empty )
	{
		if( reportCustomEventSucceededEvent != null )
			reportCustomEventSucceededEvent();
	}


	void reportCustomEventFailed( string error )
	{
		if( reportCustomEventFailedEvent != null )
			reportCustomEventFailedEvent( error );
	}


	void contentDismissed( string json )
	{
		if( contentDismissedEvent != null )
		{
			var dict = Json.Deserialize( json ) as Dictionary<string,object>;
			if( dict != null && dict.ContainsKey( "dismissType" ) && dict.ContainsKey( "placement" ) )
				contentDismissedEvent( dict["placement"].ToString(), dict["dismissType"].ToString() );
		}
	}


	void makePurchase( string json )
	{
		if( makePurchaseEvent != null )
			makePurchaseEvent( UpsightPurchase.purchaseFromJson( json ) );
	}


	void dataOptIn( string json )
	{
		if( dataOptInEvent != null )
			dataOptInEvent( Json.Deserialize( json ) as Dictionary<string,object> );
	}


	void unlockedReward( string json )
	{
		if( unlockedRewardEvent != null )
			unlockedRewardEvent( UpsightReward.rewardFromJson( json ) );
	}


	void pushNotificationWithContentReceived( string json )
	{
		if( pushNotificationWithContentReceivedEvent != null )
		{
			var dict = Json.Deserialize( json ) as Dictionary<string,object>;
			if( dict != null && dict.ContainsKey( "messageID" ) && dict.ContainsKey( "contentUnitID" ) )
			{
				var campaignID = "" as String;
				if( dict.ContainsKey( "campaignID" ) && null != dict["campaignID"])
					campaignID = dict["campaignID"].ToString();
				pushNotificationWithContentReceivedEvent( dict["messageID"].ToString(), dict["contentUnitID"].ToString(), campaignID );
			}
		}
	}


	void pushNotificationWithUrlReceived( string url )
	{
		if( pushNotificationWithUrlReceivedEvent != null )
			pushNotificationWithUrlReceivedEvent( url );
	}

	#endregion

}
#endif