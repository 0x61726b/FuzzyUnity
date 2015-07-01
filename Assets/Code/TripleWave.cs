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

public class TripleWave : WaveBase 
{
    private float m_iSleepCounter = 0;
    //--------------------------------------------------------------------------------
    public TripleWave()
    {
        Block lElement = new Block();
        lElement.Prefab = "oneMesh";
        

        int laneObjCount = Random.Range(2, 4);

        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);
        l.SetDefaultBlock(lElement);
        l.Wave = this;
        laneObjCount = Random.Range(2, 4);

        Lane l2 = new Lane();
        l2.Binary = Lane.GenerateRandomLane(laneObjCount);
        l2.SetDefaultBlock(lElement);
        laneObjCount = Random.Range(2, 4);
        l2.Wave = this;

        Lane l3 = new Lane();
        l3.Binary = Lane.GenerateRandomLane(laneObjCount);
        l3.SetDefaultBlock(lElement);
        l3.Wave = this;

        Speed = new Vector3(-5.5f, 0.0f, 0.0f);
        SpawnPosition = new Vector3(35.16f, 2.86f, -2);

        Name = "Triple Wave";
        Type = WaveType.Oa2A;

        SleepDuration = 2;

        Lanes.Add(l);
        Lanes.Add(l2);
        Lanes.Add(l3);
    }
    //--------------------------------------------------------------------------------
    public override void Initialize()
    {
        for (int i = 0; i < Lanes.Count; i++)
            Lanes[i].Initialize();
    }
    //--------------------------------------------------------------------------------
    public override void Update()
    {
        if (!Sleeping)
        {
            for (int j = 0; j < Lanes.Count; j++)
            {
                Lane l = Lanes[j];

                l.Update();
            }
        }
        else
        {
            m_iSleepCounter += Time.deltaTime;
            if (m_iSleepCounter >= SleepDuration)
            {
                Sleeping = false;
                m_iSleepCounter = 0;
            }
        }
    }
    //--------------------------------------------------------------------------------
}
