using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void openArea(GameObject button){
		string areaName = "";
		//button.name is "button1" "button2" etc.
		areaName = "area" + button.name.Substring (6, button.name.Length - 6);
		Debug.Log (areaName);
		changeToArea (areaName);
	}
	void changeToArea(string areaName){
		Application.LoadLevel(areaName);
	}
}
