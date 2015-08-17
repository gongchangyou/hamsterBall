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
		switch (button.name) {
		case "button1":
			areaName = "area1";
			break;
		}

		changeToArea (areaName);
	}
	void changeToArea(string areaName){
		Application.LoadLevel(areaName);
	}
}
