using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area_endless : Area {
	List<GameObject> map = new List<GameObject>();

	int maxMesh = 10;
	int meshIndex = 0;
	// Use this for initialization
	void Awake(){
		base.Awake ();

		timesLabel.gameObject.SetActive (false);
		//at least 3 gameObject
		addPath ("cube", new Vector3(0, -0.2f,0));
		addPath ("cube", new Vector3(0, -0.3f,1.5f));
		addPath ("cube", new Vector3(0, -0.35f,3.0f));
		meshIndex = 3;
	}

	void Start () {
		base.Start();
	}

	protected override void setPlayerInt(){
		PlayerPrefs.SetInt ("Area2", 1);
	}

	void addPath(string pathName, Vector3 pos){
		//build cube
		GameObject mainFbx = Instantiate (Resources.Load ("mapFactory/" + pathName)) as GameObject; 
		GameObject gFbx = mainFbx.transform.Find (pathName).gameObject;

		Debug.Log (gFbx);
		mainFbx.transform.parent = this.transform;
		gFbx.transform.localScale = Vector3.one * 50;
		gFbx.transform.position = pos;

//		MeshCollider mc = gFbx.GetComponent<MeshCollider> ();
//		PhysicMaterial pm = Resources.Load ("pm") as PhysicMaterial;
//		mc.material = pm;

//		mc.sharedMesh = gFbx.GetComponent<MeshRenderer> ();

		//add script
//		gFbx.AddComponent<CubeCollider>();

		map.Add (gFbx);

		meshIndex++;
	}

	public void addNewCube(GameObject cube){
		Debug.Log ("map.count=" + map.Count);

		if (cube == map [map.Count - 3]) {
			addPath ("slope", getNextPos(map[map.Count - 1]));
		}

		//destroy over 10
		for(int i=0; i< map.Count - maxMesh; i++){
			GameObject gFbx = map[i];
			map.RemoveAt(i);
			Destroy(gFbx.transform.parent.gameObject);
		}
	}

	Vector3 getNextPos(GameObject cube){
		Vector3 pos = cube.transform.position;
		switch (cube.name) {
			case "cube":
				pos.z += 1.5f;
			break;
			case "slope":
				pos.y += -0.5f;
				pos.z += 1.5f;
			break;
		}
		return pos;
	}

	void Update(){
		if (startCountDownSeconds > 0) {
			return;
		} else {
			if(notStart){
				startWhistle.Play();
				notStart = false;
				startCountDownLabel.gameObject.SetActive(false);
				canMove = true;
			}
		}

		camera.transform.position = getCameraPos (sphere.transform.position);

		if (flySeconds >= 0.5f) {
			Debug.Log ("crash");
			Application.LoadLevel ("menu");
		}
	}

}
