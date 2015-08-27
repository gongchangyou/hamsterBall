using UnityEngine;
using System.Collections;

public class CubeCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collisionInfo){
//		Debug.Log ("endless" + collisionInfo.gameObject.name);
		transform.parent.GetComponent<Area_endless> ().addNewCube (gameObject);
	}
}
