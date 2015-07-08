using UnityEngine;
using System.Collections;

public class DisplayAd : MonoBehaviour {
    AdmobAdapter admobAdapter = new AdmobAdapter();

    void Awake() {
        admobAdapter.Awake();
    }
    // Use this for initialization
	void Start () {
        admobAdapter.Start();
	}

	// Update is called once per frame
	void Update () {

	}

    void Destroy() {
        admobAdapter.Destroy();
    }

    void OnGUI() {
		if (GUI.Button(new Rect(getLeftMargin(),getTopMargin(2),getWidth(),getHeight()),"Display Interstitial"))
			admobAdapter.ShowInterstitial();
		if (GUI.Button(new Rect(getLeftMargin(),getTopMargin(3),getWidth(),getHeight()),"Show Banner"))
            admobAdapter.ShowBanner();
		if (GUI.Button(new Rect(getLeftMargin(),getTopMargin(4),getWidth(),getHeight()),"Hide Banner"))
            admobAdapter.HideBanner();
	}

	int getWidth() {
		return (int) (Screen.width * 0.9);
	}

	int getHeight() {
		return (int)(Screen.height * 0.18);
	}

	int getTopMargin(int index) {
		return (int)(((Screen.height * 0.2)*index) + (Screen.height * 0.01));
	}

	int getLeftMargin() {
		return (int)(Screen.width * 0.05);
	}
}
