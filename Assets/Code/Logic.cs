using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Logic : MonoBehaviour {

    public Button LeftButton;
    public Button CenterButton;
    public Button RightButton;

    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;

	// Use this for initialization
	void Start () 
    {
        LeftButton.onClick.AddListener(() => OnClick(1,5));
        CenterButton.onClick.AddListener(() => OnClick(3,4));
        RightButton.onClick.AddListener(() => OnClick(2,4));
	}
    void OnClick(int left,int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;
    }
    void ChangeButton(Button btn,string buttonName,int leftLane,int rightLane)
    {
        btn.onClick.AddListener(() => OnClick(leftLane,rightLane));

        btn.GetComponent<ButtonLane>().Left = leftLane;
        btn.GetComponent<ButtonLane>().Right = rightLane;

        Texture2D sprites = Resources.Load(buttonName) as Texture2D;

        btn.image.sprite = Sprite.Create(sprites, btn.image.sprite.rect, btn.image.sprite.pivot);
        
    }
	// Update is called once per frame
	void Update ()
    {

	
	}
}
