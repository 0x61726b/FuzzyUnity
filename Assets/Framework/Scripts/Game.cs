using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Services.Store;
using com.gramgames.analytics;
using Assets.Scripts.Framework.Util;
using Assets.Scripts.Framework.Services.AudioService;

#if !UNITY_EDITOR && UNITY_IPHONE
using Assets.Scripts.Framework.Services.Store.IosStoreAdapter;
#elif !UNITY_EDITOR && UNITY_ANDROID
using Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter;
#endif

public class Game : MonoBehaviour
{
	public const int APPLICATION_VERSION = 1;
	public const string LEFT_IN_GAME_FLAG = "game_wasInGame";

    public static Game _instance;

	public int CurrentLevel;
	public bool inGame = false;
	public long gameStart;

    //private LevelData levelData;

	#if !UNITY_EDITOR && UNITY_ANDROID
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if(inGame)
			{
				//InGamePause(!Board._instance.isGamePaused);
			}
			else
			{
				Application.Quit();
			}
		}
	}
	#endif

	void Awake()
	{
        
		_instance = this;
		Application.targetFrameRate = 60;

		ServiceLocator.Boot( new Dictionary<Service, Provider> {
			{Service.DB, new FilePersistence()},
			{Service.Settings, new SettingsProvider()},
			{Service.Audio, new AudioProvider()},
			{Service.Localization, new LocalizationProvider()},
			{Service.Notification, new NotificationProvider()},
			{Service.Life, new LifeProvider()},
			{Service.UAnalytics, new UpsightAnalytics()},
			{Service.LevelProgress, new ProgressProvider()}
			#if !UNITY_EDITOR && UNITY_IPHONE
				,{Service.Store, new IosStoreAdapter()}
				,{Service.Analytics, new AnalyticsProxy(new List<Analytics>() {
					new AdjustIOSAdapter()
					//, new UpsightMarketing()
				})}
			#elif !UNITY_EDITOR && UNITY_ANDROID
				,{Service.Store, new AndroidStoreAdapter()}
				,{Service.Analytics, new AnalyticsProxy(new List<Analytics>() {
					new AdjustAndroidAdapter()
					//, new UpsightMarketing()
				})}
			#elif UNITY_EDITOR
				,{Service.Store, new MockStoreProvider()}
				,{Service.Analytics, new AnalyticsProxy( new List<Analytics> {
					new MockAnalyticsProvider()
				})}
			#endif
		});

		ServiceLocator.GetUpsight().LateInit();
		ServiceLocator.GetAnalytics().LateInit();
	}
	
	void Start()
    {
		if ( ServiceLocator.GetDB().GetBool( LEFT_IN_GAME_FLAG , false ) ) {
			ServiceLocator.GetDB().Remove( LEFT_IN_GAME_FLAG );
			ServiceLocator.GetLife().SpendLife();
		}
	}
	
	private void OnApplicationPause(bool pauseStatus) {
		ServiceLocator.GetDB().Flush();
        
	}

	private void OnApplicationQuit()
	{
		ServiceLocator.Destroy();
	}
	
	public void ToggleSound()
	{
		bool current = ServiceLocator.GetSettings().GetCurrentSFXState();

		ServiceLocator.GetSettings().SetCurrentSFXState(!current);
		ServiceLocator.GetSettings().SetCurrentMusicState(!current);
	}
}
