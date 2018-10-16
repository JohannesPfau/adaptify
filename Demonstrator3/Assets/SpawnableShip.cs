using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableShip : Spawnable
{

    public SpawnableShip(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 1;
        occXneg = 1;
        occZpos = 1;
        occZneg = 1;
        description = "Schiff";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        //for (int i = 0; i < 4; i++)
        //    for (int j = 0; j < 4; j++)
        //        for (int k = 0; k < 4; k++)
        //            data.SetVoxel(x + i - 2, y + 1 + j, z + k - 2, new QuantizedColor(95, 62, 2, 255));

        //removeObstacles();

        gameObject = Instantiate(template, new Vector3(x , y, z), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0,Random.Range(-20,20),0));
        gameObject.GetComponent<rotateAroundCenter>().center = GameObject.Find("GlobalCenter");
    }

    override public void explode()
    {

    }
}
