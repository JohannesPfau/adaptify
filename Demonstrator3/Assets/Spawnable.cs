using UnityEngine;
using System.Collections;
using Cubiquity;
using System.Collections.Generic;

public abstract class Spawnable : ScriptableObject {
    public int x;
    public int y;
    public int z;
    protected byte[,] pointcloud;
    public ColoredCubesVolume coloredCubesVolume;
    public ColoredCubesVolumeData data;
    public int occXpos;
    public int occXneg;
    public int occZpos;
    public int occZneg;
    public string description;
    public GameObject gameObject;
    public GameObject template;


    public abstract void spawn();
    public abstract void explode();

    public void DestroyVoxels(int xPos, int yPos, int zPos, int range)
    {
        // Set up a material which we will apply to the cubes which we spawn to replace destroyed voxels.
        Material fakeVoxelMaterial = Resources.Load("Materials/FakeColoredCubes", typeof(Material)) as Material;
        Texture diffuseMap = coloredCubesVolume.GetComponent<ColoredCubesVolumeRenderer>().material.GetTexture("_DiffuseMap");
        if (diffuseMap != null)
        {
            List<string> keywords = new List<string> { "DIFFUSE_TEXTURE_ON" };
            fakeVoxelMaterial.shaderKeywords = keywords.ToArray();
            fakeVoxelMaterial.SetTexture("_DiffuseMap", diffuseMap);
        }
        fakeVoxelMaterial.SetTexture("_NormalMap", coloredCubesVolume.GetComponent<ColoredCubesVolumeRenderer>().material.GetTexture("_NormalMap"));
        fakeVoxelMaterial.SetFloat("_NoiseStrength", coloredCubesVolume.GetComponent<ColoredCubesVolumeRenderer>().material.GetFloat("_NoiseStrength"));

        // Initialise outside the loop, but we'll use it later.
        Vector3 pos = new Vector3(xPos, yPos, zPos);
        int rangeSquared = range * range;

        // Later on we will be deleting some voxels, but we'll also be looking at the neighbours of a voxel.
        // This interaction can have some unexpected results, so it is best to first make a list of voxels we
        // want to delete and then delete them later in a separate pass.
        List<Vector3i> voxelsToDelete = new List<Vector3i>();

        // Iterage over every voxel in a cubic region defined by the received position (the center) and
        // the range. It is quite possible that this will be hundreds or even thousands of voxels.
        for (int z = zPos - range; z < zPos + range; z++)
        {
            for (int y = yPos - range; y < yPos + range; y++)
            {
                for (int x = xPos - range; x < xPos + range; x++)
                {
                    // Compute the distance from the current voxel to the center of our explosion.
                    int xDistance = x - xPos;
                    int yDistance = y - yPos;
                    int zDistance = z - zPos;

                    // Working with squared distances avoids costly square root operations.
                    int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                    // We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
                    // we only further consider voxels which are within the required range of our explosion center. 
                    // The corners of the cubic region we are iterating over will fail the following test.
                    if (distSquared < rangeSquared)
                    {
                        // Get the current color of the voxel
                        QuantizedColor color = coloredCubesVolume.data.GetVoxel(x, y, z);

                        // Check the alpha to determine whether the voxel is visible. 
                        if (color.alpha > 127)
                        {
                            Vector3i voxel = new Vector3i(x, y, z);
                            voxelsToDelete.Add(voxel);

                            if (IsSurfaceVoxel(x, y, z))
                            {
                                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                cube.AddComponent<Rigidbody>();
                                cube.transform.parent = coloredCubesVolume.transform;
                                cube.transform.localPosition = new Vector3(x, y, z);
                                cube.transform.localRotation = Quaternion.identity;
                                cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                                cube.GetComponent<Renderer>().material = fakeVoxelMaterial;
                                cube.GetComponent<Renderer>().material.SetColor("_CubeColor", (Color32)color);
                                cube.GetComponent<Renderer>().material.SetVector("_CubePosition", new Vector4(x, y, z, 0.0f));

                                Vector3 explosionForce = cube.transform.position - pos;

                                // These are basically random values found through experimentation.
                                // They just add a bit of twist as the cubes explode which looks nice
                                float xTorque = (x * 1436523.4f) % 56.0f;
                                float yTorque = (y * 56143.4f) % 43.0f;
                                float zTorque = (z * 22873.4f) % 38.0f;

                                Vector3 up = new Vector3(0.0f, 2.0f, 0.0f);

                                cube.GetComponent<Rigidbody>().AddTorque(xTorque, yTorque, zTorque);
                                cube.GetComponent<Rigidbody>().AddForce((explosionForce.normalized + up) * 100.0f);

                                // Cubes are just a temporary visual effect, and we delete them after a few seconds.
                                float lifeTime = Random.Range(8.0f, 12.0f);
                                Destroy(cube, lifeTime);
                            }
                        }
                    }
                }
            }
        }

        foreach (Vector3i voxel in voxelsToDelete) // Loop through List with foreach
        {
            coloredCubesVolume.data.SetVoxel(voxel.x, voxel.y, voxel.z, new QuantizedColor(0, 0, 0, 0));
        }
    }

