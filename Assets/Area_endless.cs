﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area_endless : Area {
	[SerializeField]
	UILabel scoreLabel;
	
	List<GameObject> map = new List<GameObject>();

	int maxMesh = 8;// 8 cube as a circle
	int meshIndex = 0;
	int lastDirection = 0;
	List<string> blockNameList = new List<string>(){ "cube", "slope"};

	int score = 0;
	// Use this for initialization
	new void Awake(){
		base.Awake ();

		timesLabel.gameObject.SetActive (false);
		//at least 3 gameObject
		addPath ("cube", new Vector3(0, -0.25f, 0), Vector3.zero);
		addPath ("cube", new Vector3(0, -0.3f,1.5f), Vector3.zero);
		addPath ("cube", new Vector3(0, -0.35f,3.0f), Vector3.zero);
		meshIndex = 3;
		isEndless = true;
	}

	new void Start () {
		base.Start();
	}

	void addPath(string pathName, Vector3 pos, Vector3 rotate){
		//build cube
		GameObject gFbx = Instantiate (Resources.Load ("mapFactory/" + pathName)) as GameObject; 

		gFbx.transform.parent = this.transform;
	
		gFbx.transform.localScale = Vector3.one * 50;
		gFbx.transform.position = pos;
		gFbx.transform.localRotation = Quaternion.Euler(rotate.x, rotate.y, rotate.z); 

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
			addRandomPath(map[map.Count - 1]);
		}

		//destroy over maxMesh
		for(int i=0; i< map.Count - maxMesh; i++){
			GameObject gFbx = map[i];
			map.RemoveAt(i);
			Destroy(gFbx);
		}
	}

	void addRandomPath(GameObject lastBlock){
		updateScore ();
		Vector3 pos = lastBlock.transform.position;
		Debug.Log ("lastBlock pos="+ pos + "lastBlock.name=" + lastBlock.name);
		Vector3 rotate = lastBlock.transform.rotation.eulerAngles;
		int blockIndex = Random.Range (0, blockNameList.Count);
		Debug.Log ("blockIndex="+ blockIndex);
		string curBlockName = blockNameList [blockIndex];


		switch (lastBlock.name) {
		case "cube(Clone)":
			{
				int direction = Random.Range(0,3);
				if(lastDirection != 0 && direction != 0){
					while(direction == lastDirection){
						direction = Random.Range(0,3);
					}
				}
				
				lastDirection = direction;
				Debug.Log ("direction="+ direction + "(int)rotate.y" + (int)rotate.y);
				if(direction == 0){//go straight
					switch((int)rotate.y){
						case 0:
							pos.z +=1.5f;
							break;
						case 90:
							pos.x += 1.5f;
							break;
						case 180:
							pos.z -=1.5f;
							break;
						case 270:
							pos.x -=1.5f;
							break;
					}

				}else if(direction == 1){ //turn right
					rotate.y += 90;
					rotate.y %= 360;
				}else if(direction == 2){//turn left
					
					switch((int)rotate.y){
					case 0:
						pos.x -= 1.5f;
						pos.z += 1.5f;
						break;
					case 90:
						pos.x += 1.5f;
						pos.z += 1.5f;
						break;
					case 180:
						pos.x += 1.5f;
						pos.z -= 1.5f;
						break;
					case 270:
						pos.x -= 1.5f;
						pos.z -= 1.5f;
						break;

					}
					rotate.y -= 90;
					rotate.y %= 360;
				}
			}			
				break;
			case "slope(Clone)":
				pos.y -= 0.5f;

				switch((int)rotate.y){
					case 0:
						pos.z += 1.5f;
						break;
					case 90:
						pos.x += 1.5f;
						break;
					case 180:
						pos.z -= 1.5f;
						break;
					case 270:
						pos.x -= 1.5f;
						break;
					
				}

			break;
		}
		addPath (curBlockName, pos, rotate);
	}

	void Update(){
		
		camera.transform.position = getCameraPos (sphere.transform.position);
		if (notStart) {
			return;
		}


		if (flySeconds >= 0.8f) {
			Debug.Log ("crash");
			Application.LoadLevel ("menu");
		}
	}
	 
	void updateScore(){
		score++;
		scoreLabel.text = score.ToString ();
	}

	protected override void setPlayerInt(){
//		PlayerPrefs.SetInt ("Area1", 1);
	}

	protected override void OnDragging(DragInfo dragInfo){
		//Debug.Log (dragInfo.pos);
		if (!canMove) {
			return;
		}
		if (lastPos == Vector2.zero) {
			lastPos = dragInfo.pos;
		} else {
			Vector2 curPos = dragInfo.pos;
			Vector2 tmp = curPos - lastPos;

			// add force
			Vector3 force = new Vector3 (-tmp.x, 0, -tmp.x) + new Vector3 (tmp.y, 0, -tmp.y);
			force.Normalize ();
			force *= forceTime * sphere.rigidbody.mass;
			//			sphere.rigidbody.velocity = sphere.rigidbody.velocity * 0.01f;
			Vector3 pos = sphere.transform.position + new Vector3 (0.0f, sphere.transform.localScale.x * sphere.GetComponent<SphereCollider> ().radius * 1.5f, 0.0f);
			sphere.rigidbody.AddForceAtPosition (force, pos);
		}
	}
}
