using System.Collections.Generic;

namespace com.gramgames.analytics {
    class MockAnalyticsProvider : Analytics {
        private const string INITIAL_EVENT_KEY = "analyticsInitEventSent";

		public void Awake() {
			
		}

        public void Start() {
            UnityEngine.Debug.Log( "[Analytics] Mock Analytics starting." );
        }

        public void LateInit() {
            UnityEngine.Debug.Log( "[Analytics] Mock Analytics late inited." );
        }

        public void TrackEvent( AnalyticEvent analyticEvent, Dictionary<string, string> parameters = null ) {
            string logString = "[Analytics] Tracking Event: " + analyticEvent + " with params: ";
            if ( parameters != null ) {
                foreach ( KeyValuePair<string,string> pair in parameters ) {
                    logString += "\n" + pair.Key + " : " + pair.Value;
                }
            } else {
                logString += "none";
            }
            UnityEngine.Debug.Log( logString );
        }

        public void TrackCustomEvent( AnalyticEvent analyticEvent, string customTag, Dictionary<string, string> parameters = null ) {
            string logString = "[Analytics] Tracking Event: " + analyticEvent + "_" + customTag + " with params: ";
            if ( parameters != null ) {
                foreach ( KeyValuePair<string,string> pair in parameters ) {
                    logString += "\n" + pair.Key + " : " + pair.Value;
                }
            } else {
                logString += "none";
            }
            UnityEngine.Debug.Log( logString );
        }

        public void TrackRevenue(double amount) {
            TrackEvent(AnalyticEvent.REVENUE, new Dictionary<string,string>(){{"amount", amount.ToString()}});
        }

        public void Destroy() {

        }
    }
}
