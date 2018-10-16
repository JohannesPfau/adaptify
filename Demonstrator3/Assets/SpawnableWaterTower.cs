using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableWaterTower : Spawnable {

    public SpawnableWaterTower(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 6;
        occXneg = 6;
        occZpos = 6;
        occZneg = 6;
        description = "Wasserturm";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn Wasserturm on water.");
            //return;
        }

        //for (int i = 0; i < 4; i++)
        //    for (int j = 0; j < 4; j++)
        //        for (int k = 0; k < 4; k++)
        //            data.SetVoxel(x + i - 2, y + 1 + j, z + k - 2, new QuantizedColor(95, 62, 2, 255));

        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 5 , y , z - 6 ), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0,Random.Range(-20,20),0));
    }

    override public void explode()
    {

    }
}
