using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationHandler : MonoBehaviour
{
    private bool bCheckCollision = false;
    private List<WaveBase> m_Waves;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bCheckCollision = false;
    }
    public void OnWaveCollision(Collision c)
    {
        if (!bCheckCollision)
        {
            FindWave(c.collider.transform.parent.parent.gameObject);
        }
    }
    public void FindWave(GameObject g)
    {
        string name = g.name;
        int waveID = System.Int32.Parse(name.Substring(name.Length - 1));
        Debug.Log(waveID);

        bCheckCollision = true;
    }
}
