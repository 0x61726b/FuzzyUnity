using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;



#if UNITY_ANDROID
public class GPGManager : AbstractManager
{
	// Fired when authentication succeeds. Includes the user_id
	public static event Action<string> authenticationSucceededEvent;

	// Fired when authentication fails
	public static event Action<string> authenticationFailedEvent;

	// iOS only. Fired when the user signs out. This could happen if in a leaderboard they touch the settings button and logout from there.
	public static event Action userSignedOutEvent;

	// Fired when data fails to reload for a key. This particular model data is usually the player info or leaderboard/achievement metadata that is auto loaded.
	public static event Action<string> reloadDataForKeyFailedEvent;

	// Fired when data is reloaded for a key
	public static event Action<string> reloadDataForKeySucceededEvent;

	// Android only. Fired when a license check fails
	public static event Action licenseCheckFailedEvent;

	// Android only. Fired when a profile image is loaded with the full path to the image
	public static event Action<string> profileImageLoadedAtPathEvent;

	// Fired when the user finishes sharing via the showShareDialog method. The string parameter will contain either an error message if sharing did not occur
	// or null if they shared successfullu
	public static event Action<string> finishedSharingEvent;

	// Fired when a call to loadPlayer completes. The event will include either a GPGPlayerInfo or an error string if an error occured.
	public static event Action<GPGPlayerInfo,string> loadPlayerCompletedEvent;


	// ##### ##### ##### ##### ##### ##### #####
	// ## Cloud Data
	// ##### ##### ##### ##### ##### ##### #####

	// Fired when loading cloud data fails. Provides the status code/GPGAppStateStatus.
	public static event Action<string> loadCloudDataForKeyFailedEvent;

	// Fired when loading cloud data succeeds and includes the key and data
	public static event Action<int,string> loadCloudDataForKeySucceededEvent;

	// Fired when updating cloud data fails. Provides the status code/GPGAppStateStatus.
	public static event Action<string> updateCloudDataForKeyFailedEvent;

	// Fired when updating cloud data succeeds and includes the key and data
	public static event Action<int,string> updateCloudDataForKeySucceededEvent;

	// Fired when clearing cloud data fails. Provides the status code/GPGAppStateStatus.
	public static event Action<string> clearCloudDataForKeyFailedEvent;

	// Fired when clearing cloud data succeeds and includes the key
	public static event Action<string> clearCloudDataForKeySucceededEvent;

	// Fired when deleting cloud data fails. Provides the status code/GPGAppStateStatus.
	public static event Action<string> deleteCloudDataForKeyFailedEvent;

	// Fired when deleting cloud data succeeds and includes the key
	public static event Action<string> deleteCloudDataForKeySucceededEvent;


	// ##### ##### ##### ##### ##### ##### #####
	// ## Achievements
	// ##### ##### ##### ##### ##### ##### #####

	// Fired when unlocking an achievement fails. Provides the achievementId and the error in that order.
	public static event Action<string,string> unlockAchievementFailedEvent;

	// Fired when unlocking an achievement succeeds. Provides the achievementId and a bool that lets you know if it was newly unlocked.
	public static event Action<string,bool> unlockAchievementSucceededEvent;

	// Fired when incrementing an achievement fails. Provides the achievementId and the error in that order.
	public static event Action<string,string> incrementAchievementFailedEvent;

	// Fired when incrementing an achievement succeeds. Provides the achievementId and a bool that lets you know if it was newly unlocked (on iOS, on Android the bool indicates success)
	public static event Action<string,bool> incrementAchievementSucceededEvent;

	// Fired when revealing an achievement fails. Provides the achievementId and the error in that order.
	public static event Action<string,string> revealAchievementFailedEvent;

	// Fired when revealing an achievement succeeds. The string lets you know the achievementId.
	public static event Action<string> revealAchievementSucceededEvent;


	// ##### ##### ##### ##### ##### ##### #####
	// ## Leaderboards
	// ##### ##### ##### ##### ##### ##### #####

	// Fired when submitting a score fails. Provides the leaderboardId and the error in that order.
	public static event Action<string,string> submitScoreFailedEvent;

	// Fired when submitting a scores succeeds. Returns the leaderboardId and a dictionary with some extra data with the fields from
	// the GPGScoreReport class: https://developers.google.com/games/services/ios/api/interface_g_p_g_score_report
	public static event Action<string,Dictionary<string,object>> submitScoreSucceededEvent;

	// Fired when loading scores fails. Provides the leaderboardId and the error in that order.
	public static event Action<string,string> loadScoresFailedEvent;

	// Fired when loading scores succeeds
	public static event Action<List<GPGScore>> loadScoresSucceededEvent;

	// Android only. Fired when a call to loadCurrentPlayerLeaderboardScore succeeds.
	public static event Action<GPGScore> loadCurrentPlayerLeaderboardScoreSucceededEvent;

	// Android only. Fired when a call to loadCurrentPlayerLeaderboardScore fails. Provides the leaderboardId and the error in that order.
	public static event Action<string,string> loadCurrentPlayerLeaderboardScoreFailedEvent;



