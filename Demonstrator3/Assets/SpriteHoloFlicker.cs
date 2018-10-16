using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHoloFlicker : MonoBehaviour {
    public float alpha;
    public float multiplicator;
    bool inc;
    int dir;

	// Use this for initialization
	void Start () {
        alpha = 160;
        inc = false;
    }
	
	// Update is called once per frame
	void Update () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.enabled == false)
            return;

        if (alpha >= 160 && inc)
            inc = false;
        if (alpha <= 100 && !inc)
            inc = true;

        if (inc)
            dir = 1;
        else
            dir = -1;
        
        alpha += dir * Time.deltaTime * multiplicator;

        if (alpha > 255)
            alpha = 255;
        if (alpha < 0)
            alpha = 0;
        sr.color = new Color(1, 1, 1, (alpha / 255));
	}
}
