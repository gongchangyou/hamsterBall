using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class map : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
		//获取MeshRender;
		MeshRenderer[] meshRenders = GetComponentsInChildren<MeshRenderer>();
		
		//材质;
		List<Material> mats = new List<Material> ();
		for (int i = 0; i < meshRenders.Length; i++)
		{
			for(int j=0; j<meshRenders[i].sharedMaterials.Length; ++j){
				mats.Add( meshRenders[i].sharedMaterials[j] );
			}
		}
		
		//合并Mesh;
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

		List<CombineInstance> combine = new List<CombineInstance> ();

		int k = 0;
		for (int i = 0; i < meshFilters.Length; i++)
		{
			MeshFilter filter = meshFilters[i];
			for(int j = 0; j<filter.sharedMesh.subMeshCount; ++j){
				CombineInstance combineInstance = new CombineInstance();
				combineInstance.mesh = filter.sharedMesh;
				combineInstance.subMeshIndex = j;
				combineInstance.transform = meshFilters[i].transform.localToWorldMatrix;
				combine.Add(combineInstance);
				++k;
			}
			filter.gameObject.SetActive(false);
		}

		
		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.mesh = new Mesh();
		mf.mesh.CombineMeshes(combine.ToArray(), true);
		gameObject.SetActive(true);
		mr.sharedMaterials = mats.ToArray();
		mr.materials = mats.ToArray();;

		MeshCollider mc = gameObject.AddComponent<MeshCollider> ();
		mc.sharedMesh = mf.sharedMesh;
		mc.sharedMaterial = Resources.Load ("pm") as PhysicMaterial;
//		mc.sharedMaterial = pm;
*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
