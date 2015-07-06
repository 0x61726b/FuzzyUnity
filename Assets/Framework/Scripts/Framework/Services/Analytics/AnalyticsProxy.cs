using System.Collections.Generic;

namespace com.gramgames.analytics {
    class AnalyticsProxy : Analytics {
        List<Analytics> implementations;
        public AnalyticsProxy( List<Analytics> implementations ) {
            this.implementations = implementations;
        }

		public void Awake() {
			
		}

        public void Start() {
            foreach ( Analytics impl  in implementations ) {
                impl.Start();
            }
        }

        public void LateInit() {
            foreach ( Analytics impl  in implementations ) {
                impl.LateInit();
            }
        }

        public void Destroy() {
            foreach ( Analytics impl  in implementations ) {
                impl.Destroy();
            }
        }

        public void TrackEvent( AnalyticEvent analyticEvent, Dictionary<string, string> parameters = null ) {
            foreach ( Analytics impl  in implementations ) {
                impl.TrackEvent( analyticEvent , parameters );
            }
        }

        public void TrackRevenue( double amount ) {
            foreach ( Analytics impl  in implementations ) {
                impl.TrackRevenue( amount );
            }
        }

        public void TrackCustomEvent( AnalyticEvent analyticEvent, string customTag, Dictionary<string, string> parameters = null ) {
            foreach ( Analytics impl  in implementations ) {
                impl.TrackCustomEvent( analyticEvent , customTag , parameters );
            }
        }
    }
}
