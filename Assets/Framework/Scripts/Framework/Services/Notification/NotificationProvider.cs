using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Util;

class NotificationProvider : Notification {
    private const int ID_1D_NOTIFICATION = 1;
    private const int ID_7D_NOTIFICATION = 7;
    private const int ID_LIFE_FULL_NOTIFICATION = 2;
    private const string KEY_D1_TIMESTAMP = "n_day1timestamp";
    private const string KEY_D7_TIMESTAMP = "n_day7timestamp";
    private const string KEY_LIFE_FULL_TIMESTAMP = "n_lifefulltimestamp";

    private bool dailyNotificationsEnabled = true;
    private bool lifeNotificationsEnabled = true;

    private const double d1NotificationMinutes = 60 * 24; // 1 day
    private const double d7NotificationMinutes = 60 * 24 * 7; // 7 days

    #if UNITY_ANDROID
	private long day1timestamp;
	private long day7timestamp;
	private long lifetimestamp;
	#endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _EnableLocalNotificationIOS8();
 
    void RegisterNotificationService() {
        _EnableLocalNotificationIOS8();
    }
#endif

    public void Start() {
		#if UNITY_ANDROID
		day1timestamp = ServiceLocator.GetDB().GetLong( KEY_D1_TIMESTAMP, 0 );
		day7timestamp = ServiceLocator.GetDB().GetLong( KEY_D7_TIMESTAMP, 0 );
		lifetimestamp = ServiceLocator.GetDB().GetLong( KEY_LIFE_FULL_TIMESTAMP, 0 );
		#endif

#if UNITY_IPHONE && !UNITY_EDITOR
        RegisterNotificationService();
#endif
#if UNITY_IPHONE
        ClearBadge();
#endif
        CancelAll();

		ServiceLocator.GetSettings().AddDayNotificationsStateListener( OnDailyNotificationSettingChange );
		ServiceLocator.GetSettings().AddLifeNotificationsStateListener( OnLifeNotificationSettingChange );
		dailyNotificationsEnabled = ServiceLocator.GetSettings().GetCurrentDayNotificationsState();
		lifeNotificationsEnabled = ServiceLocator.GetSettings().GetCurrentLifeNotificationsState();

		ServiceLocator.RegisterPauseListener( OnPause );
    }

    private void OnDailyNotificationSettingChange( bool state ) {
        dailyNotificationsEnabled = state;
    }

    private void OnLifeNotificationSettingChange( bool state ) {
        lifeNotificationsEnabled = state;
    }

	public void Awake()
	{

	}

    public void Destroy() {
		ServiceLocator.RemovePauseListener( OnPause );
        CancelAll();
        setNotifications();
#if UNITY_ANDROID
		ServiceLocator.GetDB().SetLong( KEY_D1_TIMESTAMP, day1timestamp );
		ServiceLocator.GetDB().SetLong( KEY_D7_TIMESTAMP, day7timestamp );
		ServiceLocator.GetDB().SetLong( KEY_LIFE_FULL_TIMESTAMP, lifetimestamp );
		ServiceLocator.GetDB().Flush();
#endif
    }

    private void setNotifications() {
        if ( dailyNotificationsEnabled ) {
            sendDayNotifications();
        }
        if ( lifeNotificationsEnabled ) {
            sendLifeFullNotification();
        }
    }

    public void OnPause( bool pauseStatus ) {
        if ( pauseStatus ) {
            setNotifications();
        } else {
#if UNITY_IPHONE
            CancelAll();
            ClearBadge();
#elif UNITY_ANDROID
            CancelAll();
#endif
        }
    }

    private void sendLifeFullNotification() {
		string message = ServiceLocator.GetLocalization().GetString( Dict.NOTIF_LIFE_FULL );
		int secondsToFull = ServiceLocator.GetLife().GetTimeToFull();
        if ( secondsToFull > 0 ) {
            DateTime targetTime = DateTime.Now.AddSeconds( secondsToFull );
            sendTimedNotification( targetTime , message , ID_LIFE_FULL_NOTIFICATION );
#if UNITY_ANDROID
            lifetimestamp = Util.GetTimestampFromDateTime( targetTime );
#endif
        }
    }

