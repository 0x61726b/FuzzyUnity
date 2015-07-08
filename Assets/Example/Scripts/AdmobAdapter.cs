using Prime31;﻿

public class AdmobAdapter
{
	private const string IOS_BANNER_ADUNIT_ID = "ca-app-pub-9687821066593343/8944653711";
    private const string IOS_INTERSTITIAL_ADUNIT_ID = "ca-app-pub-9687821066593343/8114416919";
	//private const string IOS_INTERSTITIAL_ADUNIT_ID = "ca-app-pub-9687821066593343/7442410914";
	private const string ANDROID_BANNER_ADUNIT_ID = "ca-app-pub-9687821066593343/5991187311";
    private const string ANDROID_INTERSTITIAL_ADUNIT_ID = "ca-app-pub-9687821066593343/5160950519";

	private bool _adsEnabled = true;
	private bool _bannerVisible;

	public void Awake ()
	{
		PrepareInterstitial ();
	}

	private void PrepareInterstitial ()
	{
		AdMob.requestInterstital (ANDROID_INTERSTITIAL_ADUNIT_ID, IOS_INTERSTITIAL_ADUNIT_ID);
	}

	public void Start ()
	{
		//TODO: _adsEnabled = ServiceLocator.GetSettings().GetSettingBool( SettingsType.ADS_ENABLED );
		//TODO: ServiceLocator.GetSettings().AddSettingsListener( SettingsType.ADS_ENABLED, OnAdSettingChange );
	}

	public void Destroy ()
	{
		//TODO: ServiceLocator.GetSettings().RemoveSettingsListener( SettingsType.ADS_ENABLED, OnAdSettingChange );
	}

	public void ShowBanner ()
	{
		if (_adsEnabled) {
			AdMob.createBanner (IOS_BANNER_ADUNIT_ID, ANDROID_BANNER_ADUNIT_ID, AdMobBanner.SmartBanner, AdMobLocation.TopCenter);
			_bannerVisible = true;
		}
	}

	public void HideBanner ()
	{
		if (_bannerVisible) {
			AdMob.destroyBanner ();
			_bannerVisible = false;
		}
	}

	public void ShowInterstitial ()
	{
		if (_adsEnabled) {
			AdMob.displayInterstital ();
			PrepareInterstitial ();
		}
	}

	private void OnAdSettingChange ()
	{
		//TODO: _adsEnabled = ServiceLocator.GetSettings().GetSettingBool( SettingsType.ADS_ENABLED );
		if (_bannerVisible) {
			HideBanner ();
		}
	}
}
