using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.Advertisements;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MenuController : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public Text bestScoreText;
    public Text currentScoreText;

    bool dataSent;

    void Start()
    {
        Advertisement.Initialize("49224");
        
        dataSent = false;
        score = 0;
        if (PlayerPrefs.GetInt("BestScore").ToString() != null)
        {
            bestScoreText.text = "0";
        }
        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RestartLevel()
    {
        GameLogic.State = GameLogic.GameState.NotStarted;
        
        Application.LoadLevel("MainScene");
      
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void UpdateScoreboard()
    {
        
        currentScoreText.text = score.ToString();
        scoreText.text = currentScoreText.text;
        if (score > System.Convert.ToInt32(bestScoreText.text))
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        }
        //if (!dataSent && GameLogic.State == GameLogic.GameState.Ended)
        //{
        //    Analytics.CustomEvent("GameOver", new Dictionary<string, object>
        //     {
        //      { "Score", score },
        //      { "Best Score", PlayerPrefs.GetInt("BestScore") },
       
        //      });
        //    //if (Advertisement.isReady()) { Advertisement.Show(); }
        //    dataSent = true;

        //}

    }
    public void IncrementScore()
    {
        score += 1;
        UpdateScoreboard();
    }

}
