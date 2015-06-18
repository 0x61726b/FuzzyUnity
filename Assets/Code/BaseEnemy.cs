using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public float XSpeed = -0.3f;
	// Use this for initialization
    private Logic.GameState m_eState;
    private bool bShouldUpdate = true;
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
       if( m_eState == Logic.GameState.OnGoing)
           transform.Translate(new Vector3(XSpeed, 0, 0)); ;

        if (m_eState == Logic.GameState.OnGoing && bShouldUpdate)
        {
            //Base Movement
            
            //Dead Zone
            if (transform.position.x < -11)
            {
                GameObject.Find("Logic").SendMessage("EnemyDeadZone");
                bShouldUpdate = false;
            }
        }
        if (transform.position.x < -25)
        {
            Destroy(gameObject);
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
