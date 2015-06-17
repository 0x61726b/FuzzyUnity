using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Logic : MonoBehaviour {

    public Button LeftButton;
    public Button CenterButton;
    public Button RightButton;

    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;

    public BaseEnemy cBaseEnemy;

    public GameObject SpawnPoint;

    private float totalTime;

	// Use this for initialization
	void Start () 
    {
        LeftButton.onClick.AddListener(() => OnClick(1,5));
        CenterButton.onClick.AddListener(() => OnClick(3,4));
        RightButton.onClick.AddListener(() => OnClick(2,4));

        SpawnEnemyLogic();
	}
    void OnClick(int left,int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;
    }
    void ChangeButton(Button btn,int leftLane,int rightLane)
    {
        btn.onClick.AddListener(() => OnClick(leftLane,rightLane));

        btn.GetComponent<ButtonLane>().Left = leftLane;
        btn.GetComponent<ButtonLane>().Right = rightLane;

        Texture2D sprites = Resources.Load(""+leftLane+"-"+rightLane) as Texture2D;

        btn.image.sprite = Sprite.Create(sprites, btn.image.sprite.rect, btn.image.sprite.pivot);
        
    }


	// Update is called once per frame
	void Update ()
    {
        totalTime += Time.deltaTime;

        if (totalTime >= 3)
        {
            totalTime = 0;

            SpawnEnemyLogic();
        }
	}
    void SpawnEnemyLogic()
    {
        int LaneCntDecider = Random.Range(2, 4);

        int prevLane = -1;
        
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
            BasePosition.z = (Lane - 1) * (-1.5f);
            Instantiate(cBaseEnemy, BasePosition, new Quaternion());
            LaneCount++;
            Debug.Log("Spawning enemy at " + BasePosition);
            prevLane = Lane;
        }

    }
}
