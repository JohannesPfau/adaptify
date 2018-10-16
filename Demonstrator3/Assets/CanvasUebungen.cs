using UnityEngine;
using System.Collections;

public class CanvasUebungen : MonoBehaviour {

    public GameObject content;

	// Use this for initialization
	void Start () {
        content.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            content.SetActive(true);
        if(Input.GetKeyUp(KeyCode.Tab))
            content.SetActive(false);

    }
}
