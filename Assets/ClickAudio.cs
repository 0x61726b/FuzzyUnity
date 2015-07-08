using UnityEngine;
using System.Collections;

public class ClickAudio : MonoBehaviour {
    bool muted;
    AudioSource audio;
	// Use this for initialization
	void Start () {
        muted = false;
        audio = GetComponent<AudioSource>();
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
