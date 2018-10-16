using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableChurch : Spawnable
{

    public SpawnableChurch(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 15;
        occXneg = 15;
        occZpos = 10;
        occZneg = 14;
        description = "Kirche";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn church on water.");
            //return;
        }

        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 52, y, z - 9), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0,Random.Range(-20,20),0));
    }

    override public void explode()
    {

    }
}
