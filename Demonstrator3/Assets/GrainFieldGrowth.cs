using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrainFieldGrowth : MonoBehaviour {
    public ArrayList grains;
    public float growthRate = 0.00005f;
    public float completion;

	// Use this for initialization
	void Start () {
        grains = new ArrayList();
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in ts)
        {
            if (child.tag.Equals("Grain"))
            {
                grains.Add(child.gameObject);
            }
        }
        completion = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().timeReq_Rumpfbeuge / GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().fieldworkRequired;
    }
	
	// Update is called once per frame
	void Update () {
        //if(GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().waterRained > 0)
        if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isRaining && completion > 0)
        {
            completion -= Time.deltaTime;
            //GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().regenBuffer -= Time.deltaTime;
            int i = 0;
            foreach(GameObject go in grains)
            {
                float growth = Random.Range(0.0f, 0.5f); 
                if(i % 2 == 1)
                {
                    growth -= 0.2f;
                    if (growth < 0)
                        growth = 0;
                }
                growth = growth * Time.deltaTime * growthRate * GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().timeDone_Rumpfbeuge;
                if((i % 2 == 0 && go.transform.localPosition.y + growth < 1) || (i % 2 == 1 && go.transform.localPosition.y + growth < 0.5f))
                    go.transform.Translate(0, growth,0);
                i++;
            }
        }
	}

    public int nahrungsgehalt()
    {
        float nahrung = 0;
        /*foreach (GameObject go in grains)
        {
            nahrung += (go.transform.localPosition.y + 3.5f);
        }

        nahrung = nahrung / 3.675f;*/
        if (completion <= 0)
            nahrung = 90;

        return (int)nahrung;
    }

}
