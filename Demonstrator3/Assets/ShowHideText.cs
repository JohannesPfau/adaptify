using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideText : MonoBehaviour {

    public void showText()
    {
        GetComponentInChildren<Text>().enabled = true;
    }

    public void hideText()
    {
        GetComponentInChildren<Text>().enabled = false;
    }
}
