using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lane
{
    public List<int> Binary { get; set; }

    public List<Block> Blocks { get; set; }

    public Block DefaultBlock { get; set; }

    //Wave reference for science
    public WaveBase Wave { get; set; }

    public Lane()
    {
        Binary = new List<int>();
        Blocks = new List<Block>();

        //Add default elements
        Block elem = new Block();
        elem.Index = -1;
        elem.Prefab = Resources.Load("Prefabs/Enemy") as GameObject;
        DefaultBlock = elem;
    }
    public void SetDefaultBlock(Block le)
    {
        le.Index = -1;
        DefaultBlock = le;

    }
    public LaneElement GetLaneElement(int index, LaneElement.ElementType type)
    {

        switch (type)
        {
            case LaneElement.ElementType.Block:
                for (int i = 0; i < Blocks.Count; i++)
                {
                    LaneElement elem = Blocks[i];

                    if (elem.Index == index)
                        return elem;
                }
                break;
        }
        return DefaultBlock;
    }
    public virtual void Initialize()
    {
        for (int i = 0; i < WaveBase.LANE_COUNT; i++)
        {
            if (Binary[i] == 1)
            {
                Block b = new Block();
                b.Index = i;
                b.Name = "Block at #" + i.ToString();
                b.XSpeed = Wave.Speed;
                GameObject prefab = null;
                //Check if there's assigned elem for this
                LaneElement elem = this.GetLaneElement(i, LaneElement.ElementType.Block);
                if (elem.Index == i)
                {
                    prefab = elem.Prefab;
                }
                else if (elem.Index == -1)
                {
                    prefab = this.DefaultBlock.Prefab;
                }
                b.Prefab = prefab;

                Blocks.Add(b);
            }
        }
    }
    public void Update()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].Update();
        }
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
