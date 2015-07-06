using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

namespace com.gramgames.analytics {
    interface Analytics : Provider {
        void LateInit();
        void TrackEvent( AnalyticEvent analyticEvent, Dictionary<string, string> parameters = null );
        void TrackCustomEvent( AnalyticEvent analyticEvent, string customTag, Dictionary<string, string> parameters = null );
        void TrackRevenue( double revenue );
    }
}
