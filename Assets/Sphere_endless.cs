using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Sphere_endless : Sphere {


	void OnCollisionEnter(Collision collisionInfo)
	{
//		Debug.Log ("sphere endless OnCollisionEnter");
	}

	void OnCollisionStay(Collision collisionInfo){
//		Debug.Log ("sphere endless OnCollisionStay");
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", true);
		}
	}

	void OnCollisionExit(Collision collisionInfo)
	{
//		Debug.Log ("sphere endless OnCollisionExit");
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", false);
		}
	}

}
