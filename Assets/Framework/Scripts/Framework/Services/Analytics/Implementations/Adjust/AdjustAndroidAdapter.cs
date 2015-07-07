using com.adjust.sdk;
using System.Collections.Generic;

namespace com.gramgames.analytics {
class AdjustAndroidAdapter : Analytics {
    private const string ADJUST_APP_TOKEN = "ptsr3pexssax";
    private AdjustAndroid Adjust;
    private static Dictionary<AnalyticEvent, string> tokenMap = new Dictionary<AnalyticEvent, string>() {
        { AnalyticEvent.PURCHASE, "" }
    };

    public void LateInit() {

    }
	
	public void Awake() {
		
	}
	
    public void Start() {
        Adjust = new AdjustAndroid();
		Adjust.appDidLaunch(ADJUST_APP_TOKEN, AdjustUtil.AdjustEnvironment.Sandbox,"",AdjustUtil.LogLevel.Debug,true);
    }

    public void TrackEvent(AnalyticEvent analyticEvent, Dictionary<string, string> parameters = null) {
        if ( tokenMap.ContainsKey(analyticEvent) ) {
            Adjust.trackEvent(tokenMap[analyticEvent], parameters);
        }
    }

    public void TrackRevenue(double amount) {
        //Adjust.trackRevenue(amount * 100, REVENUE_EVENT_TOKEN);
    }

    public void TrackCustomEvent(AnalyticEvent analyticEvent, string customTag, Dictionary<string, string> parameters = null) {
        if ( parameters == null ) {
            parameters = new Dictionary<string, string>();
        }
        parameters.Add("tag", customTag);

        TrackEvent(analyticEvent, parameters);
    }

    public void Destroy() {

    }
}
}
