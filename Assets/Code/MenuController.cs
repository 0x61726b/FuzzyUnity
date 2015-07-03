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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using GooglePlayGames;
//--------------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SocialPlatforms;
#endif
//--------------------------------------------------------------------------------
public class MenuController : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    public int m_iScore;
    public Text ScoreText;
    public Text BestScoreText;
    public Text CurrentScoreText;

    public Button muteButton;
    public Sprite mute1;
    public Sprite mute2;

    AudioSource audio;
    bool muted;
    private bool outOfLoop = false;
    //--------------------------------------------------------------------------------
    public GameLogic m_GameLogic;
    public AchievementHandler m_AchvHandler;
    //--------------------------------------------------------------------------------
    bool achievementsProcessed;
    int gamesPlayed;
    public void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {

        });
        muted = false;
        audio = GetComponent<AudioSource>();
        achievementsProcessed = false;

        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            ToogleMute();
        }

        m_iScore = 0;


        gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);


        Social.LoadScores("CgkIzs-alcMYEAIQAQ", hue =>
            {
                foreach (var x in hue)
                {
                    if (x.userID == Social.localUser.id)
                    {
                        long score = x.value;
                        int localScore = PlayerPrefs.GetInt("BestScore", 0);

                        if (score > localScore)
                        {
                            PlayerPrefs.SetInt("BestScore", (int)score);
                        }
                        

                    }
                }
            });
        BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    //--------------------------------------------------------------------------------
    void Update()
    {

        if (GameLogic.State == GameLogic.GameState.Ended && !outOfLoop)
        {
            audio.Play();

            gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
            gamesPlayed += 1;
            PlayerPrefs.SetInt("GamesPlayed", gamesPlayed);

            Analytics.CustomEvent("GamesPlayed", new Dictionary<string, object>
                {
                    { "GamesPlayed",gamesPlayed },
                });
            outOfLoop = true;
            HandleAchievements();

            int wth = PlayerPrefs.GetInt("WTH", 0);


            //-------------------------------------
            if (wth == 0)
            {
                Social.ReportProgress("CgkIzs-alcMYEAIQDQ", 100.0f, (bool success) =>
                {
                    if (success)
                    {
                        PlayerPrefs.SetInt("WTH", 1);
                    }
                });
            }
        }

    }
    //--------------------------------------------------------------------------------
    public void RestartLevel()
    {
        m_GameLogic.Restart();
        outOfLoop = false;
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

        HandleAchievements();
    }
    public void HandleAchievements()
    {
        //------------------------------------
        if (m_iScore > 9 && m_iScore <= 24)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Oh! I got this - Highscore 10"));
        }
        if (m_iScore > 24 && m_iScore <= 74)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Getting the hang of it"));
        }
        if (m_iScore > 74 && m_iScore <= 149)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Ambitious"));
        }
        if (m_iScore > 149 && m_iScore <= 249)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Tier 0"));
        }
        if (m_iScore > 249 && m_iScore <= 999)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Superior Kind"));
        }
        if (m_iScore > 999)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Freak"));
        }

        //---------------------------------------

        //---------------------------------------
        if (gamesPlayed > 4 && gamesPlayed < 20)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Newcomer"));
        }
        if (gamesPlayed > 19 && gamesPlayed < 50)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Rookie"));
        }
        if (gamesPlayed > 49 && gamesPlayed < 100)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Regular"));
        }
        if (gamesPlayed > 99 && gamesPlayed < 250)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Citizen"));
        }
        if (gamesPlayed > 249)
        {
            m_AchvHandler.Process(m_AchvHandler.Achievements.Find(x => x.Name == "Leecher"));
        }
    }
    public void ToogleMute()
    {

        audio.volume = muted ? 100 : 0;
        muteButton.image.sprite = muted ? mute1 : mute2;
        if (!muted)
        {
            PlayerPrefs.SetInt("Mute", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 0);
        }

        muted = !muted;



    }
    //--------------------------------------------------------------------------------
}
