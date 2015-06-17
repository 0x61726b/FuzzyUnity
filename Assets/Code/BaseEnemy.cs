using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public float XSpeed = -0.3f;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Base Movement
        transform.Translate(new Vector3(XSpeed, 0, 0));
	}
}
