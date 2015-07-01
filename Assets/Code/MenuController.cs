//.     .       .  .   . .   .   . .    +  .
//  .     .  :     .    .. :. .___---------___.
//       .  .   .    .  :.:. _".^ .^ ^.  '.. :"-_. .
//    .  :       .  .  .:../:            . .^  :.:\.
//        .   . :: +. :.:/: .   .    .        . . .:\
// .  :    .     . _ :::/:               .  ^ .  . .:\
//  .. . .   . - : :.:./.                        .  .:\
//  .      .     . :..|:                    .  .  ^. .:|
//    .       . : : ..||        .                . . !:|
//  .     . . . ::. ::\(                           . :)/
// .   .     : . : .:.|. ######              .#######::|
//  :.. .  :-  : .:  ::|.#######           ..########:|
// .  .  .  ..  .  .. :\ ########          :######## :/
//  .        .+ :: : -.:\ ########       . ########.:/
//    .  .+   . . . . :.:\. #######       #######..:/
//     :: . . . . ::.:..:.\           .   .   ..:/
//  .   .   .  .. :  -::::.\.       | |     . .:/
//     .  :  .  .  .-:.":.::.\             ..:/
// .      -.   . . . .: .:::.:.\.           .:/
//.   .   .  :      : ....::_:..:\   ___.  :/
//   .   .  .   .:. .. .  .: :.:.:\       :/
//     +   .   .   : . ::. :.:. .:.|\  .:/|
//     .         +   .  .  ...:: ..|  --.:|
//.      . . .   .  .  . ... :..:.."(  ..)"
// .   .       .      :  .   .: ::/  .  .::\


//      __       ___  ___  ___  ___      ___       ___      ___       __        ______    
//     /""\     |"  \/"  ||"  \/"  |    |"  |     |"  \    /"  |     /""\      /    " \   
//    /    \     \   \  /  \   \  /     ||  |      \   \  //   |    /    \    // ____  \  
//   /' /\  \     \\  \/    \\  \/      |:  |      /\\  \/.    |   /' /\  \  /  /    ) :) 
//  //  __'  \    /   /     /   /        \  |___  |: \.        |  //  __'  \(: (____/ //  
// /   /  \\  \  /   /     /   /        ( \_|:  \ |.  \    /:  | /   /  \\  \\        /   
//(___/    \___)|___/     |___/          \_______)|___|\__/|___|(___/    \___)\"_____/    
//--------------------------------------------------------------------------------
using UnityEngine.Advertisements;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using GooglePlayGames;
//--------------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
#endif
//--------------------------------------------------------------------------------
public class MenuController : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    public int m_iScore;
    public Text ScoreText;
    public Text BestScoreText;
    public Text CurrentScoreText;
    
    //--------------------------------------------------------------------------------
    public GameLogic m_GameLogic;
    //--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------
    bool achievementsProcessed;
    int gamesPlayed;
    public void Start()
    {

        achievementsProcessed = false;

        m_iScore = 0;
        if (PlayerPrefs.GetInt("BestScore").ToString() != null)
        {
            BestScoreText.text = "0";
        }
        BestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
    }
    //--------------------------------------------------------------------------------
    void Update()
    {
        if(GameLogic.State == GameLogic.GameState.Ended && !achievementsProcessed){
            
            gamesPlayed = PlayerPrefs.GetInt("GamesPlayed" , 0);
            gamesPlayed += 1;
            PlayerPrefs.SetInt("GamesPlayed",gamesPlayed);
            int wth = PlayerPrefs.GetInt("WTH",0);


            //-------------------------------------
            if(wth == 0){
                Social.ReportProgress("CgkIzs-alcMYEAIQDQ", 100.0f, (bool success) =>
                {
                    if (success)
                    {
                        PlayerPrefs.SetInt("WTH",1);
                    }
                });
            }

            //------------------------------------
            if(m_iScore > 9){
                Social.ReportProgress("CgkIzs-alcMYEAIQAg", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (m_iScore > 24)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQAw", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (m_iScore > 74)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQBA", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (m_iScore > 149)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQBQ", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (m_iScore > 249)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQBg", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
             if (m_iScore > 999)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQBw", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            
            //---------------------------------------
            if(gamesPlayed > 4 && gamesPlayed < 20){
                Social.ReportProgress("CgkIzs-alcMYEAIQCA", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (gamesPlayed > 19 && gamesPlayed < 50)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQCQ", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (gamesPlayed > 49 && gamesPlayed < 100)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQCg", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (gamesPlayed > 99 && gamesPlayed < 250)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQCw", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            if (gamesPlayed > 249)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQDA", 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            achievementsProcessed = true;


            
           
        }

    }
    //--------------------------------------------------------------------------------
    public void RestartLevel()
    {
        achievementsProcessed = false;
        m_GameLogic.Restart();
    }
    //--------------------------------------------------------------------------------
    public void UpdateScoreboard(int iScore)
    {
        m_iScore = iScore;
        CurrentScoreText.text = m_iScore.ToString();
        ScoreText.text = CurrentScoreText.text;
        if (m_iScore > System.Convert.ToInt32(BestScoreText.text))
        {
            PlayerPrefs.SetInt("BestScore", m_iScore);
            BestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
            
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
    //--------------------------------------------------------------------------------
}
