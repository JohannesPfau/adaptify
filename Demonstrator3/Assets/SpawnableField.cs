using UnityEngine;
using System.Collections;
using Cubiquity;
using System.Collections.Generic;

public class SpawnableField : Spawnable
{    
    public SpawnableField(int locX, int locY, int locZ, ColoredCubesVolume ccv, GameObject templatePrefab)
    {
        x = locX;
        y = locY - 1;
        z = locZ;
        data = ccv.data;
        coloredCubesVolume = ccv;
        occXpos = 7;
        occXneg = 8;
        occZpos = 7;
        occZneg = 8;
        description = "Feld";
        template = templatePrefab;

        spawn();

    }
    override public void spawn()
    {
        if (y == 0)
        {
            Debug.Log("Warning: Trying to spawn Field on water.");
            y++;
            //return;
        }

        //for (int i = 0; i < 4; i++)
        //    for (int j = 0; j < 4; j++)
        //        for (int k = 0; k < 4; k++)
        //            data.SetVoxel(x + i - 2, y + 1 + j, z + k - 2, new QuantizedColor(95, 62, 2, 255));

        removeObstacles();

        gameObject = Instantiate(template, new Vector3(x - 21, y, z - 16), Quaternion.identity);
        //gameObject.transform.Rotate(new Vector3(0, Random.Range(-20, 20), 0));
    }

    override public void explode()
    {
        float nahrungGesammelt = 0.0f;
        foreach (GameObject go in gameObject.GetComponent<GrainFieldGrowth>().grains)
        {
            nahrungGesammelt += (go.transform.localPosition.y + 3.5f);
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, -3.5f, go.transform.localPosition.z);

            // "explode"-effect for each grain
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
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            cube.transform.parent = coloredCubesVolume.transform;
            cube.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y + 7.5f, go.transform.position.z);
            cube.transform.localRotation = Quaternion.identity;
            cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            cube.GetComponent<Renderer>().material = fakeVoxelMaterial;
            QuantizedColor color = new QuantizedColor(255,255,0,255);
            cube.GetComponent<Renderer>().material.SetColor("_CubeColor", (Color32)color);
            cube.GetComponent<Renderer>().material.SetVector("_CubePosition", new Vector4(go.transform.position.x, go.transform.position.y + 7.5f, go.transform.position.z, 0.0f));

            Vector3 pos = new Vector3(x, y, z);
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
        nahrungGesammelt = nahrungGesammelt / 12.25f;
        //GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().addNahrung((int)nahrungGesammelt);
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().addNahrung(90);
        GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().fieldworkDone();
        gameObject.GetComponent<GrainFieldGrowth>().completion = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().timeReq_Rumpfbeuge / GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().fieldworkRequired;
    }

    public int nahrungsgehalt()
    {
        GameObject go = gameObject;
        GrainFieldGrowth gfg = gameObject.GetComponent<GrainFieldGrowth>();
        int nahrung = gfg.nahrungsgehalt();
        return nahrung;
    }
}