    private void sendDayNotifications() {
        send1dNotification();
        send7dNotification();
    }

    private void ClearBadge() {
#if UNITY_IPHONE
        LocalNotification l = new LocalNotification ();
        l.applicationIconBadgeNumber = -1;
        NotificationServices.PresentLocalNotificationNow ( l );
#endif
    }

    private void CancelAll() {
#if UNITY_IPHONE
        NotificationServices.CancelAllLocalNotifications();
#elif UNITY_ANDROID
        if( day1timestamp > Util.GetTime() ) {
            ELANManager.CancelLocalNotification( ID_1D_NOTIFICATION );
            day1timestamp = 0;
        }
        if( day7timestamp > Util.GetTime() ) {
            ELANManager.CancelLocalNotification( ID_7D_NOTIFICATION );
            day7timestamp = 0;
        }
        if( lifetimestamp > Util.GetTime() ) {
            ELANManager.CancelLocalNotification( ID_LIFE_FULL_NOTIFICATION );
            lifetimestamp = 0;
        }
#endif
    }

    private void send1dNotification() {
//        int level = ServiceLocator.GetLevelProgress().GetCurrentLevel();

//        LevelData levelData = SaveLoadService.Load( "Level" + level );
//        if( levelData == null ) return;

//        string message = ServiceLocator.GetLocalization().GetString(UnityEngine.Random.value < 0.5f ? Dict.NOTIF_1D_1 : Dict.NOTIF_1D_2, new string[] { level.ToString(), levelData.move.ToString() });
//        //DateTime targetTime = DateTime.Now.AddHours( 24 );
//        DateTime targetTime = DateTime.Now.AddMinutes( d1NotificationMinutes );
//#if UNITY_ANDROID
//        day1timestamp = Util.GetTimestampFromDateTime( targetTime );
//#endif
//        sendTimedNotification( targetTime , message , ID_1D_NOTIFICATION );
    }

    private void send7dNotification() {
//        int level = ServiceLocator.GetLevelProgress().GetCurrentLevel();
		
//        LevelData levelData = SaveLoadService.Load( "Level" + level );
//        if( levelData == null ) return;
		
//        string message = ServiceLocator.GetLocalization().GetString( Dict.NOTIF_7D, new string[] { level.ToString(), levelData.move.ToString() } );
//        //DateTime targetTime = DateTime.Now.AddDays( 7 );
//        DateTime targetTime = DateTime.Now.AddMinutes( d7NotificationMinutes );
//#if UNITY_ANDROID
//        day7timestamp = Util.GetTimestampFromDateTime( targetTime );
//#endif
//        sendTimedNotification( targetTime , message , ID_7D_NOTIFICATION );
    }

    private void sendTimedNotification( DateTime targetTime, string body, int id ) {
#if UNITY_IPHONE
        LocalNotification notif = new LocalNotification();
        notif.alertAction = ServiceLocator.GetLocalization().GetString( Dict.NOTIF_IOS_ACTION );
        notif.alertBody = body;
        notif.hasAction = true;
        notif.fireDate = targetTime;
        notif.applicationIconBadgeNumber = 1;
        NotificationServices.ScheduleLocalNotification( notif );
#elif UNITY_ANDROID
        GameObject newGameObject = new GameObject("NotifTemp");
        ELANNotification notification = newGameObject.AddComponent<ELANNotification>();
        notification.ID = id;
		notification.title = ServiceLocator.GetLocalization().GetString( Dict.NOTIF_TITLE );
		notification.fullClassName = "com.prime31.UnityPlayerNativeActivity";
        notification.message = body;
        notification.setFireDate( targetTime );
        notification.advancedNotification = true;
        notification.useSound = false;
        notification.useVibration = false;
        notification.send();
        UnityEngine.Object.Destroy(notification);
        UnityEngine.Object.Destroy(newGameObject);
        notification = null;
#endif
    }
}