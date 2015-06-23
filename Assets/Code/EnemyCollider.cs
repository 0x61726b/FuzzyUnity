using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCollider : MonoBehaviour
{

    public Text scoreText;
    int score;
    bool entered;
    // Use this for initialization
    void Start()
    {
        entered = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision c)
    {
        entered = true;

    }
    void OnCollisionExit(Collision c)
    {
        if(entered){
             score += 1;
             scoreText.text = score.ToString();
            GameObject.Find("Logic").SendMessage("SetCollision", c);
            entered = false;
        }
       
    }
}
