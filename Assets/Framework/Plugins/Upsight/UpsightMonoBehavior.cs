using UnityEngine;
using System.Collections;


public class UpsightMonoBehavior : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	public string androidAppToken;
	public string androidAppSecret;
	public string gcmProjectNumber;
	public string iosAppToken;
	public string iosAppSecret;
	public bool registerForPushNotifications = false;


	void Start()
	{
#if UNITY_ANDROID
		Upsight.init( androidAppToken, androidAppSecret, gcmProjectNumber );
#else
		Upsight.init( iosAppToken, iosAppSecret );
#endif

		Upsight.requestAppOpen();

		if( registerForPushNotifications )
			Upsight.registerForPushNotifications();
	}
#endif
}
