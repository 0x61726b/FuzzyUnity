using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID
public class PlayGameServices
{
	private static AndroidJavaObject _plugin;


	static PlayGameServices()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.PlayGameServicesPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}


	#region Settings, Auth and Sharing

	// Android only. Returns null on error. Refer to Google's documentation (http://bit.ly/1qabUSU) to learn what you should be passing in as the scope.
	// Note that you must add the android.permission.GET_ACCOUNTS permissions to use this method! Passing in a null or empty scope defaults to Scopes.PLUS_LOGIN.
	public static string getAuthToken( string scope )
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return _plugin.Call<string>( "getAuthToken", scope );
	}


	// Here only for iOS API compatibility. Calls through to setToastSettings.
	public static void setAchievementToastSettings( GPGToastPlacement placement, int offset )
	{
		setToastSettings( placement );
	}


	// Here only for iOS API compatibility. Calls through to setToastSettings.
	public static void setWelcomeBackToastSettings( GPGToastPlacement placement, int offset )
	{
		setToastSettings( placement );
	}


	// Android only. Enables high detail logs
	public static void enableDebugLog( bool shouldEnable )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "enableDebugLog", shouldEnable );
	}


	// Sets the placement of the all toasts
	public static void setToastSettings( GPGToastPlacement placement )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setToastSettings", (int)placement );
	}


	// Android only. Returns null if not invitation is present or the invitationId if there was one
	public static string getLaunchInvitation()
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return _plugin.Call<string>( "getLaunchInvitation" );
	}


	// iOS only. This should be called at application launch. It will attempt to authenticate the user silently. If you need the AppState scope permission
	// (cloud storage requires it) pass true for the requestAppStateScope parameter
	// The clientId and pauseUnityWhileShowingFullScreenViews are iOS only
	public static void init( string clientId, bool requestAppStateScope, bool fetchMetadataAfterAuthentication = true, bool pauseUnityWhileShowingFullScreenViews = true )
	{}


	// Android only. This will attempt to sign in the user with no UI.
	public static void attemptSilentAuthentication()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "attemptSilentAuthentication" );
	}


	// Starts the authentication process which will happen either in the Google+ app, Chrome or Mobile Safari
	public static void authenticate()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "authenticate" );
	}


	// Logs the user out
	public static void signOut()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "signOut" );
	}


	// Checks to see if there is a currently signed in user. Utilizes a terrible hack due to a bug with Play Game Services connection status.
	public static bool isSignedIn()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;

		return _plugin.Call<bool>( "isSignedIn" );
	}


	// Gets the logged in players details
	public static GPGPlayerInfo getLocalPlayerInfo()
	{
		var player = new GPGPlayerInfo();

		if( Application.platform != RuntimePlatform.Android )
			return player;

		var json = _plugin.Call<string>( "getLocalPlayerInfo" );
		return Json.decode<GPGPlayerInfo>( json );
	}


	// Loads player details for the given playerId. Results in the loadPlayerCompletedEvent firing when the operation completes.
	public static void loadPlayer( string playerId )
	{
		if( Application.platform == RuntimePlatform.Android )
			_plugin.Call( "loadPlayer", playerId );
	}


	// Reloads all Play Game Services related metadata
	public static void reloadAchievementAndLeaderboardData()
	{
		if( Application.platform == RuntimePlatform.Android )
			_plugin.Call( "loadBasicModelData" );
	}


	// Android only. Loads a profile image from a Uri. Once loaded the profileImageLoadedAtPathEvent will fire.
	public static void loadProfileImageForUri( string uri )
	{
		if( Application.platform == RuntimePlatform.Android )
			_plugin.Call( "loadProfileImageForUri", uri );
	}


	// Shows a native Google+ share dialog with optional prefilled message (iOS only) and optional url to share
	public static void showShareDialog( string prefillText = null, string urlToShare = null )
	{
		if( Application.platform == RuntimePlatform.Android )
			_plugin.Call( "showShareDialog", prefillText, urlToShare );
	}

	#endregion


	#region Cloud Data

	// Sets and uploads to the cloud the new data
	public static void setStateData( string data, int key )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "setStateData", data, key );
	}


	// Fetches data for the key. Note that you must first either call setStateData or loadCloudDataForKey for this to be valid.
	public static string stateDataForKey( int key )
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return _plugin.Call<string>( "stateDataForKey", key );
	}


	// Downloads cloud data for the given key. The associated loadCloudDataForKeyFailed/Succeeded event will fire when complete.
	public static void loadCloudDataForKey( int key, bool useRemoteDataForConflictResolution = true )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadCloudDataForKey", key, useRemoteDataForConflictResolution );
	}


	//
	public static void deleteCloudDataForKey( int key, bool useRemoteDataForConflictResolution = true )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "deleteCloudDataForKey", key, useRemoteDataForConflictResolution );
	}


	// Does not exist on Android. It is here for API compatibility only.
	public static void clearCloudDataForKey( int key, bool useRemoteDataForConflictResolution = true )
	{}


	// Does not exist on Android. Calls through to loadCloudDataForKey for compatibility with iOS
	public static void updateCloudDataForKey( int key, bool useRemoteDataForConflictResolution = true )
	{
		loadCloudDataForKey( key, useRemoteDataForConflictResolution );
	}

	#endregion


	#region Achievements

	// Shows the achievements screen
	public static void showAchievements()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showAchievements" );
	}


	// Reveals the achievement if it was previously hidden
	public static void revealAchievement( string achievementId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "revealAchievement", achievementId );
	}


	// Unlocks the achievement. Note that showsCompletionNotification does nothing on Android.
	public static void unlockAchievement( string achievementId, bool showsCompletionNotification = true )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "unlockAchievement", achievementId, showsCompletionNotification );
	}


	// Increments the achievement. Only works on achievements setup as incremental in the Google Developer Console.
	// Fires the incrementAchievementFailed/Succeeded event when complete.
	public static void incrementAchievement( string achievementId, int numSteps )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "incrementAchievement", achievementId, numSteps );
	}


	// Gets the achievement metadata
	public static List<GPGAchievementMetadata> getAllAchievementMetadata()
	{
		if( Application.platform != RuntimePlatform.Android )
			return new List<GPGAchievementMetadata>();

		var json = _plugin.Call<string>( "getAllAchievementMetadata" );
		return Json.decode<List<GPGAchievementMetadata>>( json );
	}

	#endregion


	#region Leaderboards

	// Shows the requested leaderboard. timeScope is no supported on either platform with the current SDK
	public static void showLeaderboard( string leaderboardId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showLeaderboard", leaderboardId );
	}


	// Shows a list of all learderboards
	public static void showLeaderboards()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showLeaderboards" );
	}


	// Submits a score for the given leaderboard. Fires the submitScoreFailed/Succeeded event when complete.
	public static void submitScore( string leaderboardId, long score )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "submitScore", leaderboardId, score );
	}


	// Loads scores for the given leaderboard. Fires the loadScoresFailed/Succeeded event when complete.
	public static void loadScoresForLeaderboard( string leaderboardId, GPGLeaderboardTimeScope timeScope, bool isSocial, bool personalWindow )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadScoresForLeaderboard", leaderboardId, (int)timeScope, isSocial, personalWindow );
	}


	// Loads the current players score for the given leaderboard. Fires the loadCurrentPlayerLeaderboardScoreSucceeded/FailedEvent when complete.
	public static void loadCurrentPlayerLeaderboardScore( string leaderboardId, GPGLeaderboardTimeScope timeScope )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadCurrentPlayerLeaderboardScore", leaderboardId, (int)timeScope );
	}


	// Gets all the leaderboards metadata
	public static List<GPGLeaderboardMetadata> getAllLeaderboardMetadata()
	{
		if( Application.platform != RuntimePlatform.Android )
			return new List<GPGLeaderboardMetadata>();

		var json = _plugin.Call<string>( "getAllLeaderboardMetadata" );
		return Json.decode<List<GPGLeaderboardMetadata>>( json );
	}

	#endregion


	#region Events and Quests

	// Sends a request to load all events for this app. The allEventsLoadedEvent is fired when the request completes.
	public static void loadAllEvents()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadAllEvents" );
	}


	// Increments an event 1 or more steps
	public static void incrementEvent( string eventId, int steps )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		if( steps <= 0 )
			return;

		_plugin.Call( "incrementEvent", eventId, steps );
	}


	// Android only. Sends a request to load all quests for this app. The allQuestsLoadedEvent is fired when the request completes.
	public static void loadAllQuests()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadAllQuests" );
	}


	// Android only. Show the default popup for certain quest states. Popups are only supported for quest in either the STATE_ACCEPTED or STATE_COMPLETED state.
	// If the quest is in another state, no popup will be shown
	public static void showStateChangedPopup( string questId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showStateChangedPopup", questId );
	}


	// Shows the quest list with all the quests currently available to this app
	public static void showQuestList()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showQuestList" );
	}

	#endregion


	#region Snapshots

	// Shows the snapshot list view. Results in one of the following events firing: snapshotListUserSelectedSnapshotEvent,
	// snapshotListUserRequestedNewSnapshotEvent or snapshotListCanceledEvent.
	public static void showSnapshotList( int maxSavedGamesToShow, string title, bool allowAddButton, bool allowDelete )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showSnapshotList", maxSavedGamesToShow, title, allowAddButton, allowDelete );
	}


	// Saves a snapshot optionally creating a new one if createIfMissing is true. Results in the saveSnapshotSucceeded/FailedEvent firing.
	public static void saveSnapshot( string snapshotName, bool createIfMissing, byte[] data, string description, GPGSnapshotConflictPolicy conflictPolicy = GPGSnapshotConflictPolicy.MostRecentlyModified )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "saveSnapshot", snapshotName, createIfMissing, data, description, (int)conflictPolicy );
	}


	// Loads a snapshot. Results in the loadSnapshotSucceeded/FailedEvent firing.
	public static void loadSnapshot( string snapshotName )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "loadSnapshot", snapshotName );
	}


	// Deletes a snapshot
	public static void deleteSnapshot( string snapshotName )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "deleteSnapshot", snapshotName );
	}

	#endregion

}
#endif
