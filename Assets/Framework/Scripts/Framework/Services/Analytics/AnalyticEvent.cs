namespace com.gramgames.analytics {
    enum AnalyticEvent {
        // Revenue
        PURCHASE,
        REVENUE,

        // Level Events
        LEVEL_START,
        LEVEL_SUCCESS,
        LEVEL_FAILED,
        LEVEL_REPLAY_SUCCESS,
        LEVEL_REPLAY_FAILED,

        // Finance
        USE_SECONDWIND,
        USE_BOOSTER,
        LIFE_SPENT,
        LIFE_FILL,
        REWARD_VIDEO,
        ECON_COIN_REWARD,
        ECON_COIN_SPEND,

        // Facebook
        FB_BOUND,
        FB_DATA,
        FB_INVITE,
        FB_LIFE_REQUEST,
        FB_LIFE_FULFILL,

        // Tutorial
        TUTORIAL_STEP,
    }
}
