using UnityEngine;
using System.Collections;

public class MainScene : MonoBehaviour {
	
	public ELANNotification notification;
	public ELANNotification parametrizedNotification1;
	public ELANNotification parametrizedNotification2;
	public ELANNotification parametrizedNotification3;
	
	private ELANNotification parametrizedNotification4;
	private ELANNotification parametrizedNotification5;
	
	private bool sendParametrizedNotification1;
	private bool sendParametrizedNotification2;
	private bool sendParametrizedNotification3;
	private bool sendParametrizedNotification4;
	private bool sendParametrizedNotification5;

	string delay;
	string rep;
	
	private string message = "Set the repeating box to 0 or negative for one off notifications";
	private string errorRepetition = "Repetition must be an integer!";
	private string errorDelay = "Delay must be an integer!";
	private int errorType = 0;
	private string logMessage = "";
	
	
	void Start(){
		delay = notification.delay+"";
		rep = notification.repetition+"";
	}

	
	void OnGUI () {
		// Schedule notifications GUI
		GUI.Box(new Rect(10,10,300,400), "ELAN Demo");

		if(GUI.Button(new Rect(20,40,120,40), "Send")) {
			if ( int.TryParse(delay,out notification.delay) 
				&& int.TryParse(rep,out notification.repetition))
			notification.send();
		}
		
		notification.title = GUI.TextField (new Rect(20,90,120,40), notification.title, 15);
		notification.message = GUI.TextField (new Rect(20,140,120,40), notification.message, 15);
		GUI.Label (new Rect(20,190,120,40),"Delay");
		delay = GUI.TextField (new Rect(120,190,120,40), delay, 15);
		GUI.Label (new Rect(20,240,120,40),"Repetition");
		rep = GUI.TextField (new Rect(120,240,120,40), rep, 15);
		
		if ( int.TryParse(delay,out notification.delay) ){notification.delay = int.Parse(delay);}else{errorType = 1;}
		if ( int.TryParse(rep, out notification.repetition) ){notification.repetition = int.Parse(rep);}else{errorType = 2;}
		
		if ( int.TryParse(delay,out notification.delay) && int.TryParse(rep, out notification.repetition) )errorType = 0;
		
		notification.useSound = GUI.Toggle(new Rect(20,300,190,40),notification.useSound,"Use sound");
		notification.useVibration = GUI.Toggle(new Rect(20,330,190,40),notification.useVibration,"Use vibration");
		
		
		
		switch(errorType){
		case 0:
			logMessage = message;
			break;
		case 1:
			logMessage = errorDelay;
			break;
		case 2:
			logMessage = errorRepetition;
			break;
		default:
			logMessage = message;
			break;
			
		}
		
		if(GUI.Button(new Rect(20,450,120,40), "Exit")) {
			Application.Quit();
		}
		
		GUI.Label (new Rect(20,500,300,40),logMessage);
		
		// Cancel scheduled notification GUI
		GUI.Box(new Rect(410,10,300,150), "Cancel notifications");

		if(GUI.Button(new Rect(420,40,140,40), "Cancel repeating")) {
			ELANManager.CancelRepeatingNotification();
		}
		if(GUI.Button(new Rect(420,90,140,40), "Cancel ALL")) {
			ELANManager.CancelAllNotifications(); // NEW added safety check (some can be null)
			if(parametrizedNotification1 != null) parametrizedNotification1.cancel();
			if(parametrizedNotification2 != null) parametrizedNotification2.cancel();
			if(parametrizedNotification3 != null) parametrizedNotification3.cancel();
			if(parametrizedNotification4 != null) parametrizedNotification4.cancel();
			if(parametrizedNotification5 != null) parametrizedNotification5.cancel();
			if(notification != null) notification.cancel();
		}
		
		
		//Advanced notifications
		sendParametrizedNotification1=GUI.Toggle(new Rect(420,170,190,40),sendParametrizedNotification1,"Parametrized notification 1");
		sendParametrizedNotification2=GUI.Toggle(new Rect(600,170,190,40),sendParametrizedNotification2,"Parametrized notification 2");
		sendParametrizedNotification3=GUI.Toggle(new Rect(420,200,190,40),sendParametrizedNotification3,"Parametrized notification 3");
		sendParametrizedNotification4=GUI.Toggle(new Rect(600,200,190,40),sendParametrizedNotification4,"Parametrized notification 4");
		sendParametrizedNotification5=GUI.Toggle(new Rect(420,230,190,40),sendParametrizedNotification5,"Parametrized notification 5");
		
		
		if(GUI.Button(new Rect(420,280,180,40), "Send parametrized notifications")) {
			
			if (sendParametrizedNotification1)parametrizedNotification1.send();
			if (sendParametrizedNotification2)parametrizedNotification2.send();
			if (sendParametrizedNotification3)parametrizedNotification3.send();
			if (sendParametrizedNotification4)
			{
				parametrizedNotification4 = new ELANNotification();
				parametrizedNotification4.message = "Hello from parametrized notification 4";
				parametrizedNotification4.title = "parametrized notification 4";
				parametrizedNotification4.send();
			}
			if (sendParametrizedNotification5){
				
				parametrizedNotification5 = new ELANNotification();
				parametrizedNotification5.message = "Hello from parametrized notification 5";
				parametrizedNotification5.title = "parametrized notification 5";
				parametrizedNotification5.repetition = 5;
				parametrizedNotification5.delay = 3;
				parametrizedNotification5.useSound = true;
				parametrizedNotification5.useVibration = true;
				parametrizedNotification5.send();
			}
			
		}
		
	}
}
