using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToSun : MonoBehaviour {

    public float offset;
    Vector3 targetRotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // t=0: 50°
        // t=90: 0*
        // t=180: -50°
        float angle = 0f;
        if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay)
        {
             angle = ((GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime / 180f * 100f) - 50);
        }
        else
            angle = -(( -(GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime) / 180f * 100f) - 50);
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}
