using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public float XSpeed = -0.3f;
	// Use this for initialization
    private Logic.GameState m_eState;
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
       
        if (m_eState == Logic.GameState.OnGoing)
        {
            //Base Movement
            transform.Translate(new Vector3(XSpeed, 0, 0));

            //Dead Zone
            if (transform.position.x < -25)
                DestroyObject(gameObject);
        }
        if( m_eState == Logic.GameState.Over )
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}
    public void UpdateGameState(Logic.GameState state)
    {
        m_eState = state;
    }
}
