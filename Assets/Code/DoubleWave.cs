using UnityEngine;
using System.Collections;

public class DoubleWave : WaveBase
{
    public DoubleWave(int laneObjCount)
    {
       
        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);

        Block lElement = new Block();
        lElement.Prefab = Resources.Load("Prefabs/Enemy") as GameObject;
        MaterialColor = Color.red;
        l.SetDefaultBlock(lElement);

        Speed = new Vector3(-7, 0.0f, 0.0f);
        l.Wave = this;

        Lane d = new Lane();
        d.Binary = Lane.GenerateRandomLane(laneObjCount);
        d.SetDefaultBlock(lElement);
        d.Wave = this;

        Name = "Double Wave";
        Type = WaveType.Oa2A;

        Lanes.Add(l);
        Lanes.Add(d);
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
