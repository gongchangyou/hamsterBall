using UnityEngine;
using System.Collections;

public class menu : MonoBehaviour {

	void Awake(){
		for (int i=1; i<3; i++) {
			string areaName = "Area" + i;
			if(!PlayerPrefs.HasKey(areaName)){
				if(i == 1 || PlayerPrefs.HasKey("Area" + (i - 1))){
				}else{
					string buttonName = "button" + i;
					GameObject button = GameObject.Find (buttonName);
					button.GetComponent<UIButton>().isEnabled = false;
					button.GetComponent<UIButtonMessage>().enabled = false;
				}
			}
		}
	}
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
