using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using Firebase;
using Firebase.Storage;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class UDTController : MonoBehaviour, IUserDefinedTargetEventHandler {
	
	UserDefinedTargetBuildingBehaviour udt_targetBuildingBehavior;

	ObjectTracker objectTracker;
	DataSet dataSet;
	int targetCounter;

	ImageTargetBuilder.FrameQuality udt_FrameQuality;
	public ImageTargetBehaviour targetBehavior;
	public Text qualityText;
	public Text docText;
	string txtContent;
	// Use this for initialization
	void Start () {
		udt_targetBuildingBehavior = GetComponent<UserDefinedTargetBuildingBehaviour> ();
		if (udt_targetBuildingBehavior) {
			udt_targetBuildingBehavior.RegisterEventHandler (this);
		}
	}

	//updates the frame quality
	public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality){
		udt_FrameQuality = frameQuality;
	}

	public void OnInitialized(){
		objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();

		if (objectTracker != null) {
			dataSet = objectTracker.CreateDataSet ();
			objectTracker.ActivateDataSet (dataSet);
		}
	}
	public void OnNewTrackableSource(TrackableSource trackableSource){
		targetCounter++;
		objectTracker.DeactivateDataSet (dataSet);

		dataSet.CreateTrackable (trackableSource, targetBehavior.gameObject);
		objectTracker.ActivateDataSet (dataSet);
		udt_targetBuildingBehavior.StartScanning ();
	}
	public void BuildTarget(){
		if (udt_FrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH) {
			udt_targetBuildingBehavior.BuildNewTarget (targetCounter.ToString(), targetBehavior.GetSize().x);

			Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

			Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://ar-demo-1f5c9.appspot.com");

			Firebase.Storage.StorageReference file_ref = storage_ref.Child("images/" + FileControl.FilePath);

			file_ref.PutFileAsync("./Assets/Resources/" + FileControl.FilePath+".txt").ContinueWith((Task<StorageMetadata> task) =>
				{
					if (task.IsFaulted || task.IsCanceled)
					{
						Debug.Log("STILL HERE");
						Debug.Log(task.Exception.ToString());
						// Uh-oh, an error occurred!
					}
					else
					{
						// Metadata contains file metadata such as size, content-type, and download URL.
						Firebase.Storage.StorageMetadata metadata = task.Result;
						//string download_url = metadata.DownloadUrl.ToString();
						//Debug.Log("Finished uploading...");
						//Debug.Log("download url = " + download_url);
					}
				});
		}
	}
	// Update is called once per frame
	void Update () {
		if (udt_FrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH) {
			qualityText.text = "READY";
			qualityText.color = Color.green;
		} else {
			qualityText.text = "POOR QUALITY";
			qualityText.color = Color.red;
		}
	}
	public void OnDocButtonPressed(){
		TextAsset txtAssets = (TextAsset)Resources.Load (FileControl.FilePath);
		docText.text = txtAssets.text;
	}

}
