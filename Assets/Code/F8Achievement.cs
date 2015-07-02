using UnityEngine;
using System.Collections;

public class F8Achievement
{
    private string m_sName;
    //--------------------------------------------------------------------------------
    public string Name
    {
        get { return m_sName; }
        set { m_sName = value; }
    }
    //--------------------------------------------------------------------------------
    private string m_sCode;
    //--------------------------------------------------------------------------------
    public string Code
    {
        get { return m_sCode; }
        set { m_sCode = value; }
    }
    //--------------------------------------------------------------------------------
    private bool m_bAchieved;
    //--------------------------------------------------------------------------------
    public bool Achieved
    {
        get { return m_bAchieved; }
        set { m_bAchieved = value; }
    }
    //--------------------------------------------------------------------------------
    private int m_iProgress;
    //--------------------------------------------------------------------------------
    public int Progress
    {
        get { return m_iProgress; }
        set { m_iProgress = value; }
    }
    //--------------------------------------------------------------------------------
    public F8Achievement()
    {
        m_sName = "";
        m_sCode = "";
        m_bAchieved = false;
        m_iProgress = 100;
    }
    //--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------
