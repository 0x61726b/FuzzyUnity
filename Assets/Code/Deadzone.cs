using UnityEngine;
using System.Collections;

public class Deadzone : MonoBehaviour {


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    void OnTriggerEnter(Collider c)
    {
        FindWave(c.transform.parent.parent.gameObject);
    }
    public void FindWave(GameObject g)
    {
        string name = g.name;
        int waveIndex = name.IndexOf('#');
        string id = name.Substring(waveIndex+1);
        int waveID = System.Int32.Parse(id);

        GameObject.Find("GameLogic").SendMessage("DestroyWave", waveID);

        Destroy(g);
    }
}
