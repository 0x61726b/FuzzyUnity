using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lane
{
    public List<int> Binary { get; set; }

    public List<LaneElement> Blocks { get; set; }
    public List<Powerup> Powerups { get; set; }

    public Lane()
    {
        Binary = new List<int>();
        Blocks = new List<LaneElement>();
        Powerups = new List<Powerup>();

        //Add default elements
        LaneElement elem = new LaneElement(LaneElement.ElementType.Block);
        elem.Index = -1;
        elem.Prefab = Resources.Load("Prefabs/Enemy") as GameObject;
        Blocks.Add(elem);

        Powerup lPowerup = new Powerup();
        lPowerup.Prefab = Resources.Load("Prefabs/Powerup") as GameObject;
        lPowerup.Index = -1;
        Powerups.Add(lPowerup);
    }
    public void SetDefaultElement(LaneElement le)
    {
        Blocks[0] = le;
    }
    public void SetDefaultPowerup(Powerup le)
    {
        Powerups[0] = le;
    }
    public LaneElement GetLaneElement(int index)
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            LaneElement elem = Blocks[i];

            if (elem.Index == index && elem.Type == LaneElement.ElementType.Powerup)
                return elem;
        }
        return Blocks[0];
    }
    public LaneElement GetPowerup(int index)
    {
        for (int i = 0; i < Powerups.Count; i++)
        {
            LaneElement elem = Powerups[i];

            if (elem.Index == index && elem.Type == LaneElement.ElementType.Powerup)
                return elem;
        }
        return Powerups[0];
    }
    public virtual void Initialize()
    {


    }
    public virtual void Update()
    {

    }
    public static List<int> GenerateRandomLane(int laneCount)
    {
        List<int> Lanes = new List<int>();
        List<int> LaneList = new List<int>();

        for (int i = 0; i < 5; i++)
            LaneList.Add(0);

        for (int i = 0; i < laneCount; i++)
        {
            int Lane = Random.Range(1, 6);


            bool contains = false;

            for (int j = 0; j < Lanes.Count; j++)
            {
                if (Lanes[j] == Lane)
                    contains = true;
            }
            while (contains)
            {
                Lane = Random.Range(1, 6);

                contains = false;
                for (int j = 0; j < Lanes.Count; j++)
                {
                    if (Lanes[j] == Lane)
                        contains = true;
                }
            }
            LaneList[Lane - 1] = 1;
            Lanes.Add(Lane);
        }

        return LaneList;
    }
}
