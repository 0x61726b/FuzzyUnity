using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Services.Store;
using com.gramgames.analytics;
using Assets.Scripts.Framework.Util;
using Assets.Scripts.Framework.Services.AudioService;
using System.Collections.Generic;

#if !UNITY_EDITOR && UNITY_IPHONE
using Assets.Scripts.Framework.Services.Store.IosStoreAdapter;
#elif !UNITY_EDITOR && UNITY_ANDROID
using Assets.Scripts.Framework.Services.Store.AndroidStoreAdapter;
#endif

public class UpsightPluginTest : MonoBehaviour
{
    public const string LEFT_IN_GAME_FLAG = "game_wasInGame";
    public bool inGame = false;
    public long gameStart;
    public static string APPLICATION_VERSION = "xD";

    void Awake()
    {
        ServiceLocator.Boot(new Dictionary<Service, Provider> {
			{Service.DB, new FilePersistence()},
			{Service.Settings, new SettingsProvider()},
			{Service.Audio, new AudioProvider()},
			{Service.Localization, new LocalizationProvider()},
            //{Service.Achievement, new AchievementProvider()},
            //{Service.Notification, new NotificationProvider()},
            //{Service.Life, new LifeProvider()},
			{Service.UAnalytics, new UpsightAnalytics()}
            //{Service.LevelProgress, new ProgressProvider()}
			#if !UNITY_EDITOR && UNITY_IPHONE
				,{Service.Store, new IosStoreAdapter()}
				,{Service.Analytics, new AnalyticsProxy(new List<Analytics>() {
					new AdjustIOSAdapter()
					//, new UpsightMarketing()
				})}
			#elif !UNITY_EDITOR && UNITY_ANDROID
            ,{Service.Store, new MockStoreProvider()}
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
    public void TapToStartEvent()
    {
        ServiceLocator.GetUpsight().TrackEvent("player", "game", "casual", "start", "", null);
    }
    public void GameOverEvent(int bracket,int score,int duration,string diff)
    {
        ServiceLocator.GetUpsight().TrackEvent("player", "game", "casual", "finish", score.ToString(), new Dictionary<string, string>
            {
               { "Passed_Brackets",bracket.ToString() },
               { "Duration",duration.ToString() },
               { "Difficulty",diff },
            });
        Debug.Log("Score: " + score.ToString() +
            " " + "Passed_Brackets: " + bracket.ToString() + " " + "Duration: " + duration.ToString() + " Diffucluty: " + diff);
    }
    void Start()
    {
        if (ServiceLocator.GetDB().GetBool(LEFT_IN_GAME_FLAG, false))
        {
            ServiceLocator.GetDB().Remove(LEFT_IN_GAME_FLAG);
            ServiceLocator.GetLife().SpendLife();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        ServiceLocator.GetDB().Flush();
    }

    private void OnApplicationQuit()
    {
        ServiceLocator.Destroy();
    }
	
	
}