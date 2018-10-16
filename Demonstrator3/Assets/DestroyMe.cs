using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

    public int afterXSeconds;
    float initialTime;

	// Use this for initialization
	void Start () {
        initialTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > initialTime + afterXSeconds)
            Destroy(gameObject);
	}
}
