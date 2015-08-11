using UnityEngine;
using System.Collections;

public class TriggerSound : MonoBehaviour {

	[SerializeField]
	private AudioSource sound;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		sound.Play ();
	}
}
