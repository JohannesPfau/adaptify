using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipWaves : MonoBehaviour {

    bool xPos;
    public float maxAngle;
    public float minAngle;
    public float speed = 1;

	// Use this for initialization
	void Start () {
        xPos = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(xPos)
        {
            transform.Rotate(Time.deltaTime * speed, 0, Time.deltaTime * speed);
            if (transform.localRotation.eulerAngles.x > maxAngle && transform.localRotation.eulerAngles.x < 180)
                xPos = false;
        }
        else
        {
            float temp = transform.localRotation.eulerAngles.x;
            if(temp > 180)
                temp -= 360;

            transform.Rotate(-Time.deltaTime * speed, 0, -Time.deltaTime * speed);
            if (temp < minAngle)
                xPos = true;
        }
    }
}
