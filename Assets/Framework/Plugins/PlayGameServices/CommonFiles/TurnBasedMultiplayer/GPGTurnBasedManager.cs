using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID
public class GPGTurnBasedManager : AbstractManager
{
	// Fired when a new invitation is received. Includes the invitationId which can be passed to the joinRoomWithInvitation method.
	public static event Action<string> onInvitationReceivedEvent;

	// Fired when a previously received invitation has been removed from the local device.
	// For example, this might occur if the inviting player leaves the match.
	public static event Action<string> onInvitationRemovedEvent;

	// Fired anytime a match changes. Note that this can fire at any time and it could be any match so always be sure
	// to check the match status and userMatchStatus before acting on the event.
	public static event Action<GPGTurnBasedMatch> matchChangedEvent;

	// Fired when any action related to match setup fails. This could be when creating a match programmatically, picking
	// a match from the inbox, accepting an invite or creating a match via the player selector.
	public static event Action<string> matchFailedEvent;

	// iOS only. Fired when a match ends usually as a result of a push notification.
	public static event Action<GPGTurnBasedMatch> matchEndedEvent;

	// Fired when the user cancels the player selector without taking any action
	public static event Action playerSelectorCanceledEvent;

	// Fired when matches have loaded successfully. Includes a bool indicating success/failure, an error string if an error occurred and all the match data if successful.
	public static event Action<bool,string,List<GPGTurnBasedMatch>> loadMatchesCompletedEvent;

	// Fired when a call to takeTurn completes. The bool indicates success/failure and the string will be the error message
	// if the operation failed.
	public static event Action<bool,string> takeTurnCompletedEvent;

	// Fired when a call to finishMatch completes. The bool indicates success/failure and the string will be the error message
	// if the operation failed.
	public static event Action<bool,string> finishMatchCompletedEvent;

	// Fired when a call to dismissMatch completes. The bool indicates success/failure and the string will be the error message
	// if the operation failed.
	public static event Action<bool,string> dismissMatchCompletedEvent;

	// Fired when a call to leaveDuringTurn completes. The bool indicates success/failure and the string will be the error message
	// if the operation failed.
	public static event Action<bool,string> leaveDuringTurnCompletedEvent;

	// Fired when a call to leaveOutOfTurn completes. The bool indicates success/failure and the string will be the error message
	// if the operation failed.
	public static event Action<bool,string> leaveOutOfTurnCompletedEvent;

	// Fired when an invitation is received. This can occur anytime on iOS. On Android, it will only fire in response to
	// a call to checkForInvitesAndMatches or showing the users inbox
	public static event Action<GPGTurnBasedInvitation> invitationReceivedEvent;



	static GPGTurnBasedManager()
	{
		AbstractManager.initialize( typeof( GPGTurnBasedManager ) );
	}




	private void onInvitationReceived( string invitationId )
	{
		onInvitationReceivedEvent.fire( invitationId );
	}


	private void onInvitationRemoved( string invitationId )
	{
		onInvitationRemovedEvent.fire( invitationId );
	}


	void matchChanged( string json )
	{
		if( matchChangedEvent != null )
			matchChangedEvent( Json.decode<GPGTurnBasedMatch>( json ) );
	}


	void matchFailed( string error )
	{
		matchFailedEvent.fire( error );
	}


	void matchEnded( string json )
	{
		if( matchEndedEvent != null )
			matchEndedEvent( Json.decode<GPGTurnBasedMatch>( json ) );
	}


	void playerSelectorCanceled( string empty )
	{
		playerSelectorCanceledEvent.fire();
	}


	void loadMatchesFailed( string error )
	{
		loadMatchesCompletedEvent( false, error, null );
	}


	void loadMatchesSucceeded( string json )
	{
		if( loadMatchesCompletedEvent != null )
			loadMatchesCompletedEvent.fire( true, null, Json.decode<List<GPGTurnBasedMatch>>( json ) );
	}


	void takeTurnFailed( string error )
	{
		takeTurnCompletedEvent.fire( false, error );
	}


	void takeTurnSucceeded( string empty )
	{
		takeTurnCompletedEvent.fire( true, null );
	}


	void finishMatchFailed( string error )
	{
		finishMatchCompletedEvent.fire( false, error );
	}


	void finishMatchSucceeded( string empty )
	{
		finishMatchCompletedEvent.fire( true, null );
	}


	void dismissMatchFailed( string error )
	{
		dismissMatchCompletedEvent.fire( false, error );
	}


	void dismissMatchSucceeded( string empty )
	{
		dismissMatchCompletedEvent.fire( true, null );
	}


	void leaveDuringTurnFailed( string error )
	{
		leaveDuringTurnCompletedEvent.fire( false, error );
	}


	void leaveDuringTurnSucceeded( string empty )
	{
		leaveDuringTurnCompletedEvent.fire( true, null );
	}


	void leaveOutOfTurnFailed( string error )
	{
		leaveOutOfTurnCompletedEvent.fire( false, error );
	}


	void leaveOutOfTurnSucceeded( string empty )
	{
		leaveOutOfTurnCompletedEvent.fire( true, null );
	}


	void invitationReceived( string json )
	{
		if( invitationReceivedEvent != null )
			invitationReceivedEvent( Json.decode<GPGTurnBasedInvitation>( json ) );
	}

}
#endif
