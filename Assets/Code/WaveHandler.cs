using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveHandler : MonoBehaviour
{

    private List<WaveBase> m_Waves = new List<WaveBase>();
    private float tWaveSpawn = 0.0f;
    private float tTotalTime = 0.0f;

    private float SPAWN_FREQUENCY_COEFFICIENT = 0.5f;
    private float SPAWN_FREQUENCY = 2.5f;
    private bool bCheckCollision = false;

    private int m_iTotalWeight = 0;
    private int m_iInitialTW = 0;

    private List<GameObject> m_PreloadedAssets;

    public FormationHandler gL;

    private List<WaveBase> m_BakedWaves;
    private bool bWait = false;
    private Dictionary<int, WaveBase.WaveType> m_SpawnTable;
    private Vector3 m_vWaveOffset = Vector3.zero;
    private WaveBase.WaveType spawningWaveType;
    public enum GameStages
    {
        Easiest,
        Easier,
        Easy,
        ICanDoThis,
        Normal,
        ImSweating,
        Harder,
        Hard,
        ThisIsImpossible,
        Hardest,
        GiveUpAlready
    }
    private GameStages m_CurrentGameStage;
    void Start()
    {
        m_PreloadedAssets = new List<GameObject>();
        m_BakedWaves = new List<WaveBase>();

        GameObject[] gos = Resources.LoadAll<GameObject>("Prefabs");
        m_PreloadedAssets.AddRange(gos);

        m_BakedWaves.Add(GetWave(WaveBase.WaveType.Normal));
        m_BakedWaves.Add(GetWave(WaveBase.WaveType.OaA));
        m_BakedWaves.Add(GetWave(WaveBase.WaveType.Oa2A));

        for (int i = 0; i < m_BakedWaves.Count; i++)
        {
            WaveBase wb = m_BakedWaves[i];
            int SpawnRate = wb.SpawnRate;
            m_iInitialTW += SpawnRate;
        }
        m_CurrentGameStage = GameStages.Easiest;
        m_SpawnTable = new Dictionary<int, WaveBase.WaveType>();
        spawningWaveType = WaveBase.WaveType.Normal;
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
            tTotalTime += Time.deltaTime;


            if (tWaveSpawn > SPAWN_FREQUENCY)
            {
                SPAWN_FREQUENCY_COEFFICIENT = Random.Range(0.2F, 0.9f);


                if (tWaveSpawn >= SPAWN_FREQUENCY + SPAWN_FREQUENCY_COEFFICIENT)
                {
                    CalculateGameStages();
                    if (spawningWaveType == WaveBase.WaveType.Oa2A)
                    {
                        bWait = true;
                    }
                   
                    if (!bWait)
                    {

                        spawningWaveType = RandomWave();
                        SpawnWave(GetWave(spawningWaveType));
                        tWaveSpawn = 0.0f;
                        Debug.Log(m_CurrentGameStage.ToString());
                    }

                    if (bWait)
                    {
                        if (tWaveSpawn >= SPAWN_FREQUENCY + SPAWN_FREQUENCY_COEFFICIENT *8)
                        {
                            SpawnWave(GetWave(spawningWaveType));
                            tWaveSpawn = 0.0f;
                            bWait = false;
                        }
                    }

                }
            }
        }
    }

    private void CalculateGameStages()
    {
        int Split = 8;
        if (tTotalTime <= Split)
        {
            m_CurrentGameStage = GameStages.Easiest;
        }
        if (tTotalTime <= 2 * Split && tTotalTime > Split)
        {
            m_CurrentGameStage = GameStages.Easier;
        }
        if (tTotalTime <= 3 * Split && tTotalTime > 2 * Split)
        {
            m_CurrentGameStage = GameStages.Easy;
        }
        if (tTotalTime <= 4 * Split && tTotalTime > 3 * Split)
        {
            m_CurrentGameStage = GameStages.ICanDoThis;
        }
        if (tTotalTime <= 5 * Split && tTotalTime > 4 * Split)
        {
            m_CurrentGameStage = GameStages.Normal;
        }
        if (tTotalTime <= 6 * Split && tTotalTime > 5 * Split)
        {
            m_CurrentGameStage = GameStages.ImSweating;
        }
        if (tTotalTime <= 7 * Split && tTotalTime > 6 * Split)
        {
            m_CurrentGameStage = GameStages.Harder;
        }
        if (tTotalTime <= 8 * Split && tTotalTime > 7 * Split)
        {
            m_CurrentGameStage = GameStages.Hard;
        }
        if (tTotalTime <= 9 * Split && tTotalTime > 8 * Split)
        {
            m_CurrentGameStage = GameStages.ThisIsImpossible;
        }
        if (tTotalTime <= 10 * Split && tTotalTime > 9 * Split)
        {
            m_CurrentGameStage = GameStages.Hardest;
        }
        if (tTotalTime > Split * 10)
        {
            m_CurrentGameStage = GameStages.GiveUpAlready;
        }
        m_SpawnTable.Clear();
        switch (m_CurrentGameStage)
        {
            case GameStages.Easiest:
                m_SpawnTable.Add(99, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(1, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Easier:
                m_SpawnTable.Add(60, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(40, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Easy:
                m_SpawnTable.Add(51, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(49, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ICanDoThis:
                m_SpawnTable.Add(50, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(30, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(20, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Normal:
                m_SpawnTable.Add(50, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(26, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(24, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ImSweating:
                m_SpawnTable.Add(70, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(20, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(10, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Harder:
                m_SpawnTable.Add(60, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(21, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(19, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Hard:
                m_SpawnTable.Add(35, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(45, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(30, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ThisIsImpossible:
                m_SpawnTable.Add(10, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(44, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(46, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Hardest:
                m_SpawnTable.Add(0, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(49, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(51, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.GiveUpAlready:
                m_SpawnTable.Add(0, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(25, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(75, WaveBase.WaveType.Oa2A);
                break;
            default:
                int x = 0;
                break;
        }
    }
    private WaveBase.WaveType RandomWave()
    {
        List<WaveBase.WaveType> Indices = new List<WaveBase.WaveType>();
        Indices.Capacity = 100;

        int Rnd = Random.Range(0, 99);


        foreach (int i in m_SpawnTable.Keys)
        {
            for (int j = 0; j < i; j++)
            {
                Indices.Add(m_SpawnTable[i]);
            }
        }
        return Indices[Rnd];
    }
    WaveBase GetWave(WaveBase.WaveType type)
    {
        WaveBase wb = null;
        switch (type)
        {
            case WaveBase.WaveType.Normal:
                wb = new NormalWave();
                break;
            case WaveBase.WaveType.OaA:
                wb = new DoubleWave();
                break;
            case WaveBase.WaveType.Oa2A:
                wb = new TripleWave();
                break;
        }
        wb.Initialize();
        return wb;
    }
    void SpawnWave(WaveBase wave)
    {
        GameObject parent = new GameObject(wave.Name + "#" + m_Waves.Count.ToString());
        Vector3 LanePos = wave.SpawnPosition;

        List<Lane> lanes = wave.Lanes;


        Vector3 LaneSpawnPos = wave.SpawnPosition + m_vWaveOffset;
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
        NormalWave wave = new NormalWave();
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
