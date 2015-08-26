using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Area_endless : Area {
	List<GameObject> map = new List<GameObject>();

	int maxMesh = 8;// 8 cube as a circle
	int meshIndex = 0;
	int lastDirection = 0;
	List<string> blockNameList = new List<string>(){ "cube", "slope"};
	// Use this for initialization
	void Awake(){
		base.Awake ();

		timesLabel.gameObject.SetActive (false);
		//at least 3 gameObject
		addPath ("cube", new Vector3(0, -0.2f, 0), Vector3.zero);
		addPath ("cube", new Vector3(0, -0.3f,1.5f), Vector3.zero);
		addPath ("cube", new Vector3(0, -0.35f,3.0f), Vector3.zero);
		meshIndex = 3;
	}

	void Start () {
		base.Start();
	}

	protected override void setPlayerInt(){
		PlayerPrefs.SetInt ("Area2", 1);
	}

	void addPath(string pathName, Vector3 pos, Vector3 rotate){
		//build cube
		GameObject mainFbx = Instantiate (Resources.Load ("mapFactory/" + pathName)) as GameObject; 
		GameObject gFbx = mainFbx.transform.Find (pathName).gameObject;

		Debug.Log (gFbx);
		mainFbx.transform.parent = this.transform;
		gFbx.transform.localScale = Vector3.one * 50;
		gFbx.transform.position = pos;
		gFbx.transform.Rotate(rotate);

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
			Destroy(gFbx.transform.parent.gameObject);
		}
	}

	void addRandomPath(GameObject lastBlock){
		Vector3 pos = lastBlock.transform.position;
		Debug.Log ("lastBlock pos="+ pos);
		Vector3 rotate = lastBlock.transform.rotation.eulerAngles;
		int blockIndex = Random.Range (0, blockNameList.Count);
		Debug.Log ("blockIndex="+ blockIndex);
		string curBlockName = blockNameList [blockIndex];


		switch (lastBlock.name) {
			case "cube":
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
			case "slope":
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

		if (flySeconds >= 0.8f) {
			Debug.Log ("crash");
			Application.LoadLevel ("menu");
		}
	}

}
