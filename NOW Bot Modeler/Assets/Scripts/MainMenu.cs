using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
