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
    public Material DefaultMat;
    //////////////////////////////////////////////////////
    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;
    //////////////////////////////////////////////////////
    public BaseEnemy cBaseEnemy;
    //////////////////////////////////////////////////////
    public GameObject SpawnPoint;
    private Vector3 InitialSpawnPoint;
    private float totalTime;
    //////////////////////////////////////////////////////
    private int CURRENT_BUTTON_COUNT = 3;
    //////////////////////////////////////////////////////
    private List<GameObject> m_vCurrentEnemyList;
    private List<int> m_iCurrentLaneList;
    private bool m_bUpdateButtons = false;
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
        m_eState = GameState.OnGoing;

        m_vCurrentEnemyList = new List<GameObject>();
        m_iCurrentLaneList = new List<int>();

        SpawnEnemyLogic();
        DetermineRightFormation(m_iCurrentLaneList);
    }
    //////////////////////////////////////////////////////
    public void InputListener()
    {
        RaycastHit hit;
        Ray ray;
#if UNITY_EDITOR
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                {
                    if (hit.collider.tag == "FormationButton")
                        OnClick(hit.collider.GetComponent<LaneSprite>().Left,
                            hit.collider.GetComponent<LaneSprite>().Right);
                }
            }
        }
#endif
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
        totalTime += Time.deltaTime;
        UpdateGameState();
        if (m_eState != GameState.Over)
        {
            if (totalTime >= 3)
            {
                totalTime = 0;

                SpawnEnemyLogic();
            }
            InputListener();

            m_vCurrentEnemyList.Clear();
            GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < list.Length; i++)
            {
                m_vCurrentEnemyList.Add(list[i]);
            }


        }
    }
    //////////////////////////////////////////////////////
    void EnemyDeadZone()
    {
    
        DetermineRightFormation(m_iCurrentLaneList);

    }
    //////////////////////////////////////////////////////
    void SpawnEnemyLogic()
    {
        int LaneCntDecider = Random.Range(2, 4);

        List<int> Lanes = GenerateRandomLanes(LaneCntDecider);


        for (int i = 0; i < Lanes.Count; i++)
        {
            if (Lanes[i] == 1)
            {
                Vector3 BasePosition = SpawnPoint.transform.position;
                BasePosition.z = (i) * (-1.5f);
                Object g = Instantiate(cBaseEnemy, BasePosition, new Quaternion());
                g.name = "Enemy at " + i.ToString();
            }
        }

        m_iCurrentLaneList = Lanes;

        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < list.Length; i++)
        {
            m_vCurrentEnemyList.Add(list[i]);
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

        Texture TextureToSet;
        GameObject CorrectButton;

        Texture t = Resources.Load(Formation.ToString()) as Texture;
        TextureToSet = t;
        CorrectButton = GameObject.Find("Formation" + ChooseCorrectButton) as GameObject;


        CorrectButton.GetComponent<Renderer>().material.mainTexture = t;
        CorrectButton.GetComponent<LaneSprite>().Left = (int)System.Char.GetNumericValue(Formation[0]);
        CorrectButton.GetComponent<LaneSprite>().Right = (int)System.Char.GetNumericValue(Formation[2]);

        //NOTE TO SELF : DEBUG THIS SHIT
        List<int> buttonIndices = new List<int>();
        for (int i = 0; i < CURRENT_BUTTON_COUNT; i++)
            buttonIndices.Add(0);
        buttonIndices[ChooseCorrectButton - 1] = 1;

        List<GameObject> OtherButtons = new List<GameObject>();
        for (int i = 0; i < CURRENT_BUTTON_COUNT; i++)
        {
            if (buttonIndices[i] == 0)
                OtherButtons.Add(GameObject.Find("Formation" + (i + 1)) as GameObject);
        }

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

            t = Resources.Load(NewFormation.ToString()) as Texture;
            TextureToSet = t;


            OtherButtons[i].GetComponent<Renderer>().material.mainTexture = t;
            OtherButtons[i].GetComponent<LaneSprite>().Left = (int)System.Char.GetNumericValue(NewFormation[0]);
            OtherButtons[i].GetComponent<LaneSprite>().Right = (int)System.Char.GetNumericValue(NewFormation[2]);
        }
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
                list[i].SendMessage("UpdateGameState", m_eState);
            }
        }
    }
    //////////////////////////////////////////////////////
}
//////////////////////////////////////////////////////