using UnityEngine;
using System.Collections;

public class TripleWave : WaveBase 
{
    public TripleWave()
    {
        Block lElement = new Block();
        lElement.Prefab = "oneMesh";

        int laneObjCount = Random.Range(2, 4);

        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);

        laneObjCount = Random.Range(2, 4);

        Lane l2 = new Lane();
        l2.Binary = Lane.GenerateRandomLane(laneObjCount);

        laneObjCount = Random.Range(2, 4);

        Lane l3 = new Lane();
        l3.Binary = Lane.GenerateRandomLane(laneObjCount);

        Speed = new Vector3(-7, 0.0f, 0.0f);
        SpawnPosition = new Vector3(15.16f, 1.4f, -2);

        Name = "Triple Wave";
        Type = WaveType.Oa2A;

        Lanes.Add(l);
        Lanes.Add(l2);
        Lanes.Add(l3);
    }
    public override void Initialize()
    {
        for (int i = 0; i < Lanes.Count; i++)
            Lanes[i].Initialize();
    }
    public override void Update()
    {
        for (int j = 0; j < Lanes.Count; j++)
        {
            Lane l = Lanes[j];

            l.Update();
        }
    }
}
