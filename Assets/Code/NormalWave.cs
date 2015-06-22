using UnityEngine;
using System.Collections;

public class NormalWave : WaveBase 
{
    public NormalWave(int laneObjCount)
    {
        //Normal Wave = 1 lane of blocks
        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);

        LaneElement lElement = new LaneElement(LaneElement.ElementType.Block);
        lElement.Prefab = Resources.Load("Prefabs/EnemyModel") as GameObject;
        l.SetDefaultElement(lElement);

        Powerup lPowerup = new Powerup();
        lPowerup.Prefab = Resources.Load("Prefabs/Powerup") as GameObject;
        l.Powerups.Add(lPowerup);
        
        Name = "Normal Wave";
        
        Lanes.Add(l);
    }
    public virtual void Initialize()
    {
        Speed = new Vector3(-0.1f, 0, 0);
    }
    public virtual void Update()
    {
        for( int j=0; j < Lanes.Count; j++ )
        {
            Lane l = Lanes[j];

          
        }
    }
	
}
