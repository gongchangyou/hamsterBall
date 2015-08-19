using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Sphere : MonoBehaviour {

	private float angularDrag = 0.5f;

	private float crashHeight = 0.5f;

	public Vector3 jumpPos = Vector3.zero;
	private List<Vector3> trace = new List<Vector3>();

	public bool crash = false;

	public bool startFly = false;

	public bool inCube = false;
	// Use this for initialization
	void Start () {
		rigidbody.angularDrag = angularDrag;
	}
	
	// Update is called once per frame
	void Update () {
//		GameObject area = GameObject.Find("Area");
//		if (area.GetComponent<Area> ().canMove){
//		    if(rigidbody.velocity.y < -0.01f) {//down
//				rigidbody.velocity *= 1.01f;
//				rigidbody.angularDrag = 0.0f;
//			}else{
//				rigidbody.angularDrag = angularDrag;
//				if(rigidbody.velocity.y > 0.01f){//up
//					rigidbody.velocity *= 0.95f;
//				}
//			}
//		}
//		Debug.Log ("sphere angurlar" + rigidbody.angularVelocity);

	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("sphere trigger");
		inCube = true;
	}	

	void OnTriggerExit(Collider other){
		Debug.Log ("sphere trigger eixt");
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("sphere OnCollisionEnter  jumpPos="+jumpPos + "curPos="+transform.position);
		//judge crash
		if (inCube) {
			inCube = false;
		} else {
			if (startFly) {
				Vector3 curPos = transform.position;
				Vector3 lastPos = jumpPos;

				if (lastPos.y - curPos.y > crashHeight) {
					crash = true;
					Debug.Log ("crash=true");
				}
			}
		}
	}

	void OnCollisionStay(Collision collisionInfo){
//		Debug.Log("碰撞到的物体的名字是：" + collisionInfo.gameObject.name);
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", true);
		}
		updateJumpPos ();
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		Debug.Log("exit 碰撞到的物体的名字是：" + collisionInfo.gameObject.name);
		if (!inCube) {
			startFly = true;
		}
		GameObject area = GameObject.Find("Area");
		if (area) {
			area.SendMessage ("updateCanMove", false);
		}
	}

	void updateJumpPos(){
		trace.Add (transform.position);
		jumpPos = trace [(int)Mathf.Max( 0, trace.Count - 0.5f/Time.fixedDeltaTime)];//the position before 0.5 second
	}

	void FixedUpdate(){
		//update angular velocity whit velocity 
		Vector3 v = rigidbody.velocity;
//		Debug.Log ("v="+v + "av="+rigidbody.angularVelocity);
//		rigidbody.angularVelocity = new Vector3 (v.z,0,-v.x) / GetComponent<SphereCollider>().radius;

	}
}
