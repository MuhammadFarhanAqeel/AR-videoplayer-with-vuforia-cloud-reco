using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Video;
using UnityEngine.UI;

public class SimpleCloudHandler : MonoBehaviour,ICloudRecoEventHandler {

	public ImageTargetBehaviour behaviour;
	CloudRecoBehaviour cloud;
	public GameObject mainPlayer;
	public Text ErrorTxt;
	string errorTitle,errorMsg;

	public void OnInitError (TargetFinder.InitState initError){
		switch (initError) {
		case TargetFinder.InitState.INIT_ERROR_NO_NETWORK_CONNECTION:
			errorTitle = "Network Unavailble";
			errorMsg = "Check internet connection and try again";
			break;
		case TargetFinder.InitState.INIT_ERROR_SERVICE_NOT_AVAILABLE:
			errorTitle = "Service not availble";
			errorMsg = "Failed to initialize beacause service is unavailble";
			break;
		}
		errorMsg = "<color=red>" + initError.ToString ().Replace ("_", " ") + "</color>\n\n" + errorMsg;
		ErrorTxt.text = "Cloud Reco - Update Error: " + initError + "\n\n" + errorMsg;
	}

	public void OnInitialized (){}

	public void OnNewSearchResult (TargetFinder.TargetSearchResult targetSearchResult){
		GameObject newImageTarget = Instantiate (behaviour.gameObject) as GameObject;
		mainPlayer = newImageTarget.transform.GetChild (0).gameObject;
		GameObject augmentation = null;
		if (augmentation != null) {
			augmentation.transform.SetParent (newImageTarget.transform);
		}
		if (behaviour) {
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();
			ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)tracker.TargetFinder.EnableTracking (targetSearchResult, newImageTarget);
		}
		string URL = targetSearchResult.MetaData;
		mainPlayer.GetComponent<VideoPlayer> ().url = URL.Trim();
		cloud.CloudRecoEnabled = true;
	}

	public void OnStateChanged (bool scanning){
		if (scanning) {
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();
			tracker.TargetFinder.ClearTrackables (false);
		}
	}

	public void OnUpdateError (TargetFinder.UpdateState updateError){
		switch (updateError) {
		case TargetFinder.UpdateState.UPDATE_ERROR_AUTHORIZATION_FAILED:
			errorTitle = "Authorization Error";
			errorMsg = "The cloud server access keys are invalid";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_NO_NETWORK_CONNECTION:
			errorTitle = "Network Error";
			errorMsg = "Check Internet Connection";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_PROJECT_SUSPENDED:
			errorTitle = "Authorization Error";
			errorMsg = "The project has been suspended";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_REQUEST_TIMEOUT:
			errorTitle = "Request Timeout";
			errorMsg = "The request has timed out";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_SERVICE_NOT_AVAILABLE:
			errorTitle = "Service Unavailble";
			errorMsg = "The service is unavailble";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_TIMESTAMP_OUT_OF_RANGE:
			errorTitle = "Clock Sync Error";
			errorMsg = "Update date and time";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_UPDATE_SDK:
			errorTitle = "Unsupported version";
			errorMsg = "Update SDK";
			break;
		case TargetFinder.UpdateState.UPDATE_ERROR_BAD_FRAME_QUALITY:
			errorTitle = "Bad Frame";
			errorMsg = "Low Frame Quality";
			break;
		
		}

		errorMsg = "<color=red>" + updateError.ToString ().Replace ("_", " ") + "</color>\n\n" + errorMsg;
		ErrorTxt.text = "Cloud Reco - Update Error: " + updateError + "\n\n" + errorMsg;

	}

	// Use this for initialization
	void Start () {
		CloudRecoBehaviour cloudReco = GetComponent<CloudRecoBehaviour> ();
		if (cloudReco) {
			cloudReco.RegisterEventHandler (this);
		}
		cloud = cloudReco;
		mainPlayer = GameObject.Find ("Player");
		Hide (mainPlayer);
	}

	void Hide (GameObject ob) {
		Renderer[] rends = ob.GetComponentsInChildren<Renderer> ();
		Collider[] cols = ob.GetComponentsInChildren<Collider> ();

		foreach (var item in rends) {
			item.enabled = false;
		}
		foreach (var item in cols)
			item.enabled = false;

	}
}
