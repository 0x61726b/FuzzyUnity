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
        lElement.Prefab = "oneMesh";
        l.SetDefaultBlock(lElement);
        //MaterialColor = new Color(0.06f, 0.06f, 0.35f);

        Speed = new Vector3(-7, 0.0f, 0.0f);
        SpawnPosition = new Vector3(15.16f, 1.4f, -2);
        l.Wave = this;

        Name = "Normal Wave";

        Lanes.Add(l);

        Type = WaveType.Normal;
        
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
