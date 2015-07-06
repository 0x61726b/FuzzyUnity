using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Prime31;



#if UNITY_IPHONE
public class GameCenterManager : AbstractManager
{
	// Player events
	// Fired when retrieving player data (friends) fails
	public static event Action<string> loadPlayerDataFailedEvent;

	// Fired when player data is loaded after requesting friends
	public static event Action<List<GameCenterPlayer>> playerDataLoadedEvent;

	// Fired when a player is logged in
	public static event Action playerAuthenticatedEvent;

	// Fired when a player fails to login
	public static event Action<string> playerFailedToAuthenticateEvent;

	// Fired when a player logs out
	public static event Action playerLoggedOutEvent;

	// Fired when the profile image is loaded for the player and includes the full path to the image
	public static event Action<string> profilePhotoLoadedEvent;

	// Fired when the profile image fails to load
	public static event Action<string> profilePhotoFailedEvent;

	// Fired when a call to generateIdentityVerificationSignature succeeds. Includes the publicKeyUrl, timestamp, signature and salt (base64 encoded).
	public static event Action<Dictionary<string,string>> generateIdentityVerificationSignatureSucceededEvent;

	// Fired when a call to generateIdentityVerificationSignature fails.
	public static event Action<string> generateIdentityVerificationSignatureFailedEvent;


	// Leaderboard events
	// Fired when loading leaderboard category data fails
	public static event Action<string> loadCategoryTitlesFailedEvent;

	// Fired when loading leaderboard category data completes
	public static event Action<List<GameCenterLeaderboard>> categoriesLoadedEvent;

	// Fired when reporting a score fails
	public static event Action<string> reportScoreFailedEvent;

	// Fired when reporting a score finishes successfully
	public static event Action<string> reportScoreFinishedEvent;

	// Fired when retrieving scores fails
	public static event Action<string> retrieveScoresFailedEvent;

	// Fired when retrieving scores completes successfully
	public static event Action<GameCenterRetrieveScoresResult> scoresLoadedEvent;

	// Fired when retrieving scores for a playerId fails
	public static event Action<string> retrieveScoresForPlayerIdFailedEvent;

	// Fired when retrieving scores for a playerId completes successfully
	public static event Action<GameCenterRetrieveScoresResult> scoresForPlayerIdLoadedEvent;

	// Achievement events
	// Fired when reporting an achievement fails
	public static event Action<string> reportAchievementFailedEvent;

	// Fired when reporting an achievement completes successfully
	public static event Action<string> reportAchievementFinishedEvent;

	// Fired when loading achievements fails
	public static event Action<string> loadAchievementsFailedEvent;

	// Fired when loading achievements completes successfully
	public static event Action<List<GameCenterAchievement>> achievementsLoadedEvent;

	// Fired when resetting achievements fails
	public static event Action<string> resetAchievementsFailedEvent;

	// Fired when resetting achievements completes successfully
	public static event Action resetAchievementsFinishedEvent;

	// Fired when loading achievement metadata fails
	public static event Action<string> retrieveAchievementMetadataFailedEvent;

	// Fired when loading achievement metadata completes successfully
	public static event Action<List<GameCenterAchievementMetadata>> achievementMetadataLoadedEvent;


	// Challenge events
	// Fired when a call to selectChallengeablePlayerIDsForAchievement fails
	public static event Action<string> selectChallengeablePlayerIDsDidFailEvent;

	// Fired when a call to selectChallengeablePlayerIDsForAchievement completes
	public static event Action<List<object>> selectChallengeablePlayerIDsDidFinishEvent;

	// Fired when the user taps a challenge notification banner or the "Play Now" button for a challenge inside Game Center
	public static event Action<GameCenterChallenge> localPlayerDidSelectChallengeEvent;

	// Fired when the local player has completed one of their challenges, triggered by a push notification from the server
	public static event Action<GameCenterChallenge> localPlayerDidCompleteChallengeEvent;

	// Fired when a non-local player has completed a challenge issued by the local player. Triggered by a push notification from the server.
	public static event Action<GameCenterChallenge> remotePlayerDidCompleteChallengeEvent;

	// Fired when challenges load
	public static event Action<List<GameCenterChallenge>> challengesLoadedEvent;

