using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.IO;
using UnityEngineInternal;


public class DownFile : MonoBehaviour {
	
	//GameObject file;
	string ObjectId= "570b527571cfe400553517ee" ;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {	
		
	}
	void OnGUI(){
		if (GUI.Button(new Rect(50, 100, 200, 50), "Download file"))
		{
			AVFile.GetFileWithObjectIdAsync(ObjectId).ContinueWith(t =>
				{
					var file = t.Result;
					file.DownloadAsync().ContinueWith(s =>
						{
							var dataByte = file.DataByte;//取文件流的byte数，之后可以做保存，送等操作。
						});
				});
		}
	}
}


	

