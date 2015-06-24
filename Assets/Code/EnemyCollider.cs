using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour
{
    public MenuController mc;
    bool entered;

    public FormationHandler fh;

    void Start()
    {
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider c)
    {
        if(!entered)
        {
            mc.IncrementScore();
            entered = true;
        }
        fh.OnWaveCollision(c);
    }

    void OnTriggerExit(Collider c)
    {
        entered = false;
    }
}
