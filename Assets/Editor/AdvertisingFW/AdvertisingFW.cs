#if UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using System;
using System.Diagnostics;

public class CustomPostprocessScript : MonoBehaviour
{

	//IMPORTANT!!!
	//100 is order , it means this one will execute after e.g 1 as default one is 1 
	//it means our script will run after all other scripts got run
	[PostProcessBuild(99999)]
	public static void OnPostprocessBuild (BuildTarget target, string pathToBuildProject)
	{
                                  
		UnityEngine.Debug.Log ("AdvertisingFW: Executing GRAM POST PROCESSOR.");      
		Process myCustomProcess = new Process ();        
		myCustomProcess.StartInfo.FileName = "python";
		myCustomProcess.StartInfo.Arguments = string.Format ("Assets/Editor/AdvertisingFW/PostProcess_AdvertisingFW.py \"{0}\"", pathToBuildProject);
		myCustomProcess.StartInfo.UseShellExecute = false;
		myCustomProcess.StartInfo.RedirectStandardOutput = false;
		myCustomProcess.Start (); 
		myCustomProcess.WaitForExit ();
	}
}
        
                                                                                                                                  #endif
