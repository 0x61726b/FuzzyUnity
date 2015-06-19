using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fuzzy : MonoBehaviour
{

    public int CurrentLane;
    private int m_iOtherFuzzyLane;
    private Vector2 m_vSpring;

    private Logic.GameState m_eState;
    // Use this for initialization
    void Start()
    {
        m_vSpring = new Vector2(transform.position.z, 0);

    }
    public void UpdateGameState(Logic.GameState state)
    {
        m_eState = state;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_eState == Logic.GameState.OnGoing)
            LaneSwitch(); 
    }
    public void FindOtherFuzzy()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Fuzzy");

        if (list.Length > 0)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != this)
                {
                    Fuzzy f = list[i].GetComponent<Fuzzy>();

                    m_iOtherFuzzyLane = f.CurrentLane;
                }
            }
        }
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
        m_eState = Logic.GameState.Over;
        GameObject.Find("Logic").SendMessage("SetGameState", m_eState);
    }
}
