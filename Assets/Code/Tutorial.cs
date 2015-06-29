using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
    public GameObject buttonPanel;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();

        //if (PlayerPrefs.GetInt("FirstTime") == 1)
        //{
        //    this.gameObject.SetActive(false);
        //    buttonPanel.SetActive(false);
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("FirstTime", 1);
        //}
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartTutorial()
    {
        anim.Play("TutorialAnim");
    }
}
