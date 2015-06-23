using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour 
{
    public const int BUTTON_COUNT = 3;
    public Button LeftButton;
    public Button CenterButton;
    public Button RightButton;

    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;

    private List<Button> NotPickedButtons;
	
	void Start () 
    {
        NotPickedButtons = new List<Button>();

	}
	
	
	void Update () 
    {
	
	}
    public void UpdateButtons(List<List<int>> Solutions)
    {
        int correctSolutionCount = Solutions.Count;
        int falseSolutionCount = BUTTON_COUNT - correctSolutionCount;

        NotPickedButtons.Add(LeftButton);
        NotPickedButtons.Add(CenterButton);
        NotPickedButtons.Add(RightButton);

        for (int i = 0; i < correctSolutionCount; i++)
        {
            Button correctButton = GetNotAssignedButton();
            UpdateButton(correctButton,Solutions[i]);
        }
        List<List<int>> FalseLanes = Lane.GetDistinctiveLanes(falseSolutionCount, Solutions);

        for (int i = 0; i < FalseLanes.Count; i++)
        {
            List<int> lane = FalseLanes[i];
            Button falseButton = GetNotAssignedButton();
            UpdateButton(falseButton, lane);
        }
    }
    public void UpdateButton(Button b, List<int> solution)
    {
        Sprite sp = b.GetComponent<Image>().sprite;
        string LaneNumber = GetResourceName(solution);

        Texture2D t = LoadButtonTexture(solution);

        b.GetComponent<Image>().sprite = Sprite.Create(t, sp.rect, sp.pivot);
        b.GetComponent<LaneSprite>().Left = (int)System.Char.GetNumericValue(LaneNumber[0]);
        b.GetComponent<LaneSprite>().Right = (int)System.Char.GetNumericValue(LaneNumber[2]);
        b.onClick.AddListener(() => SetFuzzyLanes((int)System.Char.GetNumericValue(LaneNumber[0]),
   (int)System.Char.GetNumericValue(LaneNumber[2])));

    }
    public Button GetNotAssignedButton()
    {
        bool isPicked = false;
        Button b = null;
        while (!isPicked)
        {
            b = PickButton();
            for (int i = 0; i < NotPickedButtons.Count; i++)
            {
                
                if (b == NotPickedButtons[i])
                {
                    isPicked = true;
                    NotPickedButtons.Remove(b);
                }
            }
        }
        return b;
    }
    public Button PickButton()
    {
        int Rnd = Random.Range(1, BUTTON_COUNT+1);
        Button pickedButton = null;

        if (Rnd == 1)
        {
            pickedButton = LeftButton;
        }
        if (Rnd == 2)
        {
            pickedButton = RightButton;
        }
        if (Rnd == 3)
        {
            pickedButton = CenterButton;
        }
       
        return pickedButton;
    }
    public Texture2D LoadButtonTexture(List<int> Formation)
    {
        string textureName = GetResourceName(Formation);

        Texture2D t = Resources.Load(textureName) as Texture2D;
        return t;
    }
    public string GetResourceName(List<int> Formation)
    {
        System.Text.StringBuilder textureName = new System.Text.StringBuilder();
        bool first = false;
        for (int i = 0; i < Formation.Count; i++)
        {
            if (Formation[i] == 1 && first)
            {
                textureName.Append("-");
            }
            if (Formation[i] == 1)
            {
                first = true;
                textureName.Append(i + 1);
            }
        }
        return textureName.ToString();
    }
    public void SetFuzzyLanes(int left, int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;
    }

    public void TapToStart(GameObject b)
    {
        GameLogic.State = GameLogic.GameState.OnGoing;
        b.SetActive(false);

        GetComponent<WaveHandler>().SpawnFirstWave();
        GetComponent<FormationHandler>().UpdateFirstSpawn();
    }

}
