using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableSilo : Spawnable {

    public SpawnableSilo(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 11;
        occXneg = 3;
        occZpos = 6;
        occZneg = 6;
        description = "Silo";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn Silo on water.");
            //return;
        }

        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 37, y, z - 4), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0,Random.Range(-20,20),0));
    }

    override public void explode()
    {

    }
}
