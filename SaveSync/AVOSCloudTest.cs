using UnityEngine;
using System.Collections;
using AVOSCloud;

public class AVOSCloudTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {}
	private string msg=string.Empty;
	void OnGUI()
	{
		GUI.Label (new Rect (270, 50, 200, 80), msg);
		if (GUI.Button (new Rect (50, 50, 200, 80), "添加GameScore"))
		{
			AVObject gameScore =new AVObject("GameScore");
			gameScore["score"] = 1337;
			gameScore["playerName"] = "Neal Caffrey";
			gameScore ["darin"] = "darinlo";
			gameScore.SaveAsync().ContinueWith(t=>
				{
					if(!t.IsFaulted)
					{
						msg="保存成功";
					}
				});
		}
	}

}
		
