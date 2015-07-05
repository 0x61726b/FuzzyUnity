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
using UnityEngine.UI;
using System.Collections.Generic;
//--------------------------------------------------------------------------------
public class InputHandler : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    public const int BUTTON_COUNT = 3;
    //--------------------------------------------------------------------------------
    public Animator PanelAnimator;
    //--------------------------------------------------------------------------------
    public Button LeftButton;
    public Button CenterButton;
    public Button RightButton;
    //--------------------------------------------------------------------------------
    public Fuzzy LeftFuzzy;
    public Fuzzy RightFuzzy;
    //--------------------------------------------------------------------------------
    private Image LeftButtonImage;
    private Image CenterButtonImage;
    private Image RightButtonImage;
    //--------------------------------------------------------------------------------
    private LaneSprite LeftButtonSc;
    private LaneSprite CenterButtonSc;
    private LaneSprite RightButtonSc;
    //--------------------------------------------------------------------------------
    private List<Button> NotPickedButtons;
    //--------------------------------------------------------------------------------
    private List<Sprite> m_Sprites;
    public AudioSource audio;
    private bool muted;
    //--------------------------------------------------------------------------------
    public FormationHandler fH;
    public WaveHandler gL;
    //--------------------------------------------------------------------------------
    public GameObject TapToStartButton;
    //--------------------------------------------------------------------------------
    public void Start()
    {
        muted = false;
        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            InputToogleMute();
        }
        NotPickedButtons = new List<Button>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures");
        m_Sprites = new List<Sprite>();
        m_Sprites.AddRange(sprites);

        LeftButtonImage = LeftButton.GetComponent<Image>();
        CenterButtonImage = CenterButton.GetComponent<Image>();
        RightButtonImage = RightButton.GetComponent<Image>();

        LeftButtonSc = LeftButton.GetComponent<LaneSprite>();
        CenterButtonSc = CenterButton.GetComponent<LaneSprite>();
        RightButtonSc = RightButton.GetComponent<LaneSprite>();

        //audio = GetComponent<AudioSource>();
    }
    //--------------------------------------------------------------------------------
    void Update()
    {
     
    }
    //--------------------------------------------------------------------------------
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
            UpdateButton(correctButton, Solutions[i]);
        }
        List<List<int>> FalseLanes = Lane.GetDistinctiveLanes(falseSolutionCount, Solutions);

        for (int i = 0; i < FalseLanes.Count; i++)
        {
            List<int> lane = FalseLanes[i];
            Button falseButton = GetNotAssignedButton();
            UpdateButton(falseButton, lane);
        }

        PanelAnimator.Play("ButtonChangeAnim");
    }
    //--------------------------------------------------------------------------------
    public void UpdateButton(Button b, List<int> solution)
    {
        string LaneNumber = GetResourceName(solution);

        Sprite t = LoadButtonTexture(solution);

        Image btnImage = null;
        LaneSprite btnScript = null;
        if (b == LeftButton)
        {
            btnImage = LeftButtonImage;
            btnScript = LeftButtonSc;
        }
        if (b == CenterButton)
        {
            btnImage = CenterButtonImage;
            btnScript = CenterButtonSc;
        }
        if (b == RightButton)
        {
            btnImage = RightButtonImage;
            btnScript = RightButtonSc;
        }

        btnImage.sprite = t;

        btnScript.Left = (int)System.Char.GetNumericValue(LaneNumber[0]);
        btnScript.Right = (int)System.Char.GetNumericValue(LaneNumber[2]);
        b.onClick.AddListener(() => SetFuzzyLanes((int)System.Char.GetNumericValue(LaneNumber[0]),
   (int)System.Char.GetNumericValue(LaneNumber[2])));
    }
    //--------------------------------------------------------------------------------
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
    //--------------------------------------------------------------------------------
    public Button PickButton()
    {
        int Rnd = Random.Range(1, BUTTON_COUNT + 1);
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
    //--------------------------------------------------------------------------------
    public Sprite LoadButtonTexture(List<int> Formation)
    {
        string textureName = GetResourceName(Formation);

        Sprite t = m_Sprites.Find(x => x.name == textureName);
        
        return t;
    }
    //--------------------------------------------------------------------------------
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
    //--------------------------------------------------------------------------------
    public void SetFuzzyLanes(int left, int right)
    {
        LeftFuzzy.CurrentLane = left;
        RightFuzzy.CurrentLane = right;

    }
    //--------------------------------------------------------------------------------
    public void TapToStart(GameObject b)
    {
        GameLogic.State = GameLogic.GameState.OnGoing;


        gL.SpawnFirstWave();
        fH.UpdateFirstSpawn();

        b.SetActive(false);
    }
    //--------------------------------------------------------------------------------
    public void Restart()
    {
        TapToStartButton.SetActive(true);
        SetFuzzyLanes(3, 5);
    }
    //--------------------------------------------------------------------------------

    public void LeftButtonClicked()
    {
        PanelAnimator.SetInteger("ButtonNumber" , 1);
        PanelAnimator.Play("ButtonClicked");
        audio.Play();
    }

    public void CenterButtonClicked()
    {
        PanelAnimator.SetInteger("ButtonNumber", 2);
        PanelAnimator.Play("ButtonClicked");
        audio.Play();
    }

    public void RightButtonClicked()
    {
        PanelAnimator.SetInteger("ButtonNumber", 3);
        PanelAnimator.Play("ButtonClicked");
        audio.Play();
    }

    public void InputToogleMute()
    {

        audio.volume = muted ? 0.7f : 0;
        muted = !muted;



    }
 
}
