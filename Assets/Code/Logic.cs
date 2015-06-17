using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class Logic : MonoBehaviour
{
    public LaneSprite LeftSprite;
    public LaneSprite CenterSprite;
    public LaneSprite RightSprite;

    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;

    public BaseEnemy cBaseEnemy;

    public GameObject SpawnPoint;

    private float totalTime;

    public enum GameState
    {
        NotStarted,
        OnGoing,
        Over
    }

    private GameState m_eState;

    // Use this for initialization
    void Start()
    {
        SpawnEnemyLogic();

        m_eState = GameState.OnGoing;

    }
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
    void OnClick(int left, int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;
    }


    // Update is called once per frame
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
        }
    }
    void SpawnEnemyLogic()
    {
        int LaneCntDecider = Random.Range(2, 4);

        int prevLane = -1;
        List<int> laneList = new List<int>();


        for (int i = 0; i < 5; i++)
        {
            laneList.Add(0);
        }
        for (int i = 0; i < LaneCntDecider; i++)
        {
            //Enemy to spawn
            Vector3 BasePosition = SpawnPoint.transform.position;

            int LaneCount = 0;

            int Lane = Random.Range(1, 6);

            while (Lane == prevLane)
            {
                Lane = Random.Range(1, 6);
            }
            laneList[Lane - 1] = 1;

            BasePosition.z = (Lane - 1) * (-1.5f);
            GameObject g = Instantiate(cBaseEnemy, BasePosition, new Quaternion()) as GameObject;



            LaneCount++;

            prevLane = Lane;
        }
        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < list.Length; i++)
        {
            Physics.IgnoreCollision(list[i].GetComponent<BoxCollider>(), LeftSprite.GetComponent<Collider>());
            Physics.IgnoreCollision(list[i].GetComponent<BoxCollider>(), CenterSprite.GetComponent<Collider>());
            Physics.IgnoreCollision(list[i].GetComponent<BoxCollider>(), RightSprite.GetComponent<Collider>());
        }

        DetermineRightFormation(laneList);
    }
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
    }
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
    public void SetGameState(GameState s)
    {
        m_eState = s;

        UpdateGameState();
    }
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
}
