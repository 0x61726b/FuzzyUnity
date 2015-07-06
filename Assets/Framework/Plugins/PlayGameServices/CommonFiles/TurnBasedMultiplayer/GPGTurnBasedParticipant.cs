using UnityEngine;
using System.Collections;


#if UNITY_ANDROID
public enum GPGTurnBasedParticipantStatus
{
	Unknown = -1,
	// The Participant is slated to be invited to the match, but the invitation has
	// not been sent; the invite will be sent when it becomes their turn.
	NotInvited,
	// The Participant has been invited to join the match, but has not yet responded.
	Invited,
	// The Participant has joined the match (either after creating it or accepting an invitation.)
	Joined,
	// The Participant declined an invitation to join the match.
	Declined,
	// The Participant joined the match and then left it.
	Left,
	// The Participant finished the match.
	Finished,
	// The Participant did not take their turn in the allotted time.
	Unresponsive
}

public class GPGTurnBasedParticipant
{
	public GPGPlayerInfo player;
	public string participantId;
	public bool isAutoMatchedPlayer;
	public int statusInt;
	public GPGTurnBasedParticipantStatus status
	{
		get
		{
			return (GPGTurnBasedParticipantStatus)System.Enum.ToObject( typeof( GPGTurnBasedParticipantStatus ), statusInt );
		}
	}
	public string statusString { get { return status.ToString(); } }


	public override string ToString()
	{
		return Prime31.JsonFormatter.prettyPrint( Prime31.Json.encode( this ) );
	}
}
#endif