//.     .       .  .   . .   .   . .    +  .
//  .     .  :     .    .. :. .___---------___.
//       .  .   .    .  :.:. _".^ .^ ^.  '.. :"-_. .
//    .  :       .  .  .:../:            . .^  :.:\.
//        .   . :: +. :.:/: .   .    .        . . .:\
// .  :    .     . _ :::/:               .  ^ .  . .:\
//  .. . .   . - : :.:./.                        .  .:\
//  .      .     . :..|:                    .  .  ^. .:|
//    .       . : : ..||        .                . . !:|
//  .     . . . ::. ::\(                           . :)/
// .   .     : . : .:.|. ######              .#######::|
//  :.. .  :-  : .:  ::|.#######           ..########:|
// .  .  .  ..  .  .. :\ ########          :######## :/
//  .        .+ :: : -.:\ ########       . ########.:/
//    .  .+   . . . . :.:\. #######       #######..:/
//     :: . . . . ::.:..:.\           .   .   ..:/
//  .   .   .  .. :  -::::.\.       | |     . .:/
//     .  :  .  .  .-:.":.::.\             ..:/
// .      -.   . . . .: .:::.:.\.           .:/
//.   .   .  :      : ....::_:..:\   ___.  :/
//   .   .  .   .:. .. .  .: :.:.:\       :/
//     +   .   .   : . ::. :.:. .:.|\  .:/|
//     .         +   .  .  ...:: ..|  --.:|
//.      . . .   .  .  . ... :..:.."(  ..)"
// .   .       .      :  .   .: ::/  .  .::\


