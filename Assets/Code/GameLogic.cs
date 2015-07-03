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
using GoogleMobileAds.Api;
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
    public Animator m_TPAnimator;
    public Button tapToStart;
    //-------------------------------------------------------------------------------
    private bool m_bShowAds = false;
    private int m_iAdCounter = 0;
    private int AD_COUNT_PER_DEATH = 4;
    private bool m_bAdCounter = false;
    //-------------------------------------------------------------------------------
    public int m_iScore;
    private InterstitialAd _ins;
    //-------------------------------------------------------------------------------
    public void Start()
    {
        m_eState = GameState.NotStarted;
        Application.targetFrameRate = 30;

        m_WaveHandler = GetComponent<WaveHandler>();
        m_FormationHandler = GetComponent<FormationHandler>();

        m_InputHandler = GetComponent<InputHandler>();

        m_SPAnimator = ScorePanel.GetComponent<Animator>();
        m_iAdCounter = PlayerPrefs.GetInt("AdCounter", 0);

        RequestAd();
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
            }
            m_TPAnimator.Play("TopPanelEnterAnim");
            GameLogic.State = GameLogic.GameState.NotStarted;
            m_WaveHandler.Restart();
            m_InputHandler.Restart();
            m_FormationHandler.Restart();

            m_iScore = 0;
            MC.UpdateScoreboard(m_iScore);
        }

    }
    //-------------------------------------------------------------------------------
    public void Update()
    {
        if (State == GameState.OnGoing)
            m_bAdCounter = false;

        if (State == GameLogic.GameState.Ended && !m_bAdCounter)
        {
            m_iAdCounter++;
            m_bAdCounter = true;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);

            Social.ReportScore(PlayerPrefs.GetInt("BestScore", 0), "CgkIzs-alcMYEAIQAQ", (bool success) =>
            {
                // handle success or failure
            });
        }
        if (m_iAdCounter % AD_COUNT_PER_DEATH == 0 && m_iAdCounter != 0)
        {
            m_bShowAds = true;
            m_iAdCounter = 0;
            PlayerPrefs.SetInt("AdCounter", m_iAdCounter);
        }
        if (m_bShowAds)
        {
            if (_ins.IsLoaded())
            {
                _ins.Show();
                m_bShowAds = false;


            }
        }
    }
    //-------------------------------------------------------------------------------
    public void RequestAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-7907761596585940/5317772971";
#elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
            string adUnitId = "unexpected_platform";
#endif

        _ins = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddTestDevice("09EBEFB119E7CF791F951ABACAE47671")
            .AddKeyword("game")
            .Build();



        _ins.AdLoaded += HandleInterstitialLoaded;
        _ins.AdFailedToLoad += HandleInterstitialFailedToLoad;
        _ins.AdOpened += HandleInterstitialOpened;
        _ins.AdClosing += HandleInterstitialClosing;
        _ins.AdClosed += HandleInterstitialClosed;
        _ins.AdLeftApplication += HandleInterstitialLeftApplication;
        // Load the interstitial with the request.
        _ins.LoadAd(request);
    }
    //-------------------------------------------------------------------------------
    public void IncrementScore(int s)
    {
        m_iScore += s;
        MC.UpdateScoreboard(m_iScore);
    }
    //-------------------------------------------------------------------------------
    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");

    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");

    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
        _ins.Destroy();
        RequestAd();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
        _ins.Destroy();
    }

    #endregion
}
