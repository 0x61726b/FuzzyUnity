using UnityEngine;
using Prime31;



#if UNITY_ANDROID
public class GPGPlayerInfo
{
	// Note that the Android SDK has separate methods now for avatar URIs and URLs. URIs must be loaded by the Play SDK via the loadProfileImageForUri method.
	// URLs can be loaded via WWW or any other way your app accesses network resources. Also note that you should check both the URI and URL fields. Sometimes Google
	// provides only the URI or only the URL but not both!
	public string avatarUri; // Android only
	public string avatarUrl;
	public string avatarUriHiRes; // Android only
	public string avatarUrlHiRes; // Android only
	public string name;
	public string playerId;
	public string title;
	public System.Int64 currentExperiencePoints;
	public System.Int64 lastLevelUpTimestamp;

	public GPGPlayerLevel currentLevel;
	public GPGPlayerLevel nextLevel;
}
#endif