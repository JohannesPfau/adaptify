using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliWingsSpin : MonoBehaviour {
    public bool spinning;

	// Use this for initialization
	void Start () {
        spinning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(spinning)
        {
            transform.Rotate(new Vector3(0f, 8f, 0f));
        }
	}
}
