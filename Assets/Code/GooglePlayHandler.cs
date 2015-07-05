using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

   

public class GooglePlayHandler : MonoBehaviour {

	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowLeaderboard()
    {
        if(!Social.localUser.authenticated){
             Social.localUser.Authenticate((bool success) =>
        {

        });
             PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzs-alcMYEAIQAQ");
        }
        else
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzs-alcMYEAIQAQ");
        }
       
    }

    public void ShowAchievements()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {

            });
            Social.ShowAchievementsUI();
        }
        else
        {
            Social.ShowAchievementsUI();
        }
       
    }
}
