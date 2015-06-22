using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    private List<WaveBase> m_Waves = new List<WaveBase>();
    // Use this for initialization
    void Start()
    {
        int LaneCntDecider = Random.Range(2, 4);

        NormalWave wave = new NormalWave(LaneCntDecider);
        wave.SpawnPosition = new Vector3(0, 0, 0);
        GameObject parent = new GameObject(wave.Name);
        Vector3 LanePos = wave.SpawnPosition;
        for (int i = 0; i < wave.Lanes.Count; i++)
        {
            Lane l = wave.Lanes[i];

            for (int j = 0; j < l.Binary.Count; j++)
            {
                if (l.Binary[j] == 1)
                {
                    Vector3 BasePosition = LanePos;
                    BasePosition.z = (j) * (-1.5f);

                    LaneElement lE = l.GetLaneElement(j);

                    GameObject g = (GameObject)Instantiate(lE.Prefab, BasePosition, Quaternion.identity);
                    g.transform.parent = parent.transform;
                    g.name = "Block at #" + j.ToString();
                }
            }
            LanePos = new Vector3(LanePos.x + 10, LanePos.y, LanePos.y);
        }
        m_Waves.Add(wave);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
