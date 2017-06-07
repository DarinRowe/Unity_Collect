using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.IO;


public class DeleSave : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if (GUI.Button(new Rect(50, 150, 200, 50), "Delete file"))
		{
			AVFile.GetFileWithObjectIdAsync("570b527571cfe400553517ee").ContinueWith(t =>
				{
					var file = t.Result;
					file.DeleteAsync();
				});
		}
	}

}
