using UnityEngine;
using System.Collections;

public class FollowCube : MonoBehaviour {

	public GameObject followMe;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (followMe.transform);
	}
}
