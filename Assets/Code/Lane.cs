//.     .       .  .   . .   .   . .    +  .
//  .     .  :     .    .. :. .___---------___.
//       .  .   .    .  :.:. _".^ .^ ^.  '.. :"-_. .
//    .  :       .  .  .:../:            . .^  :.:\.
//        .   . :: +. :.:/: .   .    .        . . .:\
// .  :    .     . _ :::/:               .  ^ .  . .:\
//  .. . .   . - : :.:./.                        .  .:\
//  .      .     . :..|:                    .  .  ^. .:|
//    .       . : : ..||        .                . . !:|
//  .     . . . ::. ::\(                           . :)/
// .   .     : . : .:.|. ######              .#######::|
//  :.. .  :-  : .:  ::|.#######           ..########:|
// .  .  .  ..  .  .. :\ ########          :######## :/
//  .        .+ :: : -.:\ ########       . ########.:/
//    .  .+   . . . . :.:\. #######       #######..:/
//     :: . . . . ::.:..:.\           .   .   ..:/
//  .   .   .  .. :  -::::.\.       | |     . .:/
//     .  :  .  .  .-:.":.::.\             ..:/
// .      -.   . . . .: .:::.:.\.           .:/
//.   .   .  :      : ....::_:..:\   ___.  :/
//   .   .  .   .:. .. .  .: :.:.:\       :/
//     +   .   .   : . ::. :.:. .:.|\  .:/|
//     .         +   .  .  ...:: ..|  --.:|
//.      . . .   .  .  . ... :..:.."(  ..)"
// .   .       .      :  .   .: ::/  .  .::\


//      __       ___  ___  ___  ___      ___       ___      ___       __        ______    
//     /""\     |"  \/"  ||"  \/"  |    |"  |     |"  \    /"  |     /""\      /    " \   
//    /    \     \   \  /  \   \  /     ||  |      \   \  //   |    /    \    // ____  \  
//   /' /\  \     \\  \/    \\  \/      |:  |      /\\  \/.    |   /' /\  \  /  /    ) :) 
//  //  __'  \    /   /     /   /        \  |___  |: \.        |  //  __'  \(: (____/ //  
// /   /  \\  \  /   /     /   /        ( \_|:  \ |.  \    /:  | /   /  \\  \\        /   
//(___/    \___)|___/     |___/          \_______)|___|\__/|___|(___/    \___)\"_____/    
//--------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//--------------------------------------------------------------------------------
public class Lane
{
    //--------------------------------------------------------------------------------
    public List<int> Binary { get; set; }
    //--------------------------------------------------------------------------------
    public List<Block> Blocks { get; set; }
    //--------------------------------------------------------------------------------
    public Block DefaultBlock { get; set; }
    //--------------------------------------------------------------------------------
    public WaveBase Wave { get; set; }
    //--------------------------------------------------------------------------------
    public Lane()
    {
        Binary = new List<int>();
        Blocks = new List<Block>();

        //Add default elements
        Block elem = new Block();
        elem.Index = -1;
        elem.Prefab = "oneMesh";
        DefaultBlock = elem;
    }
    //--------------------------------------------------------------------------------
    public void SetDefaultBlock(Block le)
    {
        le.Index = -1;
        DefaultBlock = le;

    }
    //--------------------------------------------------------------------------------
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
    //--------------------------------------------------------------------------------
    public void UpdateSpeed()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].XSpeed = Wave.Speed;
        }
    }
    //--------------------------------------------------------------------------------
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
                string prefab = "";
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
    //--------------------------------------------------------------------------------
    public void Update()
    {
        for (int i = 0; i < Blocks.Count; i++)
        {
            Blocks[i].Update();
        }
    }
    //--------------------------------------------------------------------------------
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
    //--------------------------------------------------------------------------------
    public static List<List<int>> GetDistinctiveLanes(int count, List<List<int>> solutions)
    {
        List<List<int>> DistinctiveLanes = new List<List<int>>();
        if (count > 0)
        {
            bool contains = false;
            int k = 0;
            while (!contains)
            {
                List<int> randomLane = Lane.GenerateRandomLane(2);

                if (!ContainsLane(DistinctiveLanes, randomLane))
                {
                    DistinctiveLanes.Add(randomLane);
                    k++;
                }
                if (k >= count)
                {
                    contains = true;
                }
            }
        }
        return DistinctiveLanes;
    }
    //--------------------------------------------------------------------------------
    public static bool ContainsLane(List<List<int>> lanes, List<int> randomLane)
    {
        bool result = false;
        bool has2elems = false;
        for (int i = 0; i < lanes.Count; i++)
        {
            List<int> t = lanes[i];

            for (int k = 0; k < t.Count; k++)
            {
                int ti = t[k];

                if (ti == 1)
                {
                    int index = k;

                    if (randomLane[index] == 1 && has2elems)
                        result = true;

                    if (randomLane[index] == 1)
                        has2elems = true;
                }
            }
        }
        return result;
    }
    //--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------