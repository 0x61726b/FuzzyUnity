﻿//.     .       .  .   . .   .   . .    +  .
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
public class WaveBase
{
    //--------------------------------------------------------------------------------
    public const int LANE_COUNT = 5;
    //--------------------------------------------------------------------------------
    public enum WaveType
    {
        Normal,
        OaA,
        Oa2A
    }
    //--------------------------------------------------------------------------------
    private WaveType m_Type;
    //--------------------------------------------------------------------------------
    public WaveType Type
    {
        get { return m_Type; }
        set { m_Type = value; }
    }
    //--------------------------------------------------------------------------------
    private int m_iWaveID;
    //--------------------------------------------------------------------------------
    public int WaveID
    {
        get { return m_iWaveID; }
        set { m_iWaveID = value; }
    }
    //--------------------------------------------------------------------------------
    private Vector3 m_vSpeed;
    //--------------------------------------------------------------------------------
    public Vector3 Speed
    {
        get { return m_vSpeed; }
        set { m_vSpeed = value; }
    }
    //--------------------------------------------------------------------------------
    private List<Lane> m_cLanes;
    //--------------------------------------------------------------------------------
    private List<GameObject> m_Prefabs;
    //--------------------------------------------------------------------------------
    public List<GameObject> Prefabs
    {
        get { return m_Prefabs; }
        set { m_Prefabs = value; }
    }
    //--------------------------------------------------------------------------------
    private List<int> m_DesiredPrefabIndices;
    //--------------------------------------------------------------------------------
    public List<int> DesiredPrefabIndices
    {
        get { return m_DesiredPrefabIndices; }
        set { m_DesiredPrefabIndices = value; }
    }
    //--------------------------------------------------------------------------------
    private Vector3 m_vSpawnPos;
    //--------------------------------------------------------------------------------
    public Vector3 SpawnPosition
    {
        get { return m_vSpawnPos; }
        set { m_vSpawnPos = value; }
    }
    //--------------------------------------------------------------------------------
    public List<Lane> Lanes
    {
        get { return m_cLanes; }
        set { m_cLanes = value; }
    }
    //--------------------------------------------------------------------------------
    private string m_sName;
    //--------------------------------------------------------------------------------
    public string Name
    {
        get { return m_sName; }
        set { m_sName = value; }
    }
    //---------------------------------------------------------------------------------
    private bool m_bSleeping;

    public bool Sleeping
    {
        get { return m_bSleeping; }
        set { m_bSleeping = value; }
    }
    private int m_iSleepLen;

    public int SleepDuration
    {
        get { return m_iSleepLen; }
        set { m_iSleepLen = value; }
    }

    //--------------------------------------------------------------------------------
    public WaveBase()
    {
        m_vSpawnPos = Vector3.zero;
        m_vSpeed = Vector3.zero;
        Lanes = new List<Lane>();
        m_cLanes = new List<Lane>();
        m_Type = WaveType.Normal;
        m_Prefabs = new List<GameObject>();
        m_sName = "Wave Unknown";
        SpawnPosition = new Vector3(15.16f, 2, -2);

        for (int i = 0; i < LANE_COUNT; i++)
            m_Prefabs.Add(null);

        m_DesiredPrefabIndices = new List<int>();
        m_DesiredPrefabIndices = InitializeListEmpty(m_DesiredPrefabIndices);

        m_bSleeping = false;

    }
    //--------------------------------------------------------------------------------
    public List<int> InitializeListEmpty(List<int> l)
    {
        for (int i = 0; i < LANE_COUNT; i++)
            l.Add(0);
        return l;
    }
    //--------------------------------------------------------------------------------
    public virtual void Initialize()
    {

    }
    //--------------------------------------------------------------------------------
    public virtual void Update()
    {

    }
    //--------------------------------------------------------------------------------
}
//--------------------------------------------------------------------------------