using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;


#if UNITY_ANDROID
public enum GPGRoomUpdateStatus
{
	STATUS_OK = 0,
	STATUS_CLIENT_RECONNECT_REQUIRED = 2,
	STATUS_REAL_TIME_CONNECTION_FAILED = 7000,
	STATUS_MULTIPLAYER_DISABLED = 6003,
	STATUS_INTERNAL_ERROR = 1,
	STATUS_IOS_DOESNT_HAVE_THIS = 666
}

public class GPGMultiplayerManager : AbstractManager
{
	// Fired when a new invitation is received. Includes the invitationId which can be passed to the joinRoomWithInvitation method.
	public static event Action<string> onInvitationReceivedEvent;

	// Fired when a previously received invitation has been removed from the local device.
	// For example, this might occur if the inviting player leaves the match.
	public static event Action<string> onInvitationRemovedEvent;

	// Android only. Fired when the waiting room completes. The bool indicates if the waiting room succeeded (a room was joined)
	public static event Action<bool> onWaitingRoomCompletedEvent;

	// Android only. Fired when the invitation inbox screen completes. The bool indicates if a room was joined.
	public static event Action<bool> onInvitationInboxCompletedEvent;

	// Android only. Fired when the player selector screen completes. The bool indicates if a room was created.
	public static event Action<bool> onInvitePlayersCompletedEvent;


	// Room Events (RoomUpdateListener on the native side)

	// Android only. Fired when the client attempts to join a real-time room. Calling showWaitingRoom is allowed when this event fires.
	public static event Action<GPGRoom,GPGRoomUpdateStatus> onJoinedRoomEvent;

	// Fired when the client attempts to leave the real-time room
	public static event Action onLeftRoomEvent;

	// Fired when all the participants in a real-time room are fully connected
	public static event Action<GPGRoom,GPGRoomUpdateStatus> onRoomConnectedEvent;

	// Android only. Fired when when the client attempts to create a real-time room. Calling showWaitingRoom is allowed when this event fires.
	public static event Action<GPGRoom,GPGRoomUpdateStatus> onRoomCreatedEvent;


	// Realtime Message Events (RealTimeMessageReceivedListener on the native side)

	// Fired when the client receives reliable or unreliable message for a room. It is very important to understand that this
	// event can fire multiple times per frame and on any thread. The easist way to handle this in your game is to store the information
	// and process it in your Update/FixedUpdate method
	public static event Action<string,byte[]> onRealTimeMessageReceivedEvent;


	// Room Status Update Events (RoomStatusUpdateListener on the native side)

	// Fired when the client is connected to the connected set in a room
	public static event Action onConnectedToRoomEvent;

	// Fired when the client is disconnected from the connected set in a room
	public static event Action onDisconnectedFromRoomEvent;

	// Fired when the client is successfully connected to a peer participant. Includes the participantId.
	public static event Action<string> onP2PConnectedEvent;

	// Fired when the client gets disconnected from a peer participant. Includes the participantId.
	public static event Action<string> onP2PDisconnectedEvent;

	// Fired when a peer declines the invitation to a room. Includes the participantId.
	public static event Action<string> onPeerDeclinedEvent;

	// Fired when a peer is invited to a room. Includes the participantId.
	public static event Action<string> onPeerInvitedToRoomEvent;

	// Fired when a peer participant joins a room. Includes the participantId.
	public static event Action<string> onPeerJoinedEvent;

	// Fired when a peer participant leaves a room. Includes the participantId.
	public static event Action<string> onPeerLeftEvent;

	// Fired when a peer participant is connected to a room. Includes the participantId.
	public static event Action<string> onPeerConnectedEvent;

	// Fired when a peer participant is disconnected from a room. Includes the participantId.
	public static event Action<string> onPeerDisconnectedEvent;

	// Fired when the server has started the process of auto-matching
	public static event Action<GPGRoom> onRoomAutoMatchingEvent;

	// Fired when one or more participants have joined the room and have started the process of establishing peer connections.
	public static event Action<GPGRoom> onRoomConnectingEvent;


	static GPGMultiplayerManager()
	{
		AbstractManager.initialize( typeof( GPGMultiplayerManager ) );
	}


	private void onInvitationReceived( string invitationId )
	{
		onInvitationReceivedEvent.fire( invitationId );
	}


	private void onInvitationRemoved( string invitationId )
	{
		onInvitationRemovedEvent.fire( invitationId );
	}


	private void onWaitingRoomCompleted( string success )
	{
		onWaitingRoomCompletedEvent.fire( success == "1" );
	}


	private void onInvitationInboxCompleted( string success )
	{
		onInvitationInboxCompletedEvent.fire( success == "1" );
	}


	private void onInvitePlayersCompleted( string success )
	{
		onInvitePlayersCompletedEvent.fire( success == "1" );
	}


	private void onJoinedRoom( string json )
	{
		var info = Json.decode<GPGRoomUpdateInfo>( json );
		onJoinedRoomEvent.fire( info.room, info.status );
	}


	private void onLeftRoom( string empty )
	{
		onLeftRoomEvent.fire();
	}


	private void onRoomConnected( string json )
	{
		var info = Json.decode<GPGRoomUpdateInfo>( json );
		onRoomConnectedEvent.fire( info.room, info.status );
	}


	private void onRoomCreated( string json )
	{
		var info = Json.decode<GPGRoomUpdateInfo>( json );
		onRoomCreatedEvent.fire( info.room, info.status );
	}


	public static void onRealTimeMessageReceived( string senderParticipantId, byte[] message )
	{
		if( onRealTimeMessageReceivedEvent != null )
			onRealTimeMessageReceivedEvent( senderParticipantId, message );
	}


	private void onConnectedToRoom( string empty )
	{
		onConnectedToRoomEvent.fire();
	}


	private void onDisconnectedFromRoom( string empty )
	{
		onDisconnectedFromRoomEvent.fire();
	}


	private void onP2PConnected( string participantId )
	{
		onP2PConnectedEvent.fire( participantId );
	}


	private void onP2PDisconnected( string participantId )
	{
		onP2PDisconnectedEvent.fire( participantId );
	}


	private void onPeerDeclined( string id )
	{
		onPeerDeclinedEvent.fire( id );
	}


	private void onPeerInvitedToRoom( string id )
	{
		onPeerInvitedToRoomEvent.fire( id );
	}


	private void onPeerJoined( string id )
	{
		onPeerJoinedEvent.fire( id );
	}


	private void onPeerLeft( string id )
	{
		onPeerLeftEvent.fire( id );
	}


	private void onPeerConnected( string id )
	{
		onPeerConnectedEvent.fire( id );
	}


	private void onPeerDisconnected( string id )
	{
		onPeerDisconnectedEvent.fire( id );
	}


	private void onRoomAutoMatching( string json )
	{
		onRoomAutoMatchingEvent.fire( Json.decode<GPGRoom>( json ) );
	}


	private void onRoomConnecting( string json )
	{
		onRoomConnectingEvent.fire( Json.decode<GPGRoom>( json ) );
	}


	private class GPGRoomUpdateInfo
	{
		#pragma warning disable 0649
		public GPGRoom room { get; set; }
		public int statusCode { get; set; }
		public GPGRoomUpdateStatus status
		{
			get
			{
				return (GPGRoomUpdateStatus)statusCode;
			}
		}
		#pragma warning restore 0649

		public GPGRoomUpdateInfo()
		{}
	}
}
#endif