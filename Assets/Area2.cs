using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area2 : Area {
	// Use this for initialization
	void Awake(){
		maxSeconds = 2.0f;
		base.Awake ();

	}

	void Start () {
		base.Start();
	}
}
