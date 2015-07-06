using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

interface UAnalytics : Provider {
    void LateInit();
    void TrackRevenue( int amount );
    void TrackEvent( string st1, string st2, string st3, string name, string value, Dictionary<string,string> eventData = null );
}