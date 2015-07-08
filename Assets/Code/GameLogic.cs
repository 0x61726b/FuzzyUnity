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
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;
//-------------------------------------------------------------------------------
public class GameLogic : MonoBehaviour
{
    //-------------------------------------------------------------------------------
    public enum GameState
    {
        NotStarted,
        OnGoing,
        Paused,
        Ended
    };
    //-------------------------------------------------------------------------------
    private static GameState m_eState;
    public static GameState State
    {
        get { return m_eState; }
        set { m_eState = value; }
    }
    //-------------------------------------------------------------------------------
    private WaveHandler m_WaveHandler;
    private FormationHandler m_FormationHandler;
    private InputHandler m_InputHandler;
    public MenuController MC;
    //-------------------------------------------------------------------------------
    public GameObject ScorePanel;
    private Animator m_SPAnimator;
    public GameObject ButtonPanel;
    public GameObject ScoreText;
    public Animator m_TPAnimator;
    public Button tapToStart;
    //-------------------------------------------------------------------------------
    private bool m_bShowAds = false;
    private int m_iAdCounter = 0;
    private int AD_COUNT_PER_DEATH = 3;
    private bool m_bAdCounter = false;
    public UpsightPluginTest _upsight;
    AdmobAdapter admobAdapter = new AdmobAdapter();
    //-------------------------------------------------------------------------------
    public int m_iScore = 0;
    public int m_iBracket = 0;
    private float tGameDuration = 0;
    //-------------------------------------------------------------------------------
    void Awake()
    {
        admobAdapter.Awake();
    }
    void Destroy()
    {
        admobAdapter.Destroy(); 
    }
    public void Start()
    {
        admobAdapter.Start();
        m_eState = GameState.NotStarted;
        Application.targetFrameRate = 30;

        m_WaveHandler = GetComponent<WaveHandler>();
        m_FormationHandler = GetComponent<FormationHandler>();

        m_InputHandler = GetComponent<InputHandler>();

        m_SPAnimator = ScorePanel.GetComponent<Animator>();
        m_iAdCounter = PlayerPrefs.GetInt("AdCounter", 0);
    }
    //-------------------------------------------------------------------------------
    public void Restart()
    {
        if (!tapToStart.isActiveAndEnabled)
        {
            if (!(GameLogic.State == GameState.OnGoing))
            {

                //ScorePanel.SetActive(false);
                //m_SPAnimator.SetBool("gameEnded", false);
                m_SPAnimator.Play("ScorePanelOutAnim");

                ButtonPanel.SetActive(false);
                ScoreText.SetActive(false);
            }
            ButtonPanel.SetActive(false);
            ScoreText.SetActive(false);
            m_TPAnimator.Play("TopPanelEnterAnim");
            GameLogic.State = GameLogic.GameState.NotStarted;
            m_WaveHandler.Restart();
            m_InputHandler.Restart();
            m_FormationHandler.Restart();

            m_iScore = 0;
            m_iBracket = 0;
            MC.UpdateScoreboard(m_iScore);
        }

    }
    //-------------------------------------------------------------------------------
    public void Update()
    {
        if (State == GameState.OnGoing)
        {
            m_bAdCounter = false;
            tGameDuration += Time.deltaTime;
        }

        if (State == GameLogic.GameState.Ended && !m_bAdCounter)
        {
            
            m_iAdCounter++;
            m_bAdCounter = true;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);

            int postingScore = m_iScore;

            if(PlayerPrefs.GetInt("PostingError",0) > postingScore){
                postingScore = PlayerPrefs.GetInt("PostingError", 0);
                if(PlayerPrefs.GetInt("BestScore") < postingScore){
                    PlayerPrefs.SetInt("BestScore", postingScore);
                }
            }

#if UNITY_ANDROID
            Social.ReportScore(postingScore, "CgkIzs-alcMYEAIQAQ", (bool success) =>
               {
                   if (success)
                   {
                       PlayerPrefs.SetInt("PostingError", 0);
                   }
                   else
                   {
                       PlayerPrefs.SetInt("PostingError", postingScore);
                   }
               }); 
#endif
            _upsight.GameOverEvent(m_iBracket, m_iScore, (int)tGameDuration,m_WaveHandler.m_CurrentGameStage.ToString());
            tGameDuration = 0;
        }
        if (m_iAdCounter % AD_COUNT_PER_DEATH == 0 && m_iAdCounter != 0)
        {
            m_bShowAds = true;
            m_iAdCounter = 0;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);
        }
        if (m_bShowAds)
        {
            admobAdapter.ShowInterstitial();
            m_bShowAds = false;  
        }
    }
    //-------------------------------------------------------------------------------
    public void IncrementScore(int s,int b)
    {
        m_iScore += s;
        MC.UpdateScoreboard(m_iScore);
        m_iBracket += b;
    }
    //-------------------------------------------------------------------------------
}
