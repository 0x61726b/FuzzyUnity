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
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIzs-alcMYEAIQAQ");
    }

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }
}
