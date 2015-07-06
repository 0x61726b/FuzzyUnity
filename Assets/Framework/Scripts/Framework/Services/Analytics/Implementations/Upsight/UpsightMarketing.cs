//#define UPSIGHT_MARKETING_QA
using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

namespace com.gramgames.analytics {
    class UpsightMarketing : Analytics {
#if UNITY_ANDROID && UPSIGHT_MARKETING_QA
    public string appToken = "b5d5077078f940bca9f62b95a704fdf0";
    public string appSecret = "d38b8da15322409584e98f06b59e7438";
#elif UNITY_IPHONE && UPSIGHT_MARKETING_QA
    public string appToken = "036d51da7de84ff38d440c48a69479d0";
    public string appSecret = "81fc4a20a180432384cbe53720cab800";
#elif UNITY_ANDROID && !UPSIGHT_MARKETING_QA
    public string appToken = "fe30694973634b3793960bdd739734ef";
    public string appSecret = "0aa43f461d0e404dabfd45ce30ced2c5";
#elif UNITY_IPHONE && !UPSIGHT_MARKETING_QA
        public string appToken = "0761ba93d4d34fe7aa6867ba14796311";
        public string appSecret = "013edb4675b74c7487e0d62e4571202a";
#endif

		public void Awake()
		{
			
		}

        public void LateInit() {
#if UNITY_IPHONE || UNITY_ANDROID
            Upsight.init(appToken, appSecret);
            Upsight.requestAppOpen();
            Upsight.registerForPushNotifications();
#endif
        }

        public void Start() {
            ServiceLocator.RegisterPauseListener(OnPause);
        }

        void OnPause(bool pauseStatus) {
#if UNITY_IPHONE || UNITY_ANDROID
            // Make an open request whenever app is resumed
            if(!pauseStatus) {
                Upsight.requestAppOpen();
            }
#endif
        }

        public void TrackEvent(AnalyticEvent analyticEvent, Dictionary<string, string> parameters = null) {
        }

        public void TrackRevenue(double revenue) {
        }

        public void TrackCustomEvent(AnalyticEvent analyticEvent, string customTag, Dictionary<string, string> parameters = null) {
        }

        public void Destroy() {
            ServiceLocator.RemovePauseListener(OnPause);
        }
    }
}
