using UnityEngine;
using System;
using System.Collections;
using Prime31;


#if UNITY_IOS || UNITY_ANDROID
namespace Prime31
{
	#if UNITY_ANDROID

	using Manager = AdMobAndroidManager;

	public enum AdMobBanner
	{
		Phone_320x50,
		Tablet_300x250,
		Tablet_468x60,
		Tablet_728x90,
		SmartBanner
	}

	#elif UNITY_IOS

	using Manager = AdMobManager;

	public enum AdMobBanner
	{
		Phone_320x50,
		Tablet_300x250 = 3,
		Tablet_468x60 = 2,
		Tablet_728x90 = 1,
		SmartBanner = 4
	}

	#endif

	public enum AdMobLocation
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}


	public static class AdMob
	{
		// Fired when a new ad is loaded
		public static event Action receivedAdEvent;

		// Fired when an ad fails to be loaded
		public static event Action<string> failedToReceiveAdEvent;

		// Fired when an interstitial is loaded and ready for use
		public static event Action interstitialReceivedAdEvent;

		// Fired when an interstitial fails to receive an ad
		public static event Action<string> interstitialFailedToReceiveAdEvent;


		#region Event Handlers

		static void receivedAdEventHandler()
		{
			receivedAdEvent.fire();
		}


		static void failedToReceiveAdEventHandler( string error )
		{
			failedToReceiveAdEvent.fire( error );
		}


		static void interstitialReceivedAdEventHandler()
		{
			interstitialReceivedAdEvent.fire();
		}


		static void interstitialFailedToReceiveAdEventHandler( string error )
		{
			interstitialFailedToReceiveAdEvent.fire( error );
		}

		#endregion


		static AdMob()
		{
			Manager.receivedAdEvent += receivedAdEventHandler;
			Manager.failedToReceiveAdEvent += failedToReceiveAdEventHandler;
			Manager.interstitialReceivedAdEvent += interstitialReceivedAdEventHandler;
			Manager.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEventHandler;
		}


		// Passing true will set a flag that indicates that your content should be treated as child-directed for purposes of COPPA
		public static void setTagForChildDirectedTreatment( bool shouldTag )
		{
#if UNITY_IOS
			AdMobBinding.setTagForChildDirectedTreatment( shouldTag );
#elif UNITY_ANDROID
			AdMobAndroid.setTagForChildDirectedTreatment( shouldTag );
#endif
		}


		// Sets test devices. This needs to be set BEFORE a banner is created.
		public static void setTestDevices( string[] testDevices )
		{
#if UNITY_IOS
			AdMobBinding.setTestDevices( testDevices );
#elif UNITY_ANDROID
			AdMobAndroid.setTestDevices( testDevices );
#endif
		}


		// Creates a banner of the given type at the given position. This method requires an adUnitId and you must be updated to the new AdMob system.
		public static void createBanner( string iosAdUnitId, string androidAdUnitId, AdMobBanner type, AdMobLocation placement )
		{
#if UNITY_IOS
			if( type == AdMobBanner.SmartBanner )
			{
				if( Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight )
					AdMobBinding.createBanner( iosAdUnitId, AdMobBannerType.SmartBannerLandscape, (AdMobAdPosition)placement );
				else
					AdMobBinding.createBanner( iosAdUnitId, AdMobBannerType.SmartBannerPortrait, (AdMobAdPosition)placement );
			}
			else
			{
				AdMobBinding.createBanner( iosAdUnitId, (AdMobBannerType)type, (AdMobAdPosition)placement );
			}
#elif UNITY_ANDROID
			AdMobAndroid.createBanner( androidAdUnitId, (int)type, (int)placement );
#endif
		}


		// Hides/shows the ad banner
		public static void hideBanner( bool shouldHide )
		{
#if UNITY_IOS
			AdMobBinding.hideBanner( shouldHide );
#elif UNITY_ANDROID
			AdMobAndroid.hideBanner( shouldHide );
#endif
		}


		// Destroys the banner if it is showing
		public static void destroyBanner()
		{
#if UNITY_IOS
			AdMobBinding.destroyBanner();
#elif UNITY_ANDROID
			AdMobAndroid.destroyBanner();
#endif
		}


		// Requests an interstitial ad.  When it is loaded, the the interstitialReceivedAdEvent will be fired
		public static void requestInterstital( string androidInterstitialUnitId, string iosInterstitialUnitId )
		{
#if UNITY_IOS
			AdMobBinding.requestInterstital( iosInterstitialUnitId );
#elif UNITY_ANDROID
			AdMobAndroid.requestInterstital( androidInterstitialUnitId );
#endif
		}


		// Check to see if an interstitial ad is ready to be displayed
		public static bool isInterstitalReady()
		{
#if UNITY_IOS
			return AdMobBinding.isInterstitialAdReady();
#elif UNITY_ANDROID
			return AdMobAndroid.isInterstitalReady();
#else
			return false;
#endif
		}


		// Displays an interstitial if it is ready to be displayed
		public static void displayInterstital()
		{
#if UNITY_IOS
			AdMobBinding.displayInterstital();
#elif UNITY_ANDROID
			AdMobAndroid.displayInterstital();
#endif
		}

	}

}
#endif
