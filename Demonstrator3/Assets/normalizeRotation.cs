using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalizeRotation : MonoBehaviour {

    public float Multiplicator = 10f;
    public float duration = 10f;
    float i_duration;
    bool reversing;
    public string EndFunction;
    public bool HideAfterDisplay;
    public GameObject content;

    // Use this for initialization
    void Start () {
        i_duration = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if(!reversing)
        {

            float deviation = Mathf.Abs(transform.localRotation.eulerAngles.x);
            if (deviation > 5)
            {
                if (transform.localRotation.eulerAngles.x > 0)
                    transform.Rotate(Multiplicator * -Time.deltaTime, 0, 0);
                if (transform.localRotation.eulerAngles.x < 0)
                    transform.Rotate(Multiplicator * Time.deltaTime, 0, 0);
            }
            else
            {
                // wait duration
                i_duration += Time.deltaTime;
                if (i_duration >= duration)
                    reversing = true;
            }
        }
        else
        {
            transform.Rotate(Multiplicator * Time.deltaTime, 0, 0);
            if (Mathf.Abs(transform.localRotation.eulerAngles.x) >= 75)
            {
                reversing = false;
                i_duration = 0;

                if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().MayorCamera != null)
                    GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().MayorCamera.SetActive(false);
                else if (GameObject.Find("MayorCamera") != null)
                    GameObject.Find("MayorCamera").SetActive(false);

                if (HideAfterDisplay)
                    content.SetActive(false);
                if (EndFunction != null && EndFunction.Length > 0)
                    GameObject.Find("Demo3CCV").SendMessage(EndFunction);
            }
        }

	}

    public void resetDuration()
    {
        i_duration = 0;
        reversing = false;
        content.SetActive(true);
    }
}
