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
//--------------------------------------------------------------------------------
public class Fuzzy : MonoBehaviour,ISleepable
{
    //--------------------------------------------------------------------------------
    public int CurrentLane;
    //--------------------------------------------------------------------------------
    private Vector2 m_vSpring;
    //--------------------------------------------------------------------------------
    private bool m_bSleeping;
    //--------------------------------------------------------------------------------
    private float m_fSleepTimer = 0.0f;
    //--------------------------------------------------------------------------------
    private float m_fSleepThreshold = 0.4f;
    //--------------------------------------------------------------------------------
    public MenuController menuController;
    //--------------------------------------------------------------------------------
    public GameObject ScorePanel;
    private Animator m_SPAnimator;
    //--------------------------------------------------------------------------------
    public bool Sleeping
    {
        get { return m_bSleeping; }
        set { m_bSleeping = value; }
    }
    //--------------------------------------------------------------------------------
    public void Start()
    {
        m_vSpring = new Vector2(transform.position.z, 0);
        m_bSleeping = true;

        m_SPAnimator = ScorePanel.GetComponent<Animator>();
    }
    //--------------------------------------------------------------------------------
    public void Update()
    {
        if (!m_bSleeping)
        {
            if (GameLogic.State == GameLogic.GameState.OnGoing)
                LaneSwitch();
        }
        if (!m_bSleeping)
        {
            m_fSleepTimer += Time.deltaTime;

            if (m_fSleepTimer >= m_fSleepThreshold)
            {
                m_bSleeping = true;
                m_fSleepTimer = 0.0f;
            }
        }
    }
    //--------------------------------------------------------------------------------
    public void LaneSwitch()
    {
        Vector3 NewLanePos = new Vector3(-9, 2, (CurrentLane - 1) * (-1.5f));

        m_vSpring = Spring(m_vSpring.x, m_vSpring.y, NewLanePos.z, 0.6f, 9, Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, m_vSpring.x);
    }
    //--------------------------------------------------------------------------------
    public Vector2 Spring(float x, float v, float Xt, float Zeta, float Omega, float h)
    {
        float f = 1.0f + 2.0f * h * Zeta * Omega;
        float oo = Omega * Omega;
        float hoo = h * oo;
        float hhoo = h * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + h * v + hhoo * Xt;
        float detV = v + hoo * (Xt - x);
        x = detX * detInv;
        v = detV * detInv;
        return new Vector2(x, v);
    }
    //--------------------------------------------------------------------------------
    public void OnCollisionEnter(Collision c)
    {
        GameLogic.State = GameLogic.GameState.Ended;

        m_SPAnimator.Play("ScorePanelAnim");
        //m_SPAnimator.SetBool("gameEnded", true);

    }
    //--------------------------------------------------------------------------------
    public bool IsSleeping()
    {
        return m_bSleeping;
    }
    //--------------------------------------------------------------------------------
    public void SetSleepTimer()
    {
        throw new System.NotImplementedException();
    }
    //--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------