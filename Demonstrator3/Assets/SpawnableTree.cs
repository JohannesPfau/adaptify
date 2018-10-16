using UnityEngine;
using System.Collections;
using Cubiquity;

public class SpawnableTree : Spawnable
{
    public SpawnableTree(int locX, int locY, int locZ, ColoredCubesVolume ccv)
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
        description = "Baum";

        spawn();
        
    }

    public override void spawn()
    {
        if (y == 0) return;
        data.SetVoxel(x, y + 1, z, new QuantizedColor(95, 62, 2, 255));
        data.SetVoxel(x, y + 2, z, new QuantizedColor(95, 62, 2, 255));
        data.SetVoxel(x, y + 3, z, new QuantizedColor(95, 62, 2, 255));
        data.SetVoxel(x, y + 4, z, new QuantizedColor(95, 62, 2, 255));

        data.SetVoxel(x, y + 5, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 5, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 5, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 5, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 5, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 5, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 5, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 5, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 5, z - 1, new QuantizedColor(43, 81, 0, 255));

        data.SetVoxel(x, y + 6, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 6, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 6, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 6, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 6, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 6, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 6, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 6, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 6, z - 1, new QuantizedColor(43, 81, 0, 255));

        data.SetVoxel(x, y + 7, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 7, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 7, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 7, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 7, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 7, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 7, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 7, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 7, z - 1, new QuantizedColor(43, 81, 0, 255));

        data.SetVoxel(x, y + 8, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 8, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 8, z, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 8, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 8, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 8, z + 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x, y + 8, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x - 1, y + 8, z - 1, new QuantizedColor(43, 81, 0, 255));
        data.SetVoxel(x + 1, y + 8, z - 1, new QuantizedColor(43, 81, 0, 255));

        int r = Random.Range(0, 4);
        if (r == 0)
        {
            data.SetVoxel(x + 2, y + 7, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 2, y + 7, z + 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 2, y + 6, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 2, y + 6, z + 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x - 2, y + 7, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x - 2, y + 7, z + 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x - 2, y + 6, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x - 2, y + 6, z + 1, new QuantizedColor(43, 81, 0, 255));
        }
        if (r == 1)
        {
            data.SetVoxel(x, y + 7, z + 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 7, z + 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 6, z + 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 6, z + 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 7, z - 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 7, z - 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 6, z - 2, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 6, z - 2, new QuantizedColor(43, 81, 0, 255));
        }
        if (r == 2)
        {
            data.SetVoxel(x + 1, y + 9, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x - 1, y + 9, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 9, z + 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 9, z - 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 10, z, new QuantizedColor(43, 81, 0, 255));

        }
        if (r == 3)
        {
            data.SetVoxel(x, y + 9, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 9, z, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x, y + 9, z + 1, new QuantizedColor(43, 81, 0, 255));
            data.SetVoxel(x + 1, y + 9, z + 1, new QuantizedColor(43, 81, 0, 255));
        }

        r = Random.Range(0, 4);
        if (r == 0)
            data.SetVoxel(x, y + 9, z, new QuantizedColor(43, 81, 0, 255));
    }

    override public void explode()
    {
        int range = 6;
        DestroyVoxels(x, y+6, z, range);
    }
}
