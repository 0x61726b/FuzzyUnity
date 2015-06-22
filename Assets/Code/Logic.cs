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
//////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
//////////////////////////////////////////////////////
public class Logic : MonoBehaviour
{
    public struct Wave
    {
        public List<int> LaneBinary;
        public List<GameObject> LaneBlocks { get; set; }
        public int BlockCount;
        public bool Status { get; set; }
        public string Id { get; set; }
        public Wave(List<int> l, int i, bool b)
        {
            LaneBinary = l;
            BlockCount = i;
            Status = b;
            LaneBlocks = new List<GameObject>();
            Id = "Wave Unknown";
        }
    };
    public Button LeftButton;
    public Button CenterButton;
    public Button RightButton;

    public Material DefaultMat;
    //////////////////////////////////////////////////////
    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;
    //////////////////////////////////////////////////////
    public GameObject cBaseEnemy;
    //////////////////////////////////////////////////////
    public GameObject SpawnPoint;
    private Vector3 InitialSpawnPoint;
    private float totalTime;
    //////////////////////////////////////////////////////
    private int CURRENT_BUTTON_COUNT = 3;
    //////////////////////////////////////////////////////
    private List<GameObject> m_vCurrentEnemyList;
    private List<int> m_iCurrentLaneList;
    private Wave m_hitWave;
    private List<Wave> m_sLanes;
    private bool m_bUpdateButtons = true;
    private bool m_bButtonsUpdated = false;
    private bool m_bCollision = false;
    private int m_iUpdateWaveCount = 0;
    public enum GameState
    {
        NotStarted,
        OnGoing,
        Over
    }
    //////////////////////////////////////////////////////
    private GameState m_eState;
    //////////////////////////////////////////////////////
    public void Start()
    {
        m_eState = GameState.NotStarted;

        m_vCurrentEnemyList = new List<GameObject>();
        m_iCurrentLaneList = new List<int>();
        m_sLanes = new List<Wave>();

    }
    public void TapToStartButton()
    {
        m_eState = GameState.OnGoing;
        SpawnEnemyLogic();
        DetermineRightFormation(m_sLanes[0].LaneBinary);
        GameObject.FindGameObjectWithTag("TapToStart").SetActive(false);
    }
    //////////////////////////////////////////////////////
    public void InputListener()
    {
       
    }
    //////////////////////////////////////////////////////
    void OnClick(int left, int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;
    }
    //////////////////////////////////////////////////////
    void Update()
    {

        if (m_eState == GameState.OnGoing)
        {
            totalTime += Time.deltaTime;

            GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    BaseEnemy b = list[i].GetComponent<BaseEnemy>();
                    if (b)
                        b.UpdateGameState(m_eState);
                }
            }
        }

        UpdateGameState();
        InputListener();
        if (m_eState == GameState.OnGoing)
        {
            if (totalTime >= 3)
            {
                SpawnEnemyLogic();
                totalTime = 0;
            }



            if (m_bCollision)
            {
                if (m_bUpdateButtons)
                {
                    DetermineRightFormation(m_sLanes[++m_iUpdateWaveCount].LaneBinary);
                    Debug.Log(m_iUpdateWaveCount.ToString() + " Update" + m_hitWave.Id);

                    m_bUpdateButtons = false;
                    m_bCollision = false;
                }
            }
            if (!m_bCollision)
                m_bUpdateButtons = true;

        }
    }
    //////////////////////////////////////////////////////
    void SpawnEnemyLogic()
    {

        if (m_eState == GameState.OnGoing)
        {
            int LaneCntDecider = Random.Range(2, 4);

            List<int> Lanes = GenerateRandomLanes(LaneCntDecider);
            List<GameObject> thisBlocks = new List<GameObject>();

            for (int i = 0; i < Lanes.Count; i++)
            {
                if (Lanes[i] == 1)
                {
                    Vector3 BasePosition = SpawnPoint.transform.position;
                    BasePosition.z = (i) * (-1.5f);
                    GameObject g = (GameObject)Instantiate(cBaseEnemy, BasePosition, new Quaternion());
                    g.name = "Enemy at " + i.ToString();
                    thisBlocks.Add(g);
                }
            }
            Wave laneS = new Wave(Lanes, Lanes.Count, false);
            laneS.Id = "Wave " + m_sLanes.Count.ToString();
            laneS.LaneBlocks = thisBlocks;
            m_iCurrentLaneList = Lanes;

            m_sLanes.Add(laneS);

        }
    }
    //////////////////////////////////////////////////////
    public void DetermineRightFormation(List<int> laneList)
    {
        List<List<int>> listOfList = new List<List<int>>();


        for (int j = 0; j < 3; j++)
        {
            List<int> possibleFormation = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                possibleFormation.Add(0);
            }
            int c = 0;
            for (int i = 0; i < laneList.Count; i++)
            {

                if (laneList[i] == 1)
                {
                    possibleFormation[i] = 0;
                    continue;
                }
                if (laneList[i] == 0)
                {
                    //POSSIBLE CANDIDATE
                    if (c < 2)
                    {
                        possibleFormation[i] = 1;
                        c++;
                    }
                }
            }
            listOfList.Add(possibleFormation);
        }

        int ChooseCandidate = Random.Range(1, listOfList.Count);

        List<int> ChosenFormation = listOfList[ChooseCandidate];

        System.Text.StringBuilder Formation = new System.Text.StringBuilder();
        bool first = false;
        for (int i = 0; i < ChosenFormation.Count; i++)
        {
            if (ChosenFormation[i] == 1 && first)
            {
                Formation.Append("-");
            }
            if (ChosenFormation[i] == 1)
            {
                first = true;
                Formation.Append(i + 1);
            }
        }

        int ChooseCorrectButton = Random.Range(1, CURRENT_BUTTON_COUNT + 1);

        Texture2D TextureToSet;
        Button CorrectButton = null;

        Texture2D t = Resources.Load(Formation.ToString()) as Texture2D;
        TextureToSet = t;

        List<Button> OtherButtons = new List<Button>();

        if (ChooseCorrectButton == 1)
        {
            CorrectButton = LeftButton;
            OtherButtons.Add(RightButton);
            OtherButtons.Add(CenterButton);
        }
        if (ChooseCorrectButton == 2)
        {
            CorrectButton = CenterButton;
            OtherButtons.Add(RightButton);
            OtherButtons.Add(LeftButton);
        }
        if (ChooseCorrectButton == 3)
        {
            CorrectButton = RightButton;
            OtherButtons.Add(CenterButton);
            OtherButtons.Add(LeftButton);
        }


        Sprite sp = CorrectButton.GetComponent<Image>().sprite;
        CorrectButton.GetComponent<Image>().sprite = Sprite.Create(t, sp.rect, sp.pivot);
        CorrectButton.GetComponent<LaneSprite>().Left = (int)System.Char.GetNumericValue(Formation[0]);
        CorrectButton.GetComponent<LaneSprite>().Right = (int)System.Char.GetNumericValue(Formation[2]);
        CorrectButton.onClick.AddListener(() => OnClick((int)System.Char.GetNumericValue(Formation[0]),
   (int)System.Char.GetNumericValue(Formation[2])));

       


        //Change other buttons iamges
        for (int i = 0; i < OtherButtons.Count; i++)
        {
            //Decide random lane
            List<int> lanes = GenerateRandomLanes(2);
            bool ffirst = false;

            System.Text.StringBuilder NewFormation = new System.Text.StringBuilder();
            for (int j = 0; j < lanes.Count; j++)
            {
                if (lanes[j] == 1 && ffirst)
                {
                    NewFormation.Append("-");
                }
                if (lanes[j] == 1)
                {
                    ffirst = true;
                    NewFormation.Append(j + 1);
                }
            }

            t = Resources.Load(NewFormation.ToString()) as Texture2D;
            TextureToSet = t;


            sp = OtherButtons[i].GetComponent<Image>().sprite;
            OtherButtons[i].GetComponent<Image>().sprite = Sprite.Create(t, sp.rect, sp.pivot);
            OtherButtons[i].GetComponent<LaneSprite>().Left = (int)System.Char.GetNumericValue(NewFormation[0]);
            OtherButtons[i].GetComponent<LaneSprite>().Right = (int)System.Char.GetNumericValue(NewFormation[2]);
             
             OtherButtons[i].onClick.AddListener(() => OnClick((int)System.Char.GetNumericValue(NewFormation[0]), 
                (int)System.Char.GetNumericValue(NewFormation[2])));
        }
        m_bButtonsUpdated = true;
    }
    //////////////////////////////////////////////////////
    public List<int> GenerateRandomLanes(int laneCount)
    {
        List<int> Lanes = new List<int>();
        List<int> LaneList = new List<int>();

        for (int i = 0; i < 5; i++)
            LaneList.Add(0);

        for (int i = 0; i < laneCount; i++)
        {
            int Lane = Random.Range(1, 6);


            bool contains = false;

            for (int j = 0; j < Lanes.Count; j++)
            {
                if (Lanes[j] == Lane)
                    contains = true;
            }
            while (contains)
            {
                Lane = Random.Range(1, 6);

                contains = false;
                for (int j = 0; j < Lanes.Count; j++)
                {
                    if (Lanes[j] == Lane)
                        contains = true;
                }
            }
            LaneList[Lane - 1] = 1;
            Lanes.Add(Lane);
        }

        return LaneList;
    }
    //////////////////////////////////////////////////////
    public List<int> GetLanes(List<int> l)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i] == 1)
                list.Add(i);
        }
        return list;
    }
    //////////////////////////////////////////////////////
    public void SetGameState(GameState s)
    {
        m_eState = s;

        UpdateGameState();
    }
    //////////////////////////////////////////////////////
    public void UpdateGameState()
    {
        RightFuzzy.SendMessage("UpdateGameState", m_eState);
        LeftFuzzy.SendMessage("UpdateGameState", m_eState);

        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] != null)
            {
                BaseEnemy b = list[i].GetComponent<BaseEnemy>();
                if (b)
                    b.UpdateGameState(m_eState);
            }
        }
    }
    //////////////////////////////////////////////////////
    void SetCollision(Collision c)
    {
        m_bCollision = true;
        m_hitWave = FindEnemyOnLane(c.collider.gameObject);
    }
    public bool RaycastEnemy()
    {
        float lineLen = 10;
        Ray rToLeft = new Ray(new Vector3(-9, 2, -lineLen), new Vector3(0, 0, lineLen * 1.5f));


        Debug.DrawRay(new Vector3(RightFuzzy.transform.position.x, RightFuzzy.transform.position.y, -lineLen), new Vector3(0, 0, lineLen * 1.5f), Color.red);


        RaycastHit rLeftHit;

        bool AnyHit = false;

        if (Physics.Raycast(rToLeft, out rLeftHit))
        {
            if (rLeftHit.collider.gameObject.tag == "Enemy")
            {
                m_hitWave = FindEnemyOnLane(rLeftHit.collider.gameObject);
                //Debug.Log("Hit from " + m_hitWave.Id);
                AnyHit = true;
            }
        }


        return AnyHit;
    }
    public Wave FindEnemyOnLane(GameObject g)
    {
        Wave dummy = new Wave();
        dummy.BlockCount = 0;
        for (int i = 0; i < m_sLanes.Count; i++)
        {
            Wave lane = m_sLanes[i];

            for (int j = 0; j < lane.LaneBlocks.Count; j++)
            {
                GameObject block = lane.LaneBlocks[j];

                if (block != null)
                {
                    if (block == g)
                    {
                        return m_sLanes[i + 1];
                    }
                }
            }
        }
        return dummy;
    }
}
//////////////////////////////////////////////////////