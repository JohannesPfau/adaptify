using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }
	}
}