	// ##### ##### ##### ##### ##### ##### #####
	// ## Events and Quests
	// ##### ##### ##### ##### ##### ##### #####

	// Fired when a call to loadAllEvents completes and includes the full event list available to this app
	public static event Action<List<GPGEvent>> allEventsLoadedEvent;

	// Fired when a quest is accepted via the quest list screen
	public static event Action<GPGQuest> questListLauncherAcceptedQuestEvent;

	// Fired when a quest milestone is reached. On iOS, the user needs to manually claim rewards on the quest list screen.
	public static event Action<GPGQuestMilestone> questClaimedRewardsForQuestMilestoneEvent;

	// Android only. Fired when a quest is completed.
	public static event Action<GPGQuest> questCompletedEvent;

	// Fired when a call to loadAllQuests completes and includes the full quest list
	public static event Action<List<GPGQuest>> allQuestsLoadedEvent;



	// ##### ##### ##### ##### ##### ##### #####
	// ## Snapshots
	// ##### ##### ##### ##### ##### ##### #####

	// Fired when the user selects a snapshot from the snapshot list
	public static event Action<GPGSnapshotMetadata> snapshotListUserSelectedSnapshotEvent;

	// Fired when the user requested that a new snapshot should be made
	public static event Action snapshotListUserRequestedNewSnapshotEvent;

	// Fired when the snaphot view is canceled
	public static event Action snapshotListCanceledEvent;

	// Fired when a call to saveSnapshot completes successfully
	public static event Action saveSnapshotSucceededEvent;

	// Fired when a call to saveSnapshot fails
	public static event Action<string> saveSnapshotFailedEvent;

	// Fired when a call to loadSnapshot succeeds. Includes the full snapshot details.
	public static event Action<GPGSnapshot> loadSnapshotSucceededEvent;

	// Fired when a call to loadSnapshot fails
	public static event Action<string> loadSnapshotFailedEvent;



	static GPGManager()
	{
		AbstractManager.initialize( typeof( GPGManager ) );
	}


	private void fireEventWithIdentifierAndError( Action<string,string> theEvent, string json )
	{
		if( theEvent == null )
			return;

		var dict = json.dictionaryFromJson();
		if( dict != null && dict.ContainsKey( "identifier" ) && dict.ContainsKey( "error" ) )
			theEvent( dict["identifier"].ToString(), dict["error"].ToString() );
		else
			Debug.LogError( "json could not be deserialized to an identifier and an error: " + json );
	}


	private void fireEventWithIdentifierAndBool( Action<string,bool> theEvent, string param )
	{
		if( theEvent == null )
			return;

		var parts = param.Split( new char[] { ',' } );
		if( parts.Length == 2 )
			theEvent( parts[0], parts[1] == "1" );
		else
			Debug.LogError( "param could not be deserialized to an identifier and an error: " + param );
	}


	private void userSignedOut( string empty )
	{
		userSignedOutEvent.fire();
	}


	private void reloadDataForKeyFailed( string error )
	{
		reloadDataForKeyFailedEvent.fire( error );
	}


	private void reloadDataForKeySucceeded( string param )
	{
		reloadDataForKeySucceededEvent.fire( param );
	}


	private void licenseCheckFailed( string param )
	{
		licenseCheckFailedEvent.fire();
	}


	private void profileImageLoadedAtPath( string path )
	{
		profileImageLoadedAtPathEvent.fire( path );
	}


	private void finishedSharing( string errorOrNull )
	{
		finishedSharingEvent.fire( errorOrNull );
	}


	private void loadPlayerCompleted( string playerOrError )
	{
		if( loadPlayerCompletedEvent != null )
		{
			// two possibilities here: either JSON or an error string
			if( playerOrError.StartsWith( "{" ) )
				loadPlayerCompletedEvent( Json.decode<GPGPlayerInfo>( playerOrError ), null );
			else
				loadPlayerCompletedEvent( null, playerOrError );
		}
	}


	private void loadCloudDataForKeyFailed( string error )
	{
		loadCloudDataForKeyFailedEvent.fire( error );
	}


	private void loadCloudDataForKeySucceeded( string json )
	{
		var obj = json.dictionaryFromJson();
		loadCloudDataForKeySucceededEvent.fire( int.Parse( obj["key"].ToString() ), obj["data"].ToString() );
	}


	private void updateCloudDataForKeyFailed( string error )
	{
		updateCloudDataForKeyFailedEvent.fire( error );
	}


	private void updateCloudDataForKeySucceeded( string json )
	{
		var obj = json.dictionaryFromJson();
		updateCloudDataForKeySucceededEvent.fire( int.Parse( obj["key"].ToString() ), obj["data"].ToString() );
	}


	private void clearCloudDataForKeyFailed( string error )
	{
		clearCloudDataForKeyFailedEvent.fire( error );
	}


	private void clearCloudDataForKeySucceeded( string param )
	{
		clearCloudDataForKeySucceededEvent.fire( param );
	}


