using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillWingsSpin : MonoBehaviour {

    public float overrideSpin;
    public float nahrungBuffer;

	// Use this for initialization
	void Start () {
        nahrungBuffer = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if(overrideSpin == 0)
            transform.Rotate(new Vector3(0f,0f, GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().windForce));
        else
            transform.Rotate(new Vector3(0f, 0f, overrideSpin));

        if (overrideSpin == 0)
        {
            nahrungBuffer += GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().windForce * Time.deltaTime;
            if (nahrungBuffer >= 25)
            {
                //GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().addNahrung(1);
                nahrungBuffer = 0;
            }
        }
            
    }
}
