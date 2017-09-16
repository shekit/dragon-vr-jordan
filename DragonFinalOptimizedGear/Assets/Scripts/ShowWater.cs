using UnityEngine;
using System.Collections;

public class ShowWater : MonoBehaviour {

	// Use this for initialization
	void OnBecameInvisible() {
		enabled = false;
	}

	void OnBecameVisible(){
		enabled = true;
	}
}
