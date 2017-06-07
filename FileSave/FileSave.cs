using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.IO;


public class FileSave : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	private string fildId;
	//private string UserID;
	string UserID = "baoao11@gmail.com";
	void Update () {
	}
	 void OnGUI()
		{
		GUI.Label(new Rect(260, 50, 160, 50), fildId);
		if (GUI.Button(new Rect(50, 50, 200, 50), "Save"))
		{
			AVFile file = AVFile.CreateFileWithLocalPath ("SaveA.xml", Path.Combine (Application.persistentDataPath, "SaveA.xml"));{
				file.MetaData.Add("UserID", UserID);
				file.SaveAsync().ContinueWith(t =>
				{
					Debug.Log(t.IsFaulted);
					if (!t.IsFaulted)
					{

						fildId = file.ObjectId;
					}
					else
					{
						Debug.Log(t.Exception.Message);
						Debug.LogException(t.Exception);
					}
				});
		}
	}
  }
}
