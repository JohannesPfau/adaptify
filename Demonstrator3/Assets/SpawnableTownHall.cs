using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableTownHall : Spawnable
{
    public static int _occXpos = 16;
    public static int _occXneg = 14;
    public static int _occZpos = 20;
    public static int _occZneg = 20;

    public SpawnableTownHall(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 16;
        occXneg = 14;
        occZpos = 20;
        occZneg = 20;
        description = "Rathaus";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn TownHall on water.");
            return;
        }
        //make ground
        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - occXneg + 1, y, z - occXneg + 1), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0, Random.Range(-20, 20), 0));
    }

    override public void explode()
    {

    }
}
