using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area1 : Area {
	// Use this for initialization
	void Awake(){
		maxSeconds = 2000.0f;
		base.Awake ();
	}

	void Start () {
		base.Start();
	}

	protected override void setPlayerInt(){
		PlayerPrefs.SetInt ("Area1", 1);
	}
}
