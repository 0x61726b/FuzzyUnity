using UnityEngine;
using System.Collections.Generic;


#if UNITY_ANDROID
public enum GPGTurnBasedMatchStatus
{
	// One or more slots need to be filled by auto-matching; the match cannot
	// be established until they are filled.
	AutoMatching,
	// The match has started.
	Active,
	// The match has finished.
	Complete,
	// The match was canceled
	Canceled,
	// The match expired due to inactivity
	Expired,
	// The match should no longer be shown on the client.
	Deleted
}


public enum GPGTurnBasedUserMatchStatus
{
	// The user has been invited to join the match and has not responded yet.
	Invited,
	// The user is waiting for their turn.
	AwaitingTurn,
	// The user has an action to take in the match.
	YourTurn,
	// The match has ended (it is completed, canceled, or expired.)
	MatchCompleted
}


public class GPGTurnBasedMatch
{
	#pragma warning disable 0649
	private string data;
	#pragma warning restore 0649
	public bool hasDataAvailable { get { return data != null; } }
	public bool canRematch;
	public byte[] matchData { get { return data != null ? System.Convert.FromBase64String( data ) : null; } }
	public string matchDescription;
	public string matchId;
	public int matchNumber;
	public int matchVersion;
	public string pendingParticipantId;
	public string localParticipantId;

	public int statusInt;
	public GPGTurnBasedMatchStatus status
	{
		get
		{
			return (GPGTurnBasedMatchStatus)System.Enum.ToObject( typeof( GPGTurnBasedMatchStatus ), statusInt );
		}
	}
	public string statusString { get { return status.ToString(); } }

	public int userMatchStatusInt;
	public GPGTurnBasedUserMatchStatus userMatchStatus
	{
		get
		{
			return (GPGTurnBasedUserMatchStatus)System.Enum.ToObject( typeof( GPGTurnBasedUserMatchStatus ), userMatchStatusInt );
		}
	}
	public string userMatchStatusString { get { return userMatchStatus.ToString(); } }

	public int availableAutoMatchSlots;
	public List<GPGTurnBasedParticipant> players;
	public bool isLocalPlayersTurn { get { return userMatchStatus == GPGTurnBasedUserMatchStatus.YourTurn; } }


	public override string ToString()
	{
		return Prime31.JsonFormatter.prettyPrint( Prime31.Json.encode( this ) );
	}
}
#endif