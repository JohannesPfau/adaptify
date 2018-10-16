using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableWindmill : Spawnable
{

    public SpawnableWindmill(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 13;
        occXneg = 13;
        occZpos = 13;
        occZneg = 13;
        description = "Windmühle";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn windmill on water.");
            //return;
        }
        
        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 3, y + 14, z - 3), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0, Random.Range(-20, 20), 0));
    }

    override public void explode()
    {

    }
}
