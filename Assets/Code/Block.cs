using UnityEngine;
using System.Collections;

public class Block : LaneElement
{
    private string m_sBlockName;

    private Vector3 m_vXSpeed;

    public Vector3 XSpeed
    {
        get { return m_vXSpeed; }
        set { m_vXSpeed = value; }
    }



    public string Name
    {
        get { return m_sBlockName; }
        set { m_sBlockName = value; }
    }

    public Block()
        : base(ElementType.Block)
    {
        
    }
    public override void Update()
    {
        Transform.Translate(m_vXSpeed*Time.deltaTime);

    }
}
