using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

	private float angularDrag = 1.0f;

	public Vector3 jumpPos = Vector3.zero;
	// Use this for initialization
	void Start () {
		rigidbody.angularDrag = angularDrag;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject area = GameObject.Find("Area");
		if (area.GetComponent<Area> ().canMove){
		    if(rigidbody.velocity.y < -0.01f) {//down
				rigidbody.velocity *= 1.01f;
				rigidbody.angularDrag = 0.0f;
			}else{
				rigidbody.angularDrag = angularDrag;
				if(rigidbody.velocity.y > 0.01f){//up
					rigidbody.velocity *= 0.95f;
				}
			}
		}
//		Debug.Log ("sphere angurlar" + rigidbody.angularVelocity);

	}

	void OnCollisionEnter(Collision collisionInfo)
	{

	}

	void OnCollisionStay(Collision collisionInfo){
//		Debug.Log("碰撞到的物体的名字是：" + collisionInfo.gameObject.name);
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", true);
		}
		Vector3 vNormal = rigidbody.velocity;
		vNormal.Normalize ();
		jumpPos = transform.position - vNormal * 0.5f;
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		Debug.Log("exit 碰撞到的物体的名字是：" + collisionInfo.gameObject.name);

		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", false);
		}
	}
}
