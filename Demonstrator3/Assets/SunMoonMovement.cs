using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubiquity;

public class SunMoonMovement : MonoBehaviour {

    public bool isSun;

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        // day:
        // t=0: x = -57.5, y = 71.5
        // t=90: x = 0, y = 85
        // t=180: x = 57.5, y = 71.5
        //night:
        // t=0: x = -57.5, y = 71.5
        // t=-90: x = 0, y = 85
        // t=-180: x = 57.5, y = 71.5
        if (isSun)
        {
            if(GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay)
            {
                float newX = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime / 180;
                newX = (newX * 115) - 57.5f;
                float newY = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime;
                if (newY >= 90)
                    newY = 180 - newY;

                newY = (newY / 90 * 13.5f) + 71.5f;

                transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
            }
            else
                transform.localPosition = new Vector3(1000f, 1000f, transform.localPosition.z);
        }
        else if (!isSun)
        {
            if(!GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay)
            {
                float newX = -GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime / 180;
                newX = (newX * 115) - 57.5f;
                float newY = -GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().daytime;
                if(newY >= 90)
                    newY = 180 - newY;

                newY = (newY / 90 * 13.5f) + 71.5f;

                transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
            }
            else
                transform.localPosition = new Vector3(1000f, 1000f, transform.localPosition.z);
        }
    }
}
