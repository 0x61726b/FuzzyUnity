#if UNITY_ANDROID
using GooglePlayGames;
#endif
using UnityEngine;
using UnityEngine.SocialPlatforms;



public class GooglePlayHandler : MonoBehaviour
{


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowLeaderboard()
    {
        if (!Social.localUser.authenticated)
        {

            Social.localUser.Authenticate((bool success) =>
                { });
        }
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzs-alcMYEAIQAQ");
#endif
    }

    public void ShowAchievements()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {

            });
        }
#if UNITY_ANDROID
        Social.ShowAchievementsUI();
#endif

    }
}
