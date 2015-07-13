using UnityEngine;
using System.Collections;

public class ClickAudio : MonoBehaviour {
    bool muted;
    AudioSource audio;
	// Use this for initialization
	void Start () {
        muted = false;
        audio = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            MuteToggle();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MuteToggle()
    {
        audio.volume = muted ? 100 : 0;
        muted = !muted;
    }
}