	// Fired when challenges fail to laod
	public static event Action<string> challengesFailedToLoadEvent;

	// iOS 7+. Fired when a challenge is successfully sent. Includes an array of all the playerIds that the challenge was sent to.
	public static event Action<List<object>> challengeIssuedSuccessfullyEvent;

	// iOS 7+. Fired when the challenge composer completes and the user did not send the challenge to anyone.
	public static event Action challengeNotIssuedEvent;



	#region Deprecated Events

	#pragma warning disable 0067

	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> loadPlayerDataFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterPlayer>> playerDataLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action playerAuthenticated;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> playerFailedToAuthenticate;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action playerLoggedOut;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> profilePhotoLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> profilePhotoFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> loadCategoryTitlesFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterLeaderboard>> categoriesLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> reportScoreFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> reportScoreFinished;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> retrieveScoresFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterScore>> scoresLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> retrieveScoresForPlayerIdFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterScore>> scoresForPlayerIdLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> reportAchievementFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> reportAchievementFinished;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> loadAchievementsFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterAchievement>> achievementsLoaded;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> resetAchievementsFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action resetAchievementsFinished;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<string> retrieveAchievementMetadataFailed;
	[System.Obsolete( "All events have been renamed to match the style of all of our other plugins. Append 'Event' to the event name for the new name." )]
	public static event Action<List<GameCenterAchievementMetadata>> achievementMetadataLoaded;

	#pragma warning restore 0067

	#endregion





    static GameCenterManager()
    {
		AbstractManager.initialize( typeof( GameCenterManager ) );
    }


	#region Player callbacks

	public void loadPlayerDataDidFail( string error )
	{
		if( loadPlayerDataFailedEvent != null )
			loadPlayerDataFailedEvent( error );
	}


	public void loadPlayerDataDidLoad( string jsonFriendList )
	{
		List<GameCenterPlayer> list = GameCenterPlayer.fromJSON( jsonFriendList );

		if( playerDataLoadedEvent != null )
			playerDataLoadedEvent( list );
	}


	public void playerDidLogOut()
	{
		if( playerLoggedOutEvent != null )
			playerLoggedOutEvent();
	}


	public void playerDidAuthenticate( string playerId )
	{
		if( playerAuthenticatedEvent != null )
			playerAuthenticatedEvent();
	}


	public void playerAuthenticationFailed( string error )
	{
		if( playerFailedToAuthenticateEvent != null )
			playerFailedToAuthenticateEvent( error );
	}


	public void loadProfilePhotoDidLoad( string path )
	{
		if( profilePhotoLoadedEvent != null )
			profilePhotoLoadedEvent( path );
	}


	public void loadProfilePhotoDidFail( string error )
	{
		if( profilePhotoFailedEvent != null )
			profilePhotoFailedEvent( error );
	}


	public void generateIdentityVerificationSignatureSucceeded( string json )
	{
		if( generateIdentityVerificationSignatureSucceededEvent != null )
			generateIdentityVerificationSignatureSucceededEvent( Json.decode<Dictionary<string,string>>( json ) );
	}


	public void generateIdentityVerificationSignatureFailed( string error )
	{
		if( generateIdentityVerificationSignatureFailedEvent != null )
			generateIdentityVerificationSignatureFailedEvent( error );
	}

	#endregion;


	#region Leaderboard callbacks

	public void loadCategoryTitlesDidFail( string error )
	{
		if( loadCategoryTitlesFailedEvent != null )
			loadCategoryTitlesFailedEvent( error );
	}


	public void categoriesDidLoad( string jsonCategoryList )
	{
		List<GameCenterLeaderboard> list = GameCenterLeaderboard.fromJSON( jsonCategoryList );

		if( categoriesLoadedEvent != null )
			categoriesLoadedEvent( list );
	}


	public void reportScoreDidFail( string error )
	{
		if( reportScoreFailedEvent != null )
			reportScoreFailedEvent( error );
	}


	public void reportScoreDidFinish( string category )
	{
		if( reportScoreFinishedEvent != null )
			reportScoreFinishedEvent( category );
	}


	public void retrieveScoresDidFail( string category )
	{
		if( retrieveScoresFailedEvent != null )
			retrieveScoresFailedEvent( category );
	}


