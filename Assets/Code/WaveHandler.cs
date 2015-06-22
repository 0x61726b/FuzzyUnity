using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveHandler : MonoBehaviour
{

    private List<WaveBase> m_Waves = new List<WaveBase>();
    private float tWaveSpawn = 0.0f;

    private float SPAWN_FREQUENCY_COEFFICIENT = 0.5f;
    private float SPAWN_FREQUENCY = 2.0f;
    void Start()
    {


        
    }
    void WaveLogic()
    {
        tWaveSpawn += Time.deltaTime;
        if (tWaveSpawn > SPAWN_FREQUENCY)
        {
            SPAWN_FREQUENCY_COEFFICIENT = Random.Range(0.2F, 0.9f);

            if (tWaveSpawn >= SPAWN_FREQUENCY + SPAWN_FREQUENCY_COEFFICIENT)
            {
                int LaneCntDecider = Random.Range(2, 4);
                NormalWave wave = new NormalWave(LaneCntDecider);
                wave.Initialize();
                SpawnWave(wave);
                tWaveSpawn = 0.0f;

                
            }
        }

        
    }
    void SpawnWave(WaveBase wave)
    {
        GameObject parent = new GameObject(wave.Name + "# " + m_Waves.Count.ToString());
        Vector3 LanePos = wave.SpawnPosition;

        List<Lane> lanes = wave.Lanes;

        Vector3 LaneSpawnPos = wave.SpawnPosition;
        for (int i = 0; i < lanes.Count; i++)
        {
            //Get Lane
            Lane lane = lanes[i];
            List<int> binary = lane.Binary;

            GameObject laneObj = new GameObject("Lane " + i.ToString());
            laneObj.transform.parent = parent.transform;

            for (int j = 0; j < lane.Blocks.Count; j++)
            {
                GameObject prefab = lane.Blocks[j].Prefab;
                Vector3 BasePosition = LaneSpawnPos;
                BasePosition.z = lane.Blocks[j].Index * (-1.5f);

                GameObject g = (GameObject)Instantiate(prefab, BasePosition, Quaternion.identity);
                g.name = "Block at #" + lane.Blocks[j].Index.ToString();
                lane.Blocks[j].Transform = g.transform;
                g.transform.parent = laneObj.transform;

            }
            LaneSpawnPos.x += 10;
        }
        m_Waves.Add(wave);
    }
    void Update()
    {
        WaveLogic();
        for (int i = 0; i < m_Waves.Count; i++)
        {
            m_Waves[i].Update();
        }
    }
}
