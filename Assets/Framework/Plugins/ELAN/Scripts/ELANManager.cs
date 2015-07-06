using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ELANManager {
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject playerActivityContext = null;
	private static List<int> currentNotificationIDs = new List<int>();
#endif
	//private static string PREF_KEY = "ELANPlayerPrefsKey";
	
	
	
	public static void sendParametrizedNotification (ELANNotification notification){
#if UNITY_ANDROID && !UNITY_EDITOR
		Debug.Log("public static void sendParametrizedNotification (ELANNotification notification) --> " + notification.fullClassName );
		// Obtain unity context
        if(playerActivityContext == null) {
			AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		
        // Set notification within java plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.CFM.ELAN.ELANManager");
		
        if (pluginClass != null) {
			Debug.Log("FireNotification --> " + notification.fullClassName );
			pluginClass.CallStatic("FireNotification", playerActivityContext, notification.title,notification.message,notification.getDelay(),notification.ID, notification.fullClassName,notification.useSound,notification.soundName,notification.useVibration,notification.getRepetition());
			if(notification.getDelay() > 0) { // NEW forgot to add this to be able to cancel notification
				currentNotificationIDs.Add (notification.ID);
			}
       }
#endif
	}
	
	
	
	
	
	 [System.Obsolete("sendParametrizedNotification (ELANNotification notification)", true)]
	public static int SendNotification (string title, string message, long delay) {
#if UNITY_ANDROID && !UNITY_EDITOR
		//if(currentNotificationIDs.Count == 0) currentNotificationIDs.AddRange(GetIntArray());
		// Obtain unity context
        if(playerActivityContext == null) {
			AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		
        // Set notification within java plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.CFM.ELAN.ELANManager");
        int nID = (int)(Time.time*1000) + (int)Random.Range (0,int.MaxValue/2);
        if (pluginClass != null) {
			pluginClass.CallStatic("FireNotification", playerActivityContext, title,message,delay,nID);
			if(delay > 0) {
				currentNotificationIDs.Add (nID);
			}
        }
		return nID;
#endif
		return -1;
	}
	 [System.Obsolete("sendParametrizedNotification (ELANNotification notification)", true)]
	public static void ScheduleRepeatingNotification (string title, string message, long delay, long rep) {
#if UNITY_ANDROID && !UNITY_EDITOR
		// Obtain unity context
        if(playerActivityContext == null) {
			AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
        // Set notification within java plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.CFM.ELAN.ELANManager");
        if (pluginClass != null) {
            pluginClass.CallStatic("ScheduleRepeatingNotification", playerActivityContext,title,message,delay,rep);
        }
#endif
	}
	
	public static void CancelRepeatingNotification () {
#if UNITY_ANDROID && !UNITY_EDITOR
		// Obtain unity context
        if(playerActivityContext == null) {
			AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
        // Set notification within java plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.CFM.ELAN.ELANManager");
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelRepeatingNotification", playerActivityContext);
        }
#endif
	}
	
	public static void CancelAllNotifications () {
#if UNITY_ANDROID && !UNITY_EDITOR
		//currentNotificationIDs.AddRange(GetIntArray());
		if(currentNotificationIDs.Count == 0) return;
		List<int> copy = new List<int>();
		copy.AddRange (currentNotificationIDs);
		foreach(int nID in copy)
            CancelLocalNotification(nID);
		//SetIntArray(currentNotificationIDs.ToArray());
		CancelRepeatingNotification();
#endif
	}
	
	public static void CancelLocalNotification (int nID) {
#if UNITY_ANDROID && !UNITY_EDITOR
		// Obtain unity context
        if(playerActivityContext == null) {
			AndroidJavaClass actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
        // Set notification within java plugin
        AndroidJavaClass pluginClass = new AndroidJavaClass("com.CFM.ELAN.ELANManager");
        if (pluginClass != null) {
			pluginClass.CallStatic("CancelNotification", playerActivityContext,nID);
			currentNotificationIDs.Remove (nID);
        }
#endif
		

	}
	// NEW not being called anywhere!
	#if UNITY_ANDROID && !UNITY_EDITOR
	public static void addNotificationId(int nID){
		if ( currentNotificationIDs != null )currentNotificationIDs.Add(nID);
	}
#endif
	// HELPER FUNCTIONS
	// This code is not used anymore due to a funny error on unity when trying to remove notifications from previous sessions
	// I leave it here so users can build their own notification tracking system from it
	/*public static void SetIntArray (int[] intArray) {
		if (intArray.Length == 0) {
			PlayerPrefs.DeleteKey (PREF_KEY);
			return;
		}
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < intArray.Length-1; i++) {
			sb.Append(intArray[i]).Append("|");
		}
		sb.Append(intArray[intArray.Length-1]);
	 
		try {
			PlayerPrefs.SetString(PREF_KEY, sb.ToString());
		}
		catch (System.Exception err) {
			Debug.Log (err.ToString ());
		}
	}
	 
	public static int[] GetIntArray () {
		if (PlayerPrefs.HasKey(PREF_KEY)) {
			string[] stringArray = PlayerPrefs.GetString(PREF_KEY).Split("|"[0]);
			Debug.Log (PlayerPrefs.GetString(PREF_KEY));
			int[] intArray = new int[stringArray.Length];
			Debug.Log (intArray.Length);
			for (int i = 0; i < stringArray.Length; i++) {
				intArray[i] = int.Parse(stringArray[i]);
			}
			return intArray;
		}
		return new int[0];
	}*/
	
	
 
}
