using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Tapped()
    {
        animator.SetBool("tapped", true);
    }
}
