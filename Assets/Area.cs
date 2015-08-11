using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {
	[SerializeField]
	protected GameObject sphere;
	[SerializeField]
	protected Camera camera;
	Vector2 lastPos = Vector2.zero;

	protected Vector3 sphereStartPos;
	protected Vector3 cameraStartPos;

	public bool canMove{
		get{ return _canMove;}
		set{ _canMove = value;}
	}
	private bool _canMove = true;

	// Use this for initialization
	protected void Start () {
		Gesture.onDraggingE += OnDragging;
		Gesture.onDraggingEndE += OnDraggingEnd;
	}
	
	// Update is called once per frame
	void Update () {
		if (sphere.transform.position.y < -10) {
			_canMove = true;
			sphere.transform.position = sphereStartPos;
			sphere.rigidbody.Sleep();
//			sphere.rigidbody.velocity = new Vector3(0,0,0);
//			sphere.rigidbody.rotation = Quaternion.Euler(0,0,0);
//			Debug.Log("post velocity:" + sphere.rigidbody.velocity.ToString());
		}

		Vector3 tmp = Vector3.zero;
		tmp.x = (sphere.transform.position.x - sphereStartPos.x) / 2;
		tmp.y = sphere.transform.position.y - sphereStartPos.y;
		tmp.z = sphere.transform.position.z;
		camera.transform.position = cameraStartPos + tmp;
		//camera moveTo
		/*
		if (Mathf.Abs( sphere.transform.position.z ) > 10 && camera.transform.position.z != -11 + Mathf.Round (sphere.transform.position.z)) {
			iTween.MoveTo (camera.gameObject, iTween.Hash (
			"z", -11 + Mathf.Round (sphere.transform.position.z),
			"time", 0.1f, "islocal", true, "easetype", iTween.EaseType.linear));
		}
		*/
		
	}

	void OnDragging(DragInfo dragInfo){
		//Debug.Log (dragInfo.pos);
		if (!_canMove) {
			return;
		}
		if (lastPos == Vector2.zero) {
			lastPos = dragInfo.pos;
		} else {
			Vector2 curPos = dragInfo.pos;
			Vector2 tmp = curPos - lastPos;
			Vector3 force = new Vector3(tmp.x, 0, tmp.y);
			force.Normalize();
			force *= 50;
			sphere.rigidbody.velocity = sphere.rigidbody.velocity * 0.1f;
			sphere.rigidbody.AddForce(force);
//			sphere.rigidbody.velocity = force;
//			Debug.Log("Draging");

		}
	
	}

	void OnDraggingEnd(DragInfo dragInfo){
		lastPos = Vector2.zero;
		Debug.Log("Drag end");
	}

	void updateCanMove(bool canMove){
		_canMove = canMove;
	}
}
