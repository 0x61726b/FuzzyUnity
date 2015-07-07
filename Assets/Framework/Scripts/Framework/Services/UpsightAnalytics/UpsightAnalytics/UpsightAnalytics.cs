//#define UPSIGHT_ANALYTICS_QA
using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

namespace com.gramgames.analytics {
	class UpsightAnalytics : UAnalytics {
		#if UNITY_ANDROID
		public string API_KEY = "39465a43c42a4f7c91a548266c192f9d";
#elif UNITY_IPHONE
		public string API_KEY = "effb821f93d1406f9a04d5604ddf6f51";
#else
        public string API_KEY = "CEM_SEN_BU_SATIRLARI_OKURKEN_NABER";
		#endif
		
		private const bool TEST_MODE = false;
		private const string ANALYTICS_UQ_ID_KEY = "upsight_uq_userid";
		
		private UpsightSession session;
		
		public void Awake() {
			
		}

		public void Start() {
			
		}
		
		public void LateInit() {
			startSession();
			sendDeviceInfo();
			ServiceLocator.RegisterPauseListener(OnPause);
		}
		
		void OnPause(bool pauseStatus) {
			if ( pauseStatus ) {
				#if !UNITY_EDITOR
				KontagentBinding.stopSession();
				#endif
			} else {
				startSession();
			}
		}
		
		public void Destroy() {
			ServiceLocator.RemovePauseListener(OnPause);
		}
		
		// amount in kurus
		public void TrackRevenue(int amount) {
			#if !UNITY_EDITOR
			KontagentBinding.revenueTracking(amount, null);
			#endif
		}
		
		public void TrackEvent(string st1, string st2, string st3, string name, string val, Dictionary<string, string> eventData = null) {
			Dictionary<string, string> customEvent = new Dictionary<string, string>();
			customEvent.Add("st1", st1);
			customEvent.Add("st2", st2);
			customEvent.Add("st3", st3);
			customEvent.Add("n", name);
			customEvent.Add("v", val);
			
			
			Dictionary<string, string> sessionData;
			if ( session != null ) {
				sessionData = session.GetSummary();
                //customEvent.Add("l", session.Level.ToString() );
			} else {
				sessionData = new Dictionary<string, string>();
			}
			
			if ( eventData != null ) {
				foreach ( KeyValuePair<string, string> pair in eventData ) {
					sessionData.Add(pair.Key, pair.Value);
				}
			}
			
			customEvent.Add("data", MiniJSON.Json.Serialize(sessionData) );
			#if !UNITY_EDITOR
			KontagentBinding.customEvent(name, customEvent);
			#endif
		}
		
		void sendDeviceInfo() {
			Dictionary<string, string> optionalParams = new Dictionary<string, string>();
            optionalParams.Add("v_maj", UpsightPluginTest.APPLICATION_VERSION.ToString());
			#if !UNITY_EDITOR
			KontagentBinding.sendDeviceInformation(optionalParams);
			#endif
		}
		
		void startSession() {
			if ( session != null ) {
				session.EndSession();
			}
			session = new UpsightSession();
			#if !UNITY_EDITOR
			KontagentBinding.startSession(API_KEY, TEST_MODE, session.UserUniqueId, true, null, null, null, null, true);
			#endif
		}
	}
}
