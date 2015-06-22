using UnityEngine;
using System.Collections;

public class NormalWave : WaveBase
{
    public NormalWave(int laneObjCount)
    {
        //Normal Wave = 1 lane of blocks
        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);

        Block lElement = new Block();
        lElement.Prefab = Resources.Load("Prefabs/Enemy") as GameObject;
        l.SetDefaultBlock(lElement);

        Speed = new Vector3(-0.1f, 0.0f, 0.0f);
        SpawnPosition = new Vector3(15.16f, 2, -2);
        l.Wave = this;

        Name = "Normal Wave";

        Lanes.Add(l);

        
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
