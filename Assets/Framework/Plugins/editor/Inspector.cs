using UnityEditor;

[CustomEditor(typeof(ELANNotification))] 
public class Inspector : Editor {
	
	ELANNotification notification; // NEW No need for a private variable
	
	public override void OnInspectorGUI() {
		
		notification = (ELANNotification) target;
			
		notification.sendOnStart     = EditorGUILayout.Toggle("Send on start", notification.sendOnStart);
		notification.fullClassName  = EditorGUILayout.TextField("Full class name", notification.fullClassName );
		notification.title 	 		= EditorGUILayout.TextField("Insert title", notification.title );
		notification.message  		= EditorGUILayout.TextField("Insert message",notification.message );
		
		// NEW added repetitionTypeTime
		// NEW no need to use intParse for notification.delay
		
		notification.delayTypeTime = ( EnumTimeType )EditorGUILayout.EnumPopup("Type time (delay):",  notification.delayTypeTime);
		notification.delay			= EditorGUILayout.IntSlider("Delay", notification.delay, 0, 60);
		
		notification.repetitionTypeTime = ( EnumTimeType )EditorGUILayout.EnumPopup("Type time (rep):",  notification.repetitionTypeTime);
		notification.repetition		= EditorGUILayout.IntSlider("Repetition", notification.repetition, 0, 60);
		
		notification.advancedNotification = EditorGUILayout.Foldout(notification.advancedNotification,"Advanced notification" );
		
		if ( notification.advancedNotification ){
			notification.useVibration = EditorGUILayout.Toggle("Use virbration", notification.useVibration);
			notification.useSound     = EditorGUILayout.Toggle("Use sound", notification.useSound);
			if(notification.useSound) {
				notification.soundName = EditorGUILayout.TextField("Sound name",notification.soundName );
			}
		}
    }
}