	public void retrieveScoresDidLoad( string json )
	{
		Debug.Log( json );
		if( scoresLoadedEvent != null )
			scoresLoadedEvent( Json.decode<GameCenterRetrieveScoresResult>( json ) );
	}


	public void retrieveScoresForPlayerIdDidFail( string error )
	{
		if( retrieveScoresForPlayerIdFailedEvent != null )
			retrieveScoresForPlayerIdFailedEvent( error );
	}


	public void retrieveScoresForPlayerIdDidLoad( string json )
	{
		if( scoresForPlayerIdLoadedEvent != null )
			scoresForPlayerIdLoadedEvent( Json.decode<GameCenterRetrieveScoresResult>( json ) );
	}

	#endregion;


	#region Achievements

	public void reportAchievementDidFail( string error )
	{
		if( reportAchievementFailedEvent != null )
			reportAchievementFailedEvent( error );
	}


	public void reportAchievementDidFinish( string identifier )
	{
		if( reportAchievementFinishedEvent != null )
			reportAchievementFinishedEvent( identifier );
	}


	public void loadAchievementsDidFail( string error )
	{
		if( loadAchievementsFailedEvent != null )
			loadAchievementsFailedEvent( error );
	}


	public void achievementsDidLoad( string jsonAchievmentList )
	{
		List<GameCenterAchievement> list = GameCenterAchievement.fromJSON( jsonAchievmentList );

		if( achievementsLoadedEvent != null )
			achievementsLoadedEvent( list );
	}


	public void resetAchievementsDidFail( string error )
	{
		if( resetAchievementsFailedEvent != null )
			resetAchievementsFailedEvent( error );
	}


	public void resetAchievementsDidFinish( string emptyString )
	{
		if( resetAchievementsFinishedEvent != null )
			resetAchievementsFinishedEvent();
	}


	public void retrieveAchievementsMetaDataDidFail( string error )
	{
		if( retrieveAchievementMetadataFailedEvent != null )
			retrieveAchievementMetadataFailedEvent( error );
	}


	public void achievementMetadataDidLoad( string jsonAchievementDescriptionList )
	{
		List<GameCenterAchievementMetadata> list = GameCenterAchievementMetadata.fromJSON( jsonAchievementDescriptionList );

		if( achievementMetadataLoadedEvent != null )
			achievementMetadataLoadedEvent( list );
	}

	#endregion;


	#region Challenges

	public void selectChallengeablePlayerIDsDidFail( string error )
	{
		if( selectChallengeablePlayerIDsDidFailEvent != null )
			selectChallengeablePlayerIDsDidFailEvent( error );
	}


	public void selectChallengeablePlayerIDsDidFinish( string json )
	{
		if( selectChallengeablePlayerIDsDidFinishEvent != null )
			selectChallengeablePlayerIDsDidFinishEvent( json.listFromJson() );
	}


	public void localPlayerDidSelectChallenge( string json )
	{
		if( localPlayerDidSelectChallengeEvent != null )
			localPlayerDidSelectChallengeEvent( new GameCenterChallenge( json.dictionaryFromJson() ) );
	}


	public void localPlayerDidCompleteChallenge( string json )
	{
		if( localPlayerDidCompleteChallengeEvent != null )
			localPlayerDidCompleteChallengeEvent( new GameCenterChallenge( json.dictionaryFromJson() ) );
	}


	public void remotePlayerDidCompleteChallenge( string json )
	{
		if( remotePlayerDidCompleteChallengeEvent != null )
			remotePlayerDidCompleteChallengeEvent( new GameCenterChallenge( json.dictionaryFromJson() ) );
	}


	public void challengesLoaded( string json )
	{
		if( challengesLoadedEvent != null )
			challengesLoadedEvent( GameCenterChallenge.fromJson( json ) );
	}


	public void challengesFailedToLoad( string error )
	{
		if( challengesFailedToLoadEvent != null )
			challengesFailedToLoadEvent( error );
	}


	public void challengeIssuedSuccessfully( string json )
	{
		if( challengeIssuedSuccessfullyEvent != null )
			challengeIssuedSuccessfullyEvent( json.listFromJson() );
	}


	public void challengeNotIssued( string empty )
	{
		if( challengeNotIssuedEvent != null )
			challengeNotIssuedEvent();
	}

	#endregion

}
#endif