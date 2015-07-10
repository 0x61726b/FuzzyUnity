using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Assets.Scripts.Framework.Services;
using Assets.Scripts.Framework.Util;

class NotificationProvider : Notification
{
    private const int ID_1D_NOTIFICATION = 1;
    private const int ID_7D_NOTIFICATION = 7;

    private const string KEY_D1_TIMESTAMP = "n_day1timestamp";
    private const string KEY_D7_TIMESTAMP = "n_day7timestamp";


    private bool dailyNotificationsEnabled = true;


    private const double d1NotificationMinutes = 60 * 24; // 1 day
    private const double d7NotificationMinutes = 60 * 24 * 7; // 7 days

#if UNITY_ANDROID
    private long day1timestamp;
    private long day7timestamp;
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _EnableLocalNotificationIOS8();
 
    void RegisterNotificationService() {
        _EnableLocalNotificationIOS8();
    }
#endif

    public void Start()
    {
#if UNITY_ANDROID
        day1timestamp = ServiceLocator.GetDB().GetLong(KEY_D1_TIMESTAMP, 0);
        day7timestamp = ServiceLocator.GetDB().GetLong(KEY_D7_TIMESTAMP, 0);
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
        RegisterNotificationService();
#endif
#if UNITY_IPHONE
        ClearBadge();
#endif
        CancelAll();

        ServiceLocator.GetSettings().AddDayNotificationsStateListener(OnDailyNotificationSettingChange);

        dailyNotificationsEnabled = ServiceLocator.GetSettings().GetCurrentDayNotificationsState();


        ServiceLocator.RegisterPauseListener(OnPause);
    }

    private void OnDailyNotificationSettingChange(bool state)
    {
        dailyNotificationsEnabled = state;
    }
    public void Awake()
    {

    }

    public void Destroy()
    {
        ServiceLocator.RemovePauseListener(OnPause);
        CancelAll();
        setNotifications();
#if UNITY_ANDROID
        ServiceLocator.GetDB().SetLong(KEY_D1_TIMESTAMP, day1timestamp);
        ServiceLocator.GetDB().SetLong(KEY_D7_TIMESTAMP, day7timestamp);

        ServiceLocator.GetDB().Flush();
#endif
    }

    private void setNotifications()
    {
        if (dailyNotificationsEnabled)
        {
            sendDayNotifications();
        }

    }

    public void OnPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            setNotifications();
        }
        else
        {
#if UNITY_IPHONE
            CancelAll();
            ClearBadge();
#elif UNITY_ANDROID
            CancelAll();
#endif
        }
    }

   
    private void sendDayNotifications()
    {
        send1dNotification();
        send7dNotification();
    }

    private void ClearBadge()
    {
#if UNITY_IPHONE
        LocalNotification l = new LocalNotification ();
        l.applicationIconBadgeNumber = -1;
        NotificationServices.PresentLocalNotificationNow ( l );
#endif
    }

    private void CancelAll()
    {
#if UNITY_IPHONE
        NotificationServices.CancelAllLocalNotifications();
#elif UNITY_ANDROID
        if (day1timestamp > Util.GetTime())
        {
            ELANManager.CancelLocalNotification(ID_1D_NOTIFICATION);
            day1timestamp = 0;
        }
        if (day7timestamp > Util.GetTime())
        {
            ELANManager.CancelLocalNotification(ID_7D_NOTIFICATION);
            day7timestamp = 0;
        }
#endif
    }

    private void send1dNotification()
    {
       

        string message = ServiceLocator.GetLocalization().GetString(UnityEngine.Random.value < 0.5f ? Dict.NOTIF_1D_1 : Dict.NOTIF_1D_2);
        //DateTime targetTime = DateTime.Now.AddHours( 24 );
        DateTime targetTime = DateTime.Now.AddMinutes(d1NotificationMinutes);
#if UNITY_ANDROID
        day1timestamp = Util.GetTimestampFromDateTime(targetTime);
#endif
        sendTimedNotification(targetTime, message, ID_1D_NOTIFICATION);
    }

    private void send7dNotification()
    {
        

        string message = ServiceLocator.GetLocalization().GetString(Dict.NOTIF_7D);
        //DateTime targetTime = DateTime.Now.AddDays( 7 );
        DateTime targetTime = DateTime.Now.AddMinutes(d7NotificationMinutes);
#if UNITY_ANDROID
        day7timestamp = Util.GetTimestampFromDateTime(targetTime);
#endif
        sendTimedNotification(targetTime, message, ID_7D_NOTIFICATION);
    }

    private void sendTimedNotification(DateTime targetTime, string body, int id)
    {
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
        notification.title = ServiceLocator.GetLocalization().GetString(Dict.NOTIF_TITLE);
        notification.fullClassName = "com.prime31.UnityPlayerNativeActivity";
        notification.message = body;
        notification.setFireDate(targetTime);
        notification.advancedNotification = true;
        notification.useSound = false;
        notification.useVibration = false;
        notification.send();
        Debug.Log("HUEHEEHUEU");
        UnityEngine.Object.Destroy(notification);
        UnityEngine.Object.Destroy(newGameObject);
        notification = null;
#endif
    }
}