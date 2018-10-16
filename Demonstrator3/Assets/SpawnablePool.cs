using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnablePool : Spawnable {

    public SpawnablePool(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 17;
        occXneg = 17;
        occZpos = 11;
        occZneg = 11;
        description = "Freibad";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn Pool on water.");
            //return;
        }

        //for (int i = 0; i < 4; i++)
        //    for (int j = 0; j < 4; j++)
        //        for (int k = 0; k < 4; k++)
        //            data.SetVoxel(x + i - 2, y + 1 + j, z + k - 2, new QuantizedColor(95, 62, 2, 255));

        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 14 , y - 6.9f, z - 9 ), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0,Random.Range(-20,20),0));
    }

    override public void explode()
    {

    }
}
