using UnityEngine;
using System.Collections;
using Prime31;


#if UNITY_ANDROID
public class GPGScore
{
	public string leaderboardId;
	public double rank;
	public double writeTimestamp;
	public string avatarUrl;
	public string avatarUrlHiRes; // Android only
	public string formattedRank;
	public long value;
	public string playerId;
	public string formattedScore;
	public string displayName;
}
#endif
