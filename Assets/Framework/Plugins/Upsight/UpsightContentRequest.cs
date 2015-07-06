using UnityEngine;
using System.Collections;


public class UpsightContentRequest : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	public string placementID;
	public bool showsOverlayImmediately = true;
	public bool shouldAnimate = true;


	void Start()
	{
		Upsight.sendContentRequest( placementID, showsOverlayImmediately, shouldAnimate );
	}


	void OnEnable()
	{
		UpsightManager.contentRequestLoadedEvent += contentRequestLoaded;
		UpsightManager.contentRequestFailedEvent += contentRequestFailed;
		UpsightManager.contentWillDisplayEvent += contentWillDisplay;
		UpsightManager.contentDismissedEvent += contentDismissed;
	}


	void OnDisable()
	{
		UpsightManager.contentRequestLoadedEvent -= contentRequestLoaded;
		UpsightManager.contentRequestFailedEvent -= contentRequestFailed;
		UpsightManager.contentWillDisplayEvent -= contentWillDisplay;
		UpsightManager.contentDismissedEvent -= contentDismissed;
	}


	void contentRequestLoaded( string placementID )
	{}


	void contentRequestFailed( string placementID, string error )
	{}


	void contentWillDisplay( string placementID )
	{}


	void contentDismissed( string placementID, string dismissType )
	{}

#endif
}