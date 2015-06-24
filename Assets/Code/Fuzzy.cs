using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Fuzzy : MonoBehaviour
{

    public int CurrentLane;
    private int m_iOtherFuzzyLane;
    private Vector2 m_vSpring;

 
    // Use this for initialization
    void Start()
    {
        m_vSpring = new Vector2(transform.position.z, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.State == GameLogic.GameState.OnGoing)
            LaneSwitch();
    }
    public void LaneSwitch()
    {
        Vector3 NewLanePos = new Vector3(-9, 2, (CurrentLane - 1) * (-1.5f));

        m_vSpring = Spring(m_vSpring.x, m_vSpring.y, NewLanePos.z, 0.6f, 9, Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, m_vSpring.x);
    }
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
    public void OnCollisionEnter(Collision c)
    {
        GameLogic.State = GameLogic.GameState.Ended;

        Animator animator = GameObject.FindWithTag("ScorePanel").GetComponent<Animator>();
        
        animator.SetBool("gameEnded", true);

        GameObject.Find("MenuController").GetComponent<MenuController>().UpdateScoreboard();
    }
}
