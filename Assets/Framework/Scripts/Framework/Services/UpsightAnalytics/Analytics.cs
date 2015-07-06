using System.Collections.Generic;

namespace Assets.Scripts.Framework.Services.UpsightAnalytics {
    internal interface Analytics : Provider {
        void TrackRevenue( int amount );
        void TrackEvent( string st1, string st2, string st3, string name, string value, Dictionary<string, string> eventData = null );
    }
}