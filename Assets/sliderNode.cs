using UnityEngine;
using System.Collections;

public class sliderNode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnSliderChange(float value)  {
		Debug.Log ("slider value=" + value);

	}
}
