using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnAtNight : MonoBehaviour {
    public bool inverted;

    public GameObject NightContent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!inverted)
        {
            if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay)
                NightContent.SetActive(false);
            else
                NightContent.SetActive(true);
        }
        else
            if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay)
            NightContent.SetActive(true);
        else
            NightContent.SetActive(false);

    }
}
