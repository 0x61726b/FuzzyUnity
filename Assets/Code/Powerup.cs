using UnityEngine;
using System.Collections;

public class Powerup
{
    private string m_sPowerupName;

    public string Name
    {
        get { return m_sPowerupName; }
        set { m_sPowerupName = value; }
    }

    private PowerupEffectBase m_Effect;

    public PowerupEffectBase Effect
    {
        get { return m_Effect; }
        set { m_Effect = value; }
    }

    private bool m_bRandom;

    public bool IsRandom
    {
        get { return m_bRandom; }
        set { m_bRandom = value; }
    }

    private int m_iDropTime;

    public int DropTime
    {
        get { return m_iDropTime; }
        set { m_iDropTime = value; }
    }

    private GameObject m_GameObj;

    public GameObject GameObject
    {
        get { return m_GameObj; }
        set { m_GameObj = value; }
    }


    public Powerup()
    {
        
    }
    public void Update()
    {
        //Debug.Log("Powerup active!!");
    }

}
