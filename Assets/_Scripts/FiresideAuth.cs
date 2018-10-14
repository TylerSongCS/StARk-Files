using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class FiresideAuth : MonoBehaviour {
	

	public InputField EmailAdd, Password;

	public void LoginPressed(){
		FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (EmailAdd.text, Password.text).
			ContinueWith((obj) =>
		{
			SceneManager.LoadSceneAsync("DataChoose");
		});
	
	}
	public void loginAnonPressed(){
		FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().
		ContinueWith((obj) =>
		{
				SceneManager.LoadSceneAsync("DataChoose");
		});
	}
	public void CreateNewUserButtonPressed(){
		FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync (EmailAdd.text, Password.text).
			ContinueWith((obj) =>
		{
				SceneManager.LoadSceneAsync("DataChoose");
		});
	}


}
