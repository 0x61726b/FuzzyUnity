using UnityEngine;
using Prime31;



#if UNITY_ANDROID
public class GPGMultiplayerParticipant
{
	public string participantId;
	public string displayName;
	public string iconImageUrl;
	public string hiResImageUrl;
	public bool isConnectedToRoom;
	// status will be one of the following: Invited, Joined, Declined, Left
	public string status;
}
#endif