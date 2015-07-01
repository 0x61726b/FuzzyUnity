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
//--------------------------------------------------------------------------------
public class DoubleWave : WaveBase
{
    private float m_iSleepCounter = 0;
    //--------------------------------------------------------------------------------
    public DoubleWave()
    {
        int laneObjCount = Random.Range(2, 4);
        Lane l = new Lane();
        l.Binary = Lane.GenerateRandomLane(laneObjCount);

        Block lElement = new Block();
        lElement.Prefab = "oneMesh";
        l.SetDefaultBlock(lElement);

        Speed = new Vector3(-5.5f, 0.0f, 0.0f);
        SpawnPosition = new Vector3(35.16f, 2.86f, -2);
        l.Wave = this;

        laneObjCount = Random.Range(2, 4);
        Lane d = new Lane();
        d.Binary = Lane.GenerateRandomLane(laneObjCount);
        d.SetDefaultBlock(lElement);
        d.Wave = this;

        Name = "Double Wave";
        Type = WaveType.OaA;

        SleepDuration = 1;

        Lanes.Add(l);
        Lanes.Add(d);
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
