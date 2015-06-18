using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CameraScript : MonoBehaviour {
    public Text scoreText;
    
    private float score = 0;
	// Use this for initialization
	void Start () {
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
#if !UNITY_EDITOR
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            Debug.Log("SHO");
            Camera.main.orthographicSize = 18.5f;

        }
        else
        {
            Debug.Log("ehe");
            Camera.main.orthographicSize = 6f;
        }

#endif
        score += Time.deltaTime/2;
        scoreText.text = System.Math.Floor(score).ToString();

    }

    public void Restart()
    {
        Application.LoadLevel("MainScene");
    }

}
