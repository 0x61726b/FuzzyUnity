using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MenuController : MonoBehaviour {
    
    public Text score;
    public Text bestScore;
    public Text currentScore;

    public Logic logic;
	void Start () {
        if(PlayerPrefs.GetInt("BestScore").ToString() != null){
            bestScore.text = "0";
        }
        bestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RestartLevel()
    {
        Time.timeScale = 1;
        Application.LoadLevel("MainScene");

    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void updateScoreboard() {
        score.text = currentScore.text;
        if (System.Convert.ToInt32(score.text) > System.Convert.ToInt32(bestScore.text))
        {
            PlayerPrefs.SetInt("BestScore", System.Convert.ToInt32(score.text));
            bestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
        }
            
    }

}
