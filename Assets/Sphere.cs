using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collisionInfo)
	{

	}

	void OnCollisionStay(Collision collisionInfo){
//		Debug.Log("碰撞到的物体的名字是：" + collisionInfo.gameObject.name);
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("canMove", true);
		}
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		Debug.Log("exit 碰撞到的物体的名字是：" + collisionInfo.gameObject.name);
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("canMove", false);
		}
	}
}
