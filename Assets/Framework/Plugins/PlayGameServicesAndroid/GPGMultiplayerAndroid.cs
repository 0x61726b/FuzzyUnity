using UnityEngine;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID
public class GPGMultiplayer
{
	private static AndroidJavaObject _plugin;


	static GPGMultiplayer()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		using( var pluginClass = new AndroidJavaClass( "com.prime31.PlayGameServicesPlugin" ) )
		{
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "realtimeMultiplayerInstance" );
			_plugin.Call( "setRealtimeMessageListener", new GPGMultiplayer.RealTimeMessageReceivedListener() );
		}
	}


	// iOS only. Registers the push device token with Google. isProductionEnvironment controls whether Apple's production
	// or sandbox servers will be used to send the push notifications.
	public static void registerDeviceToken( byte[] deviceToken, bool isProductionEnvironment )
	{}


	// Shows the invitation inbox with all the game invitations the current user has available. If a user selects one of the invitations
	// the room is joined automatically.
	public static void showInvitationInbox()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showInvitationInbox" );
	}


	// iOS only. On Android this forwards to createRoomProgrammatically. Creates a room with the provided auto-match criteria and shows the Google provided UI.
	// Exclusive bitmasks are for the automatching request. The logical AND of each pairing of automatching requests must equal zero for auto-match.
	// If there are no exclusivity requirements for the game, this value should just be set to 0
	// When variant is set, only other players with the same variant will be matched up.
	public static void startQuickMatch( int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitmask = 0, int variant = 1 )
	{
		createRoomProgrammatically( minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitmask, variant );
	}


	// Creates a room with the provided auto-match criteria.
	// Exclusive bitmasks are for the automatching request. The logical AND of each pairing of automatching requests must equal zero for auto-match.
	// If there are no exclusivity requirements for the game, this value should just be set to 0
	// When variant is set, only other players with the same variant will be matched up.
	public static void createRoomProgrammatically( int minAutoMatchPlayers, int maxAutoMatchPlayers, long exclusiveBitmask = 0, int variant = 1 )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "createRoomProgrammatically", minAutoMatchPlayers, maxAutoMatchPlayers, exclusiveBitmask, variant );
	}


	// Shows the player selector. minPlayers and maxPlayers does NOT include the current player.
	// When variant is set, only other players with the same variant will be matched up.
	public static void showPlayerSelector( int minPlayers, int maxPlayers, int variant = 1 )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showPlayerSelector", minPlayers, maxPlayers, variant );
	}


	// Joins a room with the given invitationId which is aquired when an invite is received and the onInvitationReceivedEvent fires
	public static void joinRoomWithInvitation( string invitationId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "joinRoomWithInvitation", invitationId );
	}


	// Android only. Shows the waiting room UI. minParticipantsToStart is the minimum number of participants that must be connected to the room
	// (including the current player) for the "Start playing" menu item to become enabled. Use int.MaxValue to require all players
	// to be connected.
	public static void showWaitingRoom( int minParticipantsToStart )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "showWaitingRoom", minParticipantsToStart );
	}


	// Leaves the current room and exits the real-time multiplayer match
	public static void leaveRoom()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "leaveRoom" );
	}


	// Gets the current room
	public static GPGRoom getRoom()
	{
		if( Application.platform != RuntimePlatform.Android )
			return new GPGRoom();

		var json = _plugin.Call<string>( "getRoom" );
		return Json.decode<GPGRoom>( json );
	}


	// Gets all the participants in the current room
	public static List<GPGMultiplayerParticipant> getParticipants()
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		var json = _plugin.Call<string>( "getParticipants" );
		return Json.decode<List<GPGMultiplayerParticipant>>( json );
	}


	// Gets the participantId of the current player
	public static string getCurrentPlayerParticipantId()
	{
		if( Application.platform != RuntimePlatform.Android )
			return null;

		return _plugin.Call<string>( "getCurrentPlayerParticipantId" );
	}


	#region Sending Realtime Messages

	// Send a message to a participant in a real-time room reliably
	public static void sendReliableRealtimeMessage( string participantId, byte[] message )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "sendReliableRealtimeMessage", participantId, message );
	}


	// Send a message to all participants in a real-time room reliably
	public static void sendReliableRealtimeMessageToAll( byte[] message )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "sendReliableRealtimeMessageToAll", message );
	}


	// Send a message to a participant in a real-time room unreliably
	public static void sendUnreliableRealtimeMessage( string participantId, byte[] message )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "sendUnreliableRealtimeMessage", participantId, message );
	}


	// Send a message to all participants in a real-time room unreliably
	public static void sendUnreliableRealtimeMessageToAll( byte[] message )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "sendUnreliableRealtimeMessageToAll", message );
	}

	#endregion


	#region AndroidJavaProxy

	private class RealTimeMessageReceivedListener : AndroidJavaProxy
	{
		public RealTimeMessageReceivedListener() : base( "com.prime31.IRealTimeMessageReceivedListener" )
		{}


		// this is currently not used due to it being slower dealing with base64 encode/decode
		public void onMessageReceived( string senderParticipantId, string messageData )
		{
			var bytes = System.Convert.FromBase64String( messageData );
			GPGMultiplayerManager.onRealTimeMessageReceived( senderParticipantId, bytes );
		}


		public void onRawMessageReceived( AndroidJavaObject senderParticipantId, AndroidJavaObject messageData )
		{
			var partcipantId = senderParticipantId.Call<string>( "toString" );
			var bytes = AndroidJNI.FromByteArray( messageData.GetRawObject() );
			GPGMultiplayerManager.onRealTimeMessageReceived( partcipantId, bytes );
		}


		public override AndroidJavaObject Invoke( string methodName, AndroidJavaObject[] args )
		{
			// we have to override invoke when dealing with the onRawMessageReceived method due to Unity's implementation being buggy.
			// this will be faster as well since it is just a single string compare vs using reflection to invoke the method.
			if( methodName == "onRawMessageReceived" )
			{
				onRawMessageReceived( args[0], args[1] );
				return null;
			}

			return base.Invoke( methodName, args );
		}


		public string toString()
		{
			return "RealTimeMessageReceivedListener class instance from Unity";
		}
	}

	#endregion

}
#endif