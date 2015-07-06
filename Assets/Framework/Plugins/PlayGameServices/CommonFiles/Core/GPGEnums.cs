using UnityEngine;
using System.Collections;


#if UNITY_ANDROID
public enum GPGToastPlacement
{
	Top,
	Bottom,
	Center
}


public enum GPGLeaderboardTimeScope
{
	Unknown = -1,
	Today = 1,
	ThisWeek = 2,
	AllTime = 3
}


public enum GPGQuestState
{
	// Upcoming quest, before start time.
	Upcoming,
	// Open quest, in between start time and expiration time.
	Open,
	// Quest accepted by player, in between start time and expiration time.
	Accepted,
	// Quest completed by player, rewards been claimed.
	Completed,
	// Quest expired, not accepted by player, after expiration time.
	Expired,
	// Quest expired, accepted by player, after expiration time.
	Failed
}


public enum GPGQuestMilestoneState
{
	// Milestone is not completed.
	NotCompleted,
	// Milestone is completed, not claimed.
	CompletedNotClaimed,
	// Milestone is completed, claimed.
	Claimed
}


public enum GPGSnapshotConflictPolicy
{
	// In the event of a conflict, the conflict data will be returned to you, and you must resolve
	// the conflict. This is the only policy where the conflicts will be visible to you. Use this for
	// a custom merge. This policy ensures that no user changes to the state of the save game
	// will ever be lost.
	//Manual, NOT SUPPORTED

	// In the event of a conflict, the snapshot with the largest playtime value will be used.
	// This policy is a good choice if the length of play time is a reasonable
	// proxy for the "best" save game. Note that you must use
	// |SnapshotMetadataChange.playedTime| when saving games for this policy to be meaningful.
	//LongestPlaytime, NOT SUPPORTED
	// In the event of a conflict, the conflictSnapshot, or the last agreed upon
	// good version of the snapshot will be used.
	// This policy is a reasonable choice if your game requires stability from
	// the snapshot data. This policy ensures that only writes which are not
	// contested are seen by the player, which guarantees that all clients
	// converge.
	// Note: previously |GPGSnapshotConflictPolicyBaseWins|.
	LastKnownGood = 2,
	// In the event of a conflict, the most recently modified snapshot will be used.
	// This policy is a reasonable choice if your game can tolerate players
	// on multiple devices clobbering their own changes. Because this policy
	// blindly chooses the most recent data, it is possible that a player's
	// changes may get lost.
	// Note: previously |GPGSnapshotConflictPolicyRemoteWins|.
	MostRecentlyModified
};

#endif