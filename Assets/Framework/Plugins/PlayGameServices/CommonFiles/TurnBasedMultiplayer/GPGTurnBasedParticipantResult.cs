using UnityEngine;
using System.Collections;


#if UNITY_ANDROID
public enum GPGTurnBasedParticipantResultStatus
{
	// The participant won the match.
	Win,
	// The participant lost the match.
	Loss,
	// The participant tied the match.
	Tie,
	// There was no winner for the match (nobody wins or loses this kind of game.)
	None,
	// The participant disconnected / left during the match.
	Disconnect,
	// Different clients reported different results for this participant.
	Disagreed
}

public class GPGTurnBasedParticipantResult
{
	public string participantId;
	public int result;
	public int placing;


	public GPGTurnBasedParticipantResult( string participantId, GPGTurnBasedParticipantResultStatus result )
	{
		this.participantId = participantId;
		this.result = (int)result;
	}


	public GPGTurnBasedParticipantResult( string participantId, int placing )
	{
		this.participantId = participantId;
		this.placing = placing;
	}


	public GPGTurnBasedParticipantResult( string participantId, GPGTurnBasedParticipantResultStatus result, int placing )
	{
		this.participantId = participantId;
		this.result = (int)result;
		this.placing = placing;
	}
}
#endif