using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area1 : Area {
	// Use this for initialization
	void Awake(){
		maxSeconds = 15.0f;
		base.Awake ();
		sphereStartPos = sphere.transform.position;
		cameraStartPos = camera.transform.position;// new Vector3 (0,13,-12); 
	}

	void Start () {
		base.Start();
	}
}
