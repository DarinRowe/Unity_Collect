using UnityEngine;
using System.Collections;
using AVOSCloud;
using System.IO;
using UnityEngineInternal;

public class Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var userName = "darinlo";
		var pwd = "888888";
		AVUser.LogInAsync(userName, pwd).ContinueWith(t =>
			{
				if (t.IsFaulted || t.IsCanceled)
				{
					var error = t.Exception.Message; // 登录失败，可以查看错误信息。
				}
				else
				{
					//登录成功
				}
			});
	}
}
