using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private Vector2 m_vInputPosition;
	private bool m_bDragging = false;
	public GameObject Fuzzy1;
	public GameObject Fuzzy2;
	public GameObject Fuzzy3;

	private GameObject MovingFuzzy;

	private Vector2 m_vInputStart;
	private Vector2 m_vInputEnd;
	private float m_fElapsed = 0.0f;

	public int MaximumDragLimit = 2;
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		ControlDragging ();

		if (m_bDragging)
		{
			RaycastHit hit;
			Ray ry = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ry, out hit)) 
			{
				if (hit.collider.tag == "Fuzzy")
				{
					if(hit.collider.name == Fuzzy1.name)
					{
						MovingFuzzy = Fuzzy1;
					}
					if(hit.collider.name == Fuzzy2.name)
					{
						MovingFuzzy = Fuzzy2;
					}
					if(hit.collider.name == Fuzzy3.name)
					{
						MovingFuzzy = Fuzzy3;
					}
				}
			}
			ControlTime();
		}
	}
	private void ControlTime()
	{
		m_fElapsed += Time.deltaTime;

		if (m_fElapsed >= MaximumDragLimit)
		{
			m_fElapsed = 0.0f;
			m_bDragging = false;
		} 
		else
		{

		}
	}
	private void MoveFuzzy()
	{
		

		Vector3 InputEndWorld = Camera.main.ScreenToWorldPoint (m_vInputEnd);
		Vector3 InputStartWorld = Camera.main.ScreenToWorldPoint (m_vInputStart);
		Vector3 Delta = InputEndWorld - InputStartWorld;
		Delta = Delta.normalized;
		if (MovingFuzzy != null)
		{
			MovingFuzzy.GetComponent<Rigidbody>().velocity = Delta*5;
		}
		MovingFuzzy = null;
	}
	private void ControlDragging()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			m_bDragging = true;
			m_vInputStart = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			m_bDragging = false;
			m_vInputEnd = Input.mousePosition;

			MoveFuzzy();
		}
	}
}
