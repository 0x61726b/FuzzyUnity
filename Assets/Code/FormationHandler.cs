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
public class FormationHandler : MonoBehaviour
{
    private bool bCheckCollision = false;
    private List<WaveBase> m_Waves;
    public List<WaveBase> Waves
    {
        get { return m_Waves; }
        set { m_Waves = value; }
    }
    //--------------------------------------------------------------------------------
    public void Start()
    {
        
    }
    //--------------------------------------------------------------------------------
    public void Update()
    {
        bCheckCollision = false;
    }
    //--------------------------------------------------------------------------------
    public void OnWaveCollision(Collider c)
    {
        if (!bCheckCollision)
        {
            FindWave(c.transform.parent.parent.gameObject);
        }
    }
    //--------------------------------------------------------------------------------
    public void FindWave(GameObject g)
    {
        string name = g.name;
        int waveIndex = name.IndexOf('#');
        string id = name.Substring(waveIndex+1);
        int waveID = System.Int32.Parse(id);
        Solve(waveID+1);
        
        bCheckCollision = true;
    }
    //--------------------------------------------------------------------------------
    public void Solve(int waveID)
    {
        WaveBase Wave = m_Waves[waveID];
        List<List<int>> Solutions = new List<List<int>>();
        for (int i = 0; i < Wave.Lanes.Count; i++)
        {
            Lane lane = Wave.Lanes[i];
            Solutions.Add(SolveForLane(lane));
        }
        GetComponent<InputHandler>().UpdateButtons(Solutions);
    }
    //--------------------------------------------------------------------------------
    public void UpdateFirstSpawn()
    {
        Solve(0);
    }
    //--------------------------------------------------------------------------------
    public List<int> SolveForLane(Lane lane)
    {
        List<List<int>> CorectSolutions = new List<List<int>>();
        List<int> laneList = lane.Binary;

        for (int j = 0; j < 3; j++)
        {
            List<int> possibleFormation = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                possibleFormation.Add(0);
            }
            int c = 0;
            for (int i = 0; i < laneList.Count; i++)
            {

                if (laneList[i] == 1)
                {
                    possibleFormation[i] = 0;
                    continue;
                }
                if (laneList[i] == 0)
                {
                    //POSSIBLE CANDIDATE
                    if (c < 2)
                    {
                        possibleFormation[i] = 1;
                        c++;
                    }
                }
            }
            CorectSolutions.Add(possibleFormation);
        }
        int ChooseSolution = Random.Range(1, CorectSolutions.Count);

        List<int> CorrectSolution = CorectSolutions[ChooseSolution];
        return CorrectSolution;
    }
    //--------------------------------------------------------------------------------
}
