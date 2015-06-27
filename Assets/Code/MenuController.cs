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
    public void Start()
    {


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

    }
    //--------------------------------------------------------------------------------
    public void RestartLevel()
    {
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
