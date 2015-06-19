using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision c)
    {
        GameObject.Find("Logic").SendMessage("SetCollision",c);
    }
}
