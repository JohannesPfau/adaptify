using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAroundCenter : MonoBehaviour {
    
    public GameObject center;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(center.transform.position, new Vector3(0, -1, 0), Time.deltaTime * 5);
	}
}
