using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private int m_iRowCount;
	private int m_iColumnCount;

	private int m_iGridWidth;
	private int m_iGridDepth;
	// Use this for initialization
	void Start () 
	{
		m_iRowCount = 10;
		m_iColumnCount = 10;

		m_iGridWidth = 10;
		m_iGridDepth = 20;



	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i=0; i < m_iRowCount; i++) {
			for(int j=0; j < m_iColumnCount; j++)
			{
				Vector3 vStart = new Vector3(m_iColumnCount*m_iGridDepth ,0,0 );
				Vector3 vEnd = new Vector3(m_iColumnCount* m_iGridDepth,10,0);

				Debug.DrawLine(vStart,vEnd);
			}
		}
	}
}
