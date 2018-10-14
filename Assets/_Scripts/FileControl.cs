using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FileControl : MonoBehaviour {
	public static string FilePath;

	public void documentButtonPressed(){
		FilePath = "document";
		SceneManager.LoadSceneAsync("AR_DataStore");
	}

	public void sdhacksButtonPressed(){
		FilePath = "sdhacks";
		SceneManager.LoadSceneAsync("AR_DataStore");
	}
	// Update is called once per frame

}
