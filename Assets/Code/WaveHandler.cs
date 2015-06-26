using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveHandler : MonoBehaviour
{

    private List<WaveBase> m_Waves = new List<WaveBase>();
    private float tWaveSpawn = 0.0f;

    private float SPAWN_FREQUENCY_COEFFICIENT = 0.5f;
    private float SPAWN_FREQUENCY = 2.5f;
    private bool bCheckCollision = false;

    private List<GameObject> m_PreloadedAssets;

    public FormationHandler gL;
    void Start()
    {
        m_PreloadedAssets = new List<GameObject>();
        GameObject[] gos = Resources.LoadAll<GameObject>("Prefabs");
        m_PreloadedAssets.AddRange(gos);
    }
    public void SetWaves()
    {
        gL.Waves = m_Waves;
    }
    void WaveLogic()
    {
        if (GameLogic.State == GameLogic.GameState.OnGoing)
        {
            tWaveSpawn += Time.deltaTime;
            if (tWaveSpawn > SPAWN_FREQUENCY)
            {
                SPAWN_FREQUENCY_COEFFICIENT = Random.Range(0.2F, 0.9f);

                if (tWaveSpawn >= SPAWN_FREQUENCY + SPAWN_FREQUENCY_COEFFICIENT)
                {
                    int LaneCntDecider = Random.Range(2, 4);
                    int waveDecider = Random.Range(1, 3);
                    WaveBase wb = null;
                    if (waveDecider == 1)
                    {
                        wb = new NormalWave(LaneCntDecider);
                    }
                    if (waveDecider == 2)
                    {
                        wb = new DoubleWave(LaneCntDecider);
                    }
                    //wb = new NormalWave(LaneCntDecider);

                    wb.Initialize();
                    SpawnWave(wb);
                    tWaveSpawn = 0.0f;
                }
            }
        }
    }
    void SpawnWave(WaveBase wave)
    {
        GameObject parent = new GameObject(wave.Name + "#" + m_Waves.Count.ToString());
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
                GameObject prefab = m_PreloadedAssets.Find(x => x.name == lane.Blocks[j].Prefab);
                Vector3 BasePosition = LaneSpawnPos;
                BasePosition.z = lane.Blocks[j].Index * (-1.5f);

                GameObject g = (GameObject)Instantiate(prefab, BasePosition, Quaternion.identity);
                //g.GetComponent<Renderer>().material.color = wave.MaterialColor;
                g.name = "Block at #" + lane.Blocks[j].Index.ToString();
                lane.Blocks[j].Transform = g.transform;
                g.transform.parent = laneObj.transform;

            }
            LaneSpawnPos.x += 6;
        }
        m_Waves.Add(wave);
    }
    void Update()
    {
        bCheckCollision = false;

        if (GameLogic.State == GameLogic.GameState.OnGoing)
        {
            WaveLogic();
            for (int i = 0; i < m_Waves.Count; i++)
            {
                if (m_Waves[i] != null)
                    m_Waves[i].Update();
            }
            SetWaves();   
        }
        
    }
    public void SpawnFirstWave()
    {
        int LaneCntDecider = Random.Range(2, 4);
        NormalWave wave = new NormalWave(LaneCntDecider);
        wave.Initialize();
        SpawnWave(wave);

        SetWaves();
    }
    public void DestroyWave(int ID)
    {
        if (!bCheckCollision)
        {
            m_Waves[ID] = null;

            bCheckCollision = true;
        }
    }
}