//      __       ___  ___  ___  ___      ___       ___      ___       __        ______    
//     /""\     |"  \/"  ||"  \/"  |    |"  |     |"  \    /"  |     /""\      /    " \   
//    /    \     \   \  /  \   \  /     ||  |      \   \  //   |    /    \    // ____  \  
//   /' /\  \     \\  \/    \\  \/      |:  |      /\\  \/.    |   /' /\  \  /  /    ) :) 
//  //  __'  \    /   /     /   /        \  |___  |: \.        |  //  __'  \(: (____/ //  
// /   /  \\  \  /   /     /   /        ( \_|:  \ |.  \    /:  | /   /  \\  \\        /   
//(___/    \___)|___/     |___/          \_______)|___|\__/|___|(___/    \___)\"_____/    
//--------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//--------------------------------------------------------------------------------
public class WaveHandler : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    private List<WaveBase> m_Waves = new List<WaveBase>();
    private float tWaveSpawn = 0.0f;
    private float tTotalTime = 0.0f;
    //--------------------------------------------------------------------------------
    private float SPAWN_FREQUENCY_COEFFICIENT = 0.5f;
    private float SPAWN_FREQUENCY = 3f;
    private bool bCheckCollision = false;
    private List<GameObject> m_PreloadedAssets;
    private List<GameObject> m_AssociatedGameObjects;
    //--------------------------------------------------------------------------------
    public FormationHandler gL;
    //--------------------------------------------------------------------------------
    private int m_iWaveCount = 0;
    //--------------------------------------------------------------------------------
    private List<WaveBase> m_BakedWaves;
    private bool bWait = false;
    //--------------------------------------------------------------------------------
    private Dictionary<int, WaveBase.WaveType> m_SpawnTable;
    //-------------------------------------------------------------------------------
    private Vector3 m_vWaveOffset = Vector3.zero;
    //-------------------------------------------------------------------------------
    private WaveBase.WaveType spawningWaveType;
    //-------------------------------------------------------------------------------
    private WaveBase.WaveType prevWaveType;
    //--------------------------------------------------------------------------------
    private int m_iBackToBackWaveCount = 0;
    private int m_iCumulativeSleepDur = 0;
    //--------------------------------------------------------------------------------
    private Vector3 m_vSpawnSpeed;
    //--------------------------------------------------------------------------------
    public FloorScript m_FS;
    //--------------------------------------------------------------------------------
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
    //--------------------------------------------------------------------------------
    private GameStages m_CurrentGameStage;
    //--------------------------------------------------------------------------------
    void Start()
    {
        m_PreloadedAssets = new List<GameObject>();
        m_BakedWaves = new List<WaveBase>();

        GameObject[] gos = Resources.LoadAll<GameObject>("Prefabs");
        m_PreloadedAssets.AddRange(gos);

        m_BakedWaves.Add(GetWave(WaveBase.WaveType.Normal));
        m_BakedWaves.Add(GetWave(WaveBase.WaveType.OaA));
        m_BakedWaves.Add(GetWave(WaveBase.WaveType.Oa2A));

        m_CurrentGameStage = GameStages.Harder;
        m_SpawnTable = new Dictionary<int, WaveBase.WaveType>();
        spawningWaveType = WaveBase.WaveType.Normal;

        m_AssociatedGameObjects = new List<GameObject>();

        m_vSpawnSpeed = new Vector3(-5.5f, 0, 0);
    }
    //--------------------------------------------------------------------------------
    public void IncrementSpeed(float laneSpeed, float floorSpeed)
    {
        m_vSpawnSpeed = new Vector3(laneSpeed, 0, 0);
        for (int i = 0; i < m_Waves.Count; i++)
        {
            m_Waves[i].Speed = m_vSpawnSpeed;
            m_Waves[i].UpdateSpeed();
        }
        m_FS.scrollSpeed = floorSpeed;
    }
    //--------------------------------------------------------------------------------
    public void SetWaves()
    {
        gL.Waves = m_Waves;
    }
    //--------------------------------------------------------------------------------
    public void WaveLogic()
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

                    spawningWaveType = RandomWave();
                    WaveBase wb = GetWave(spawningWaveType);

                    if (spawningWaveType == WaveBase.WaveType.Oa2A)
                    {
                        tWaveSpawn = -2;
                    }
                    if (spawningWaveType == WaveBase.WaveType.OaA)
                    {
                        tWaveSpawn = -0.8f;
                    }
                    if (spawningWaveType == WaveBase.WaveType.Normal)
                    {
                        tWaveSpawn = 0;
                    }
                    SpawnWave(wb);
                    prevWaveType = spawningWaveType;


                    

                }
            }


        }
    }
    //--------------------------------------------------------------------------------
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
            IncrementSpeed(-7f, 0.78f);
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
            IncrementSpeed(-9f, 1f);
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
             IncrementSpeed(-11f, 1.23f);
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
                m_SpawnTable.Add(80, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(20, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Easy:
                m_SpawnTable.Add(60, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(40, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ICanDoThis:
                m_SpawnTable.Add(49, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(51, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(0, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Normal:
                m_SpawnTable.Add(45, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(50, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(5, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ImSweating:
                m_SpawnTable.Add(40, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(50, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(10, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Harder:
                m_SpawnTable.Add(40, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(45, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(15, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Hard:
                m_SpawnTable.Add(35, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(45, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(20, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.ThisIsImpossible:
                m_SpawnTable.Add(30, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(50, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(20, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.Hardest:
                m_SpawnTable.Add(20, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(50, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(30, WaveBase.WaveType.Oa2A);
                break;
            case GameStages.GiveUpAlready:
                m_SpawnTable.Add(10, WaveBase.WaveType.Normal);
                m_SpawnTable.Add(50, WaveBase.WaveType.OaA);
                m_SpawnTable.Add(40, WaveBase.WaveType.Oa2A);
                break;
            default:
                break;
        }
    }
    //--------------------------------------------------------------------------------
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

        WaveBase.WaveType type = Indices[Rnd];

        return type;
    }
    //--------------------------------------------------------------------------------
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

        return wb;
    }
    //--------------------------------------------------------------------------------
    public void SpawnWave(WaveBase wave)
    {
        GameObject parent = new GameObject(wave.Name + "#" + m_iWaveCount.ToString());
        Vector3 LanePos = wave.SpawnPosition;
        wave.WaveID = m_iWaveCount;
        List<Lane> lanes = wave.Lanes;
        wave.Speed = m_vSpawnSpeed;
        wave.Initialize();
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
                g.name = "Block at #" + lane.Blocks[j].Index.ToString();
                lane.Blocks[j].Transform = g.transform;
                g.transform.parent = laneObj.transform;
                Physics.IgnoreLayerCollision(g.layer, g.layer);

            }
            LaneSpawnPos.x += 8;
        }
        m_Waves.Add(wave);
        m_iWaveCount++;

        m_AssociatedGameObjects.Add(parent);
    }
    //--------------------------------------------------------------------------------
    public void Update()
    {
        bCheckCollision = false;

        if (GameLogic.State == GameLogic.GameState.OnGoing)
        {
            WaveLogic();
            for (int i = 0; i < m_Waves.Count; i++)
            {
                m_Waves[i].Update();
            }
            SetWaves();
        }
    }
    //--------------------------------------------------------------------------------
    public void SpawnFirstWave()
    {
        NormalWave wave = new NormalWave();

        spawningWaveType = WaveBase.WaveType.Normal;
        prevWaveType = spawningWaveType;
        SpawnWave(wave);

        SetWaves();
    }
    //--------------------------------------------------------------------------------
    public void DestroyWave(int ID)
    {
        if (!bCheckCollision)
        {
            WaveBase wb = m_Waves.Find(x => x.WaveID == ID);
            m_Waves.Remove(wb);


            bCheckCollision = true;
        }
    }
    //--------------------------------------------------------------------------------
    public void Restart()
    {
        m_Waves.Clear();

        for (int i = 0; i < m_AssociatedGameObjects.Count; i++)
        {
            Destroy(m_AssociatedGameObjects[i]);
        }
        m_AssociatedGameObjects.Clear();

        tTotalTime = 0;
        tWaveSpawn = 0;
        m_CurrentGameStage = GameStages.Easiest;
        m_iWaveCount = 0;
        m_vWaveOffset = Vector3.zero;
        bWait = false;
        spawningWaveType = WaveBase.WaveType.Normal;
        m_vSpawnSpeed = new Vector3(-5.5f, 0, 0);
        m_FS.scrollSpeed = 0.609f;
    }
}