    public bool IsSurfaceVoxel(int x, int y, int z)
    {
        QuantizedColor quantizedColor;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x, y, z);
        if (quantizedColor.alpha < 127) return false;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x + 1, y, z);
        if (quantizedColor.alpha < 127) return true;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x - 1, y, z);
        if (quantizedColor.alpha < 127) return true;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x, y + 1, z);
        if (quantizedColor.alpha < 127) return true;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x, y - 1, z);
        if (quantizedColor.alpha < 127) return true;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x, y, z + 1);
        if (quantizedColor.alpha < 127) return true;

        quantizedColor = coloredCubesVolume.data.GetVoxel(x, y, z - 1);
        if (quantizedColor.alpha < 127) return true;

        return false;
    }

    public void removeObstacles()
    {
        int y = this.y;
        if (y <= 1)
            y = 2;
        // ground
        for (int i = x - occXneg; i <= x + occXpos; i++)
            for (int j = z - occZneg; j <= z + occZpos; j++)
            {
                QuantizedColor qc = new QuantizedColor();
                if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().landscapeType == CreateVoxelLandscape.LandscapeType.valley)
                    qc = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().height2QColor(y);
                if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().landscapeType == CreateVoxelLandscape.LandscapeType.island)
                    qc = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().height2QColorIsland(y);
                data.SetVoxel(i, y - 1, j, qc);
            }

        // level adjacent fields
        for (int i = (x - occXneg); i <= x + occXpos; i++)
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField(i, (z - occZneg) - 1, y - 1, 0, -1);

        for (int i = (x - occXneg); i <= x + occXpos; i++)
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField(i, (z + occZpos) + 1, y - 1, 0, +1);

        for (int i = (z - occZneg); i <= z + occZpos; i++)
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x - occXneg) - 1, i, y- 1, -1, 0);

        for (int i = (z - occZneg); i <= z + occZpos; i++)
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x + occXpos) + 1, i, y - 1, +1, 0);

        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x + occXpos) + 1, (z + occZpos) + 1, y - 1, +1, +1);
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x - occXneg) - 1, (z + occZpos) + 1, y - 1, -1, +1);
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x + occXpos) + 1, (z - occZneg) - 1, y - 1, +1, -1);
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().levelField((x - occXneg) - 1, (z - occZneg) - 1, y - 1, -1, -1);


        // delete rest
        for (int i = x - occXneg; i <= x + occXpos; i++)
            for (int j = z - occZneg; j <= z + occZpos; j++)
                for (int k = y; k <= y + 20; k++)
                    data.SetVoxel(i, k, j, new QuantizedColor(0, 0, 0, 0));
    }

    public enum BuildingType
    {
        House,
        TownHall,
        WindMill,
        WindWheel,
        Solar,
        Silo,
        Field,
        WaterTower
    };

}
