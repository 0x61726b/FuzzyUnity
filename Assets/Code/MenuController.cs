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

    public Button muteButton;
    public Sprite mute1;
    public Sprite mute2;

    AudioSource audio;
    bool muted;
    private bool outOfLoop = false;
    //--------------------------------------------------------------------------------
    public GameLogic m_GameLogic;
    public AchievementHandler m_AchvHandler;
    private string m_sUserKey;
    //--------------------------------------------------------------------------------
    bool achievementsProcessed;
    int gamesPlayed;
    public void Start()
    {
        muted = false;
        audio = GetComponent<AudioSource>();
        achievementsProcessed = false;

        m_iScore = 0;
        m_sUserKey = Social.localUser.id;
        
        BestScoreText.text = PlayerPrefs.GetInt("BestScore" + m_sUserKey , 0).ToString();
        gamesPlayed = PlayerPrefs.GetInt("GamesPlayed" + m_sUserKey,0);
    }
    //--------------------------------------------------------------------------------
    void Update()
    {
        if (GameLogic.State == GameLogic.GameState.Ended && !outOfLoop)
        {
            audio.Play();

            gamesPlayed = PlayerPrefs.GetInt("GamesPlayed"+m_sUserKey, 0);
            gamesPlayed += 1;
            PlayerPrefs.SetInt("GamesPlayed"+m_sUserKey, gamesPlayed);

            Analytics.CustomEvent("GamesPlayed", new Dictionary<string, object>
                {
                    { "GamesPlayed",gamesPlayed },
                });
            outOfLoop = true;
            HandleAchievements();
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
            PlayerPrefs.SetInt("BestScore"+m_sUserKey, m_iScore);
            BestScoreText.text = PlayerPrefs.GetInt("BestScore"+m_sUserKey).ToString();

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

        int wth = PlayerPrefs.GetInt("WTH"+m_sUserKey, 0);


        //-------------------------------------
        if (wth == 0)
        {
            Social.ReportProgress("CgkIzs-alcMYEAIQDQ", 100.0f, (bool success) =>
            {
                if (success)
                {
                    PlayerPrefs.SetInt("WTH"+m_sUserKey, 1);
                }
            });
        }
    }
    public void ToogleMute()
    {
        audio.volume = muted ? 100 : 0;
        muteButton.image.sprite = muted ? mute1 : mute2;
        muted = !muted;


    }
    //--------------------------------------------------------------------------------
}
