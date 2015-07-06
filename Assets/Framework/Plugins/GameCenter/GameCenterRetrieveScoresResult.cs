using UnityEngine;
using System.Collections.Generic;


#if UNITY_IPHONE || UNITY_STANDALONE_OSX
public class GameCenterRetrieveScoresResult
{
	public List<GameCenterScore> scores;
	public string category;
}
#endif