using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCrane : MonoBehaviour {

    public float rotateMultiplicator = 10f;
    public GameObject Lift;
    float liftYoffset;
    float liftDelta;
    float liftDeltaPart; 
    public Spawnable.BuildingType buildingType;
    int direction; // 0 = init, 1 = right, -1 = left
    bool moving;

    // Use this for initialization
    void Start() {
        liftDelta = Mathf.Abs(Lift.transform.localPosition.y);
        liftDeltaPart = liftDelta / ((GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().reptReq_SeitschrittLinks + GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().reptReq_SeitschrittRechts) / GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().buildingsRequired);
        direction = 0;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        liftYoffset = Lift.transform.localPosition.y;
        Debug.Log(Lift.transform.localPosition.y);
        if (liftYoffset >= 0)
            finishConstruction(true);

        if (moving && direction == 1)
        {
            if((transform.localEulerAngles.y < 30 || transform.localEulerAngles.y > 320))
            {
                // Right
                transform.Rotate(0f, Time.deltaTime * rotateMultiplicator, 0f);
            }
            else
            {
                if (liftYoffset < 0)
                    Lift.transform.Translate(0, liftDeltaPart, 0);
                moving = false;
            }
        }
        if (moving && direction == -1)
        {
            if ((transform.localEulerAngles.y > 330 || transform.localEulerAngles.y < 40))
            {
                // Left
                transform.Rotate(0f, -Time.deltaTime * rotateMultiplicator, 0f);
            }
            else
            {
                if (liftYoffset < 0)
                    Lift.transform.Translate(0, liftDeltaPart, 0);
                moving = false;
            }
        }
    }

    public void finishConstruction(bool sendMessage)
    {
        Lift.transform.localPosition = new Vector3();
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().buildingFinished(buildingType, (int)Lift.transform.position.x, (int)Lift.transform.position.z, sendMessage);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("DeleteMeKran"))
            Destroy(go);
        //Destroy(GameObject.Find("KranParent"));
    }

    public void moveLeft()
    {
        if (!moving && direction != -1)
        {
            moving = true;
            direction = -1;
        }
    }

    public void moveRight()
    {
        if (!moving && direction != 1)
        {
            moving = true;
            direction = 1;
        }
    }

}
