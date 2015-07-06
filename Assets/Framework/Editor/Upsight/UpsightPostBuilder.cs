using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using System.Diagnostics;


public class UpsightPostBuilder : MonoBehaviour
{
	private const string kDisableIDFAKeyPrefix = "UpsightDisableIDFAInXcodeProject-";
	private static bool shouldDisableIDFAInXcodeProject
	{
		get
		{
			var key = kDisableIDFAKeyPrefix + PlayerSettings.productName;
			return EditorPrefs.GetBool( key, false );
		}
		set
		{
			var key = kDisableIDFAKeyPrefix + PlayerSettings.productName;
			EditorPrefs.SetBool( key, value );
		}
	}


	private static void buildDemo()
	{
		var rootPath = Path.Combine( Application.dataPath, "../Build");
		var iosExportPath = Path.Combine( rootPath, "UpsightiOSBuild" );
		var androidExportPath = Path.Combine( rootPath, "UpsightAndroid.apk" );
		var unitypackageExportPath = Path.Combine( rootPath, "UpsightPlugin.unitypackage" );

		EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.iOS );
        BuildPipeline.BuildPlayer(new string[] { "Assets/Plugins/Upsight/demo/UpsightDemo.unity" }, iosExportPath, BuildTarget.iOS, BuildOptions.None);

		EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.Android );
		BuildPipeline.BuildPlayer( new string[] { "Assets/Plugins/Upsight/demo/UpsightDemo.unity" }, androidExportPath, BuildTarget.Android, BuildOptions.None );

		createDemoAssetBundle( unitypackageExportPath );
	}


	private static void createDemoAssetBundle( string unitypackageExportPath )
	{
		// grab all the files and create a unitypackage
		var allFiles = Directory.GetFiles( Application.dataPath, "*", SearchOption.AllDirectories )
			.Where( file => Path.GetFileName( file ) != "mod_pbxproj.pyc" )
			.Where( file => !file.EndsWith( ".meta" ) )
			.Where( file => !file.EndsWith( ".DS_Store" ) )
			.Where( file => !file.Contains( "QADemo" ) )
			.Select( file => file.Replace( Application.dataPath, "Assets" ) );

		UnityEngine.Debug.Log( allFiles.Count() );
		AssetDatabase.ExportPackage( allFiles.ToArray(), unitypackageExportPath );
	}


	private static void buildInternalDemo()
	{
		var rootPath = Application.dataPath.Replace( "Assets", "Build" );
		var iosExportPath = Path.Combine( rootPath, "UpsightInternaliOSBuild" );
		var androidExportPath = Path.Combine( rootPath, "UpsightInternalAndroid.apk" );

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
        BuildPipeline.BuildPlayer(new string[] { "Assets/Plugins/Upsight/QADemo/QADemoScene.unity" }, iosExportPath, BuildTarget.iOS, BuildOptions.None);

		EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.Android );
		BuildPipeline.BuildPlayer( new string[] { "Assets/Plugins/Upsight/QADemo/QADemoScene.unity" }, androidExportPath, BuildTarget.Android, BuildOptions.None );


		// iOS specific project modifications
		var pchPath = Path.Combine( iosExportPath, "Classes/iPhone_target_Prefix.pch" );
		if( File.Exists( pchPath ) )
		{
			UnityEngine.Debug.Log( "------- modifying pch file to override PH_BASE_URL" );
			var lines = new List<string>( File.ReadAllLines( pchPath ) );
			lines.Add( "#define PH_BASE_URL [[NSUserDefaults standardUserDefaults] stringForKey:@\"USBaseUrl\"] ? [[NSUserDefaults standardUserDefaults] stringForKey:@\"USBaseUrl\"] : @\"http://api2.playhaven.com\"" );
			File.WriteAllLines( pchPath, lines.ToArray() );
		}

		var infoPlistPath = Path.Combine( iosExportPath, "Info.plist" );
		if( File.Exists( infoPlistPath ) )
		{
			// we can do a "dumb mod" here since we always know we are working from the unity trampoline
			// this will avoid having to deal with builds on Windows vs Mac and writing a plist parser
			UnityEngine.Debug.Log( "------- modifying Info.plist file. Adding UIViewControllerBasedStatusBarAppearance key" );
			var lines = new List<string>( File.ReadAllLines( infoPlistPath ) );
			var insertIndex = 0;
			for( ; insertIndex < lines.Count; insertIndex++ )
			{
				// we know we have a plist so the root is a dictionary. Find that and inject right after.
				if( lines[insertIndex].Contains( "<dict>" ) )
					break;
			}

			// inject the new key/value at the top of the dictionary
			lines.Insert( ++insertIndex, "\t<key>UIViewControllerBasedStatusBarAppearance</key>" );
			lines.Insert( ++insertIndex, "\t<false/>" );
			File.WriteAllLines( infoPlistPath, lines.ToArray() );
		}
	}


	[PostProcessBuild]
	private static void onPostProcessBuildPlayer( BuildTarget target, string pathToBuiltProject )
	{
		if( target == BuildTarget.Android )
		{
			if( !File.Exists( Path.Combine( Application.dataPath, "Plugins/Android/AndroidManifest.xml" ) ) )
			{
				UnityEngine.Debug.Log( "Could not find an AndroidManifest.xml file. Generating one now. You will need to rebuild your project for the changes to take affect." );
				generateAndroidManifest();
			}
		}
        else if (target == BuildTarget.iOS)
		{
			// grab the path to the postProcessor.py file
			var scriptPath = Path.Combine( Application.dataPath, "Editor/Upsight/postProcessor.py" );

			// sanity check
			if( !File.Exists( scriptPath ) )
			{
				UnityEngine.Debug.LogError( "Upsight post processor could not find the postProcessor.py file. Did you accidentally delete it?" );
				return;
			}

			var args = string.Format( "\"{0}\" \"{1}\" \"{2}\"", scriptPath, pathToBuiltProject, shouldDisableIDFAInXcodeProject ? "1" : "0" );
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "python2.6",
					Arguments = args,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			proc.Start();
		}
	}


	private static void generateAndroidManifest()
	{
		var baseManifestPath = Path.Combine( Application.dataPath, "Editor/Upsight/BaseAndroidManifest.xml" );
		if( !File.Exists( baseManifestPath ) )
		{
			EditorUtility.DisplayDialog( "Upsight Error", "The BaseAndroidManifest.xml file could not be found. Please reimport the plugin and try again", "OK" );
			return;
		}

		var androidManifestPath = Path.Combine( Application.dataPath, "Plugins/Android/AndroidManifest.xml" );
		if( File.Exists( androidManifestPath ) )
		{
			if( !EditorUtility.DisplayDialog( "Upsight", "There is already an AndroidManifest.xml present. Should we overwrite the file with a fresh AndroidManifest.xml file (any changes you made to it will be lost)?", "Overwrite It", "Abort" ) )
				return;

			File.Delete( androidManifestPath );
		}

		var fileContents = File.ReadAllText( baseManifestPath );

		// the only thing we need in there is the package so a simple replace will suffice
		fileContents = fileContents.Replace( "CURRENT_PACKAGE_NAME", PlayerSettings.bundleIdentifier );

		File.WriteAllText( androidManifestPath, fileContents );
		AssetDatabase.Refresh();
	}


	#region Upsight menu

	static bool isAndroid()
	{
		return EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.Android;
	}


	static bool isIos()
	{
        return EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.iOS;
	}


	[UnityEditor.MenuItem( "Upsight/Generate AndroidManifest.xml file", true )]
	static bool validateGenerateAndroidManifestMenuItem()
	{
		return isAndroid();
	}


	[UnityEditor.MenuItem( "Upsight/Generate AndroidManifest.xml file" )]
	static void generateAndroidManifestMenuItem()
	{
		generateAndroidManifest();
	}


	[UnityEditor.MenuItem( "Upsight/iOS IDFA Inclusion/Disable IDFA Acccess for Project", true )]
	static bool validateEnableIDFARemoval()
	{
		return isIos() && !shouldDisableIDFAInXcodeProject;
	}


	[UnityEditor.MenuItem( "Upsight/iOS IDFA Inclusion/Disable IDFA Acccess for Project", false )]
	static void EnableIDFARemoval()
	{
		shouldDisableIDFAInXcodeProject = true;
		EditorUtility.DisplayDialog( "Upsight", "You will need to regenerate your Xcode project for this to take effect.", "OK" );
	}


	[UnityEditor.MenuItem( "Upsight/iOS IDFA Inclusion/Enable IDFA Access for Project", true )]
	static bool validateDisableIDFARemoval()
	{
		return isIos() && shouldDisableIDFAInXcodeProject;
	}


	[UnityEditor.MenuItem( "Upsight/iOS IDFA Inclusion/Enable IDFA Access for Project", false )]
	static void DisableIDFARemoval()
	{
		shouldDisableIDFAInXcodeProject = false;
		EditorUtility.DisplayDialog( "Upsight", "You will need to regenerate your Xcode project for this to take effect.", "OK" );
	}

	#endregion

}
