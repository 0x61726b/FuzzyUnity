using UnityEngine;
using System.Collections;

public class LaneElement 
{
    public enum ElementType
    {
        Block,
        Powerup
    }

    private ElementType m_Type;

    public ElementType Type
    {
        get { return m_Type; }
        set { m_Type = value; }
    }

    private string m_Prefab;

    public string Prefab
    {
        get { return m_Prefab; }
        set { m_Prefab = value; }
    }

    private int m_iIndex;

    public int Index
    {
        get { return m_iIndex; }
        set { m_iIndex = value; }
    }

    private Transform m_Transform;

    public Transform Transform
    {
        get { return m_Transform; }
        set { m_Transform = value; }
    }


    public LaneElement(ElementType type)
    {
        m_Type = type;
    }

    public virtual void Update()
    {
       
    }
}