	private void deleteCloudDataForKeyFailed( string error )
	{
		deleteCloudDataForKeyFailedEvent.fire( error );
	}


	private void deleteCloudDataForKeySucceeded( string param )
	{
		deleteCloudDataForKeySucceededEvent.fire( param );
	}


	private void unlockAchievementFailed( string json )
	{
		fireEventWithIdentifierAndError( unlockAchievementFailedEvent, json );
	}


	private void unlockAchievementSucceeded( string param )
	{
		fireEventWithIdentifierAndBool( unlockAchievementSucceededEvent, param );
	}


	private void incrementAchievementFailed( string json )
	{
		fireEventWithIdentifierAndError( incrementAchievementFailedEvent, json );
	}


	private void incrementAchievementSucceeded( string param )
	{
		var parts = param.Split( new char[] { ',' } );
		if( parts.Length == 2 )
			incrementAchievementSucceededEvent.fire( parts[0], parts[1] == "1" );
	}


	private void revealAchievementFailed( string json )
	{
		fireEventWithIdentifierAndError( revealAchievementFailedEvent, json );
	}


	private void revealAchievementSucceeded( string achievementId )
	{
		revealAchievementSucceededEvent.fire( achievementId );
	}


	private void submitScoreFailed( string json )
	{
		fireEventWithIdentifierAndError( submitScoreFailedEvent, json );
	}


	private void submitScoreSucceeded( string json )
	{
		if( submitScoreSucceededEvent != null )
		{
			var dict = json.dictionaryFromJson();
			string leaderboardId = "Unknown";

			if( dict.ContainsKey( "leaderboardId" ) )
				leaderboardId = dict["leaderboardId"].ToString();

			submitScoreSucceededEvent( leaderboardId, dict );
		}
	}


	private void loadScoresFailed( string json )
	{
		fireEventWithIdentifierAndError( loadScoresFailedEvent, json );
	}


	private void loadScoresSucceeded( string json )
	{
		if( loadScoresSucceededEvent != null )
			loadScoresSucceededEvent( Json.decode<List<GPGScore>>( json ) );
	}


	private void loadCurrentPlayerLeaderboardScoreSucceeded( string json )
	{
		if( loadCurrentPlayerLeaderboardScoreSucceededEvent != null )
			loadCurrentPlayerLeaderboardScoreSucceededEvent( Json.decode<GPGScore>( json ) );
	}


	private void loadCurrentPlayerLeaderboardScoreFailed( string json )
	{
		fireEventWithIdentifierAndError( loadCurrentPlayerLeaderboardScoreFailedEvent, json );
	}


	private void authenticationSucceeded( string param )
	{
		authenticationSucceededEvent.fire( param );
	}


	private void authenticationFailed( string error )
	{
		authenticationFailedEvent.fire( error );
	}


	#region Events and Quests

	private void allEventsLoaded( string json )
	{
		if( allEventsLoadedEvent != null )
			allEventsLoadedEvent( Json.decode<List<GPGEvent>>( json ) );
	}


	private void questListLauncherClaimedRewardsForQuestMilestone( string json )
	{
		if( questClaimedRewardsForQuestMilestoneEvent != null )
			questClaimedRewardsForQuestMilestoneEvent( Json.decode<GPGQuestMilestone>( json ) );
	}


	private void questCompleted( string json )
	{
		if( questCompletedEvent != null )
			questCompletedEvent( Json.decode<GPGQuest>( json ) );
	}


	private void questListLauncherAcceptedQuest( string json )
	{
		if( questListLauncherAcceptedQuestEvent != null )
			questListLauncherAcceptedQuestEvent( Json.decode<GPGQuest>( json ) );
	}


	private void allQuestsLoaded( string json )
	{
		if( allQuestsLoadedEvent != null )
			allQuestsLoadedEvent( Json.decode<List<GPGQuest>>( json ) );
	}

	#endregion


	#region Snapshots

	void snapshotListUserSelectedSnapshot( string json )
	{
		if( snapshotListUserSelectedSnapshotEvent != null )
			snapshotListUserSelectedSnapshotEvent( Json.decode<GPGSnapshotMetadata>( json ) );
	}


	void snapshotListUserRequestedNewSnapshot( string empty )
	{
		snapshotListUserRequestedNewSnapshotEvent.fire();
	}


	void snapshotListCanceled( string empty )
	{
		snapshotListCanceledEvent.fire();
	}


	void saveSnapshotSucceeded( string empty )
	{
		saveSnapshotSucceededEvent.fire();
	}


	void saveSnapshotFailed( string error )
	{
		saveSnapshotFailedEvent.fire( error );
	}


	void loadSnapshotSucceeded( string json )
	{
		if( loadSnapshotSucceededEvent != null )
			loadSnapshotSucceededEvent( Json.decode<GPGSnapshot>( json ) );
	}


	void loadSnapshotFailed( string error )
	{
		loadSnapshotFailedEvent.fire( error );
	}

	#endregion

}
#endif
