using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.IO;
using UnityEngineInternal;

public class Registered : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		var userName = "darinlo";
		var pwd = "888888";
		var email = "baoao11@gmail.com";
		var user = new AVUser();
		user.Username = userName;
		user.Password = pwd;
		user.Email = email;
		user.SignUpAsync().ContinueWith(t =>
			{
				var uid = user.ObjectId;
			});
	}
}
