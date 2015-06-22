using UnityEngine;
using System.Collections;

public class Powerup : LaneElement
{
    private string m_sPowerupName;

    public string Name
    {
        get { return m_sPowerupName; }
        set { m_sPowerupName = value; }
    }

    //private PowerupEffectBase m_Effect;

    //public PowerupEffectBase Effect
    //{
    //    get { return m_Effect; }
    //    set { m_Effect = value; }
    //}

    public Powerup() :base(ElementType.Powerup)
    {
        
    }
    public virtual void Update()
    {
        Debug.Log("Powerup active!!");
    }

}
