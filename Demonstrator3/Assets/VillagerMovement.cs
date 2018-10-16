using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerMovement : MonoBehaviour {

    Animator animator;
    float v;
    float h;
    public float run;
    
    float actiontime;
    float lastTime;
    int targetX;
    int targetZ;
    Spawnable target;
    public int homeX;
    public int homeZ;
    bool isRunning;
    VillagerBehavior behavior;
    public GameObject targetVillager;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        actiontime = 0f;
        lastTime = Time.time;
        run = 0f;
        Spawnable tree = (Spawnable)(GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList[1]);
        transform.position.Set(tree.x, 50f, tree.z);
        targetX = -1;
        targetZ = -1;
        target = null;
    }
	
	// Update is called once per frame
	void Update () {
        isRunning = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().VillagersRunning;
        bool commandWoodcutting = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().CommandWoodcutting;

        // Behavior initiation
        if (actiontime <= Time.time - lastTime) 
        {
            float actionRng = Random.Range(0f, 100f);
            if (actionRng <= 30)     // TODO: Make dependable on motivation
            {
                behavior = VillagerBehavior.IDLE;
                actiontime = Random.Range(2f, 10f);
                lastTime = Time.time;
                v = 0;
                h = 0;
            }
            else
            {
                if (actionRng <= 60)
                {
                    behavior = VillagerBehavior.RANDOM;
                    actiontime = Random.Range(2f, 10f);
                    lastTime = Time.time;
                    v = 1;
                    h = Random.Range(-0.35f, 0.35f);
                }
                else
                {
                    if (actionRng <= 70)
                    {
                        behavior = VillagerBehavior.FIELDWORKING;
                        actiontime = 60f;
                        lastTime = Time.time;
                        // go to nearest field
                        if (targetX == -1)
                        {
                            float mindist = Mathf.Infinity;
                            Spawnable feld = null;
                            foreach (Spawnable s in GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList)
                            {
                                if (s.description.Equals("Feld"))
                                {
                                    if (((SpawnableField)s).nahrungsgehalt() >= 40)
                                    {
                                        float dist = Mathf.Sqrt(Mathf.Pow(s.x - transform.position.x, 2) + Mathf.Pow(s.z - transform.position.z, 2));
                                        if (dist <= mindist)
                                        {
                                            mindist = dist;
                                            feld = s;
                                        }
                                    }
                                }
                            }
                            if(feld == null)
                            {
                                actiontime = 0f;
                            }
                            else
                            {
                                targetX = feld.x;
                                targetZ = feld.z;
                                target = feld;
                                v = 1;
                                GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().villagerApproachingField(target.gameObject);
                            }
                        }
                    }
                    else
                    {
                        if (actionRng <= 80)
                        {
                            behavior = VillagerBehavior.HOME;
                            transform.LookAt(new Vector3(homeX, transform.position.y, homeZ));
                            actiontime = Random.Range(5f, 20f);
                            lastTime = Time.time;
                            v = 1;
                        }
                        else
                        {
                            if (actionRng <= 90)
                            {
                                behavior = VillagerBehavior.SOCIAL;
                                targetVillager = null;
                                actiontime = 0f;
                                lastTime = Time.time;
                                // search for idleing guy to talk to
                                foreach (GameObject go in GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().VillagersList)
                                {
                                    if (go.GetComponent<VillagerMovement>().behavior == VillagerBehavior.IDLE)
                                    {
                                        targetVillager = go;
                                        actiontime = 60f;
                                        lastTime = Time.time;
                                        go.GetComponent<VillagerMovement>().targetVillager = transform.gameObject;
                                        go.GetComponent<VillagerMovement>().actiontime = 60f;
                                        go.GetComponent<VillagerMovement>().lastTime = Time.time;
                                        go.GetComponent<VillagerMovement>().behavior = VillagerBehavior.SOCIAL;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (actionRng <= 100)
                                {
                                    behavior = VillagerBehavior.WOODCUTTING;
                                    actiontime = 60f;
                                    lastTime = Time.time;
                                    // go to nearest tree
                                    if (targetX == -1)
                                    {
                                        float mindist = Mathf.Infinity;
                                        Spawnable tree = null;
                                        foreach (Spawnable s in GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList)
                                        {
                                            if (s.description.Equals("Baum"))
                                            {
                                                //Debug.Log(s.x + " " + s.y);
                                                float dist = Mathf.Sqrt(Mathf.Pow(s.x - transform.position.x, 2) + Mathf.Pow(s.z - transform.position.z, 2));
                                                if (dist <= mindist)
                                                {
                                                    mindist = dist;
                                                    tree = s;
                                                }
                                            }
                                        }
                                        targetX = tree.x;
                                        targetZ = tree.z;
                                        target = tree;
                                        v = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // COMMAND: Woodcutting
        if(commandWoodcutting && behavior != VillagerBehavior.WOODCUTTING)
        {
            behavior = VillagerBehavior.WOODCUTTING;
            actiontime = 60f;
            lastTime = Time.time;
            // go to nearest tree
            if (targetX == -1)
            {
                float mindist = Mathf.Infinity;
                Spawnable tree = null;
                foreach (Spawnable s in GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList)
                {
                    if (s.description.Equals("Baum"))
                    {
                        //Debug.Log(s.x + " " + s.y);
                        float dist = Mathf.Sqrt(Mathf.Pow(s.x - transform.position.x, 2) + Mathf.Pow(s.z - transform.position.z, 2));
                        if (dist <= mindist)
                        {
                            mindist = dist;
                            tree = s;
                        }
                    }
                }
                targetX = tree.x;
                targetZ = tree.z;
                target = tree;
                v = 1;
            }
        }
    

        // Behavior check
        if(behavior == VillagerBehavior.WOODCUTTING)
        {
            if (target == null || !GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList.Contains(target))
            {
                targetX = -1;
                targetZ = -1;
                actiontime = 0f;
            }
            else
            {
                transform.LookAt(new Vector3(targetX, transform.position.y, targetZ));
                // Timber!:
                if (Mathf.Sqrt(Mathf.Pow(targetX - transform.position.x, 2) + Mathf.Pow(targetZ - transform.position.z, 2)) <= 5)
                {
                    targetX = -1;
                    targetZ = -1;
                    target.explode();
                    GameObject.Find("AudioController_TreeChopped").GetComponent<AudioSource>().Play();
                    GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList.Remove(target);
                    GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().mapExerciseToWood(this);
                    Destroy(target);
                    actiontime = 0f;
                }
            }
        }
        if(behavior == VillagerBehavior.SOCIAL)
        {
            if(targetVillager == null)
            {
                targetX = -1;
                targetZ = -1;
                actiontime = 0f;
            }
            else
            {
                transform.LookAt(new Vector3(targetVillager.transform.position.x, transform.position.y, targetVillager.transform.position.z));
                v = 1f;

                // Meet & Talk:
                if (Mathf.Sqrt(Mathf.Pow(targetVillager.transform.position.x - transform.position.x, 2) + Mathf.Pow(targetVillager.transform.position.z - transform.position.z, 2)) <= 3)
                {
                    behavior = VillagerBehavior.TALKING;
                    targetVillager.GetComponent<VillagerMovement>().behavior = VillagerBehavior.TALKING;
                    actiontime = Random.Range(5f,15f);
                    lastTime = Time.time;
                    targetVillager.GetComponent<VillagerMovement>().actiontime = actiontime;
                    targetVillager.GetComponent<VillagerMovement>().lastTime = Time.time;
                    v = 0f;
                    targetVillager.GetComponent<VillagerMovement>().v = 0f;
                }
            }
        }

        if (behavior == VillagerBehavior.FIELDWORKING)
        {
            if (target == null || !GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().SpawnableObjectList.Contains(target) || (target is SpawnableField && ((SpawnableField)target).nahrungsgehalt() < 40))
            {
                targetX = -1;
                targetZ = -1;
                actiontime = 0f;
            }
            else
            {
                transform.LookAt(new Vector3(targetX, transform.position.y, targetZ));
                // empty the field:
                if (Mathf.Sqrt(Mathf.Pow(targetX - transform.position.x, 2) + Mathf.Pow(targetZ - transform.position.z, 2)) <= 3)
                {
                    targetX = -1;
                    targetZ = -1;
                    GameObject.Find("AudioController_TreeChopped").GetComponent<AudioSource>().Play();
                    target.explode();
                    actiontime = 0f;
                }
            }
        }
        if (behavior == VillagerBehavior.GLOBALMEETING)
        {
            transform.LookAt(new Vector3(targetX, transform.position.y, targetZ));
            // meeting:
            if (Mathf.Sqrt(Mathf.Pow(targetX - transform.position.x, 2) + Mathf.Pow(targetZ - transform.position.z, 2)) <= 11)
            {
                targetX = -1;
                targetZ = -1;
                target = null;
                
                behavior = VillagerBehavior.IDLE;
                actiontime = Random.Range(20f, 30f);
                lastTime = Time.time;
                v = 0;
                h = 0;
            }
        }

        // Behavior override: Night time?
        if (!GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isDay || GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isRaining || GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().amountMunterkeit <= 0)
        {
            transform.LookAt(new Vector3(homeX, transform.position.y, homeZ));
            v = 1;
            if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().isRaining)
                isRunning = true;
        }

        // Behavior override: Global meeting
        if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().globalMeetingDorfzentrum)
        {
            targetX = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().townCenterX + 2;
            targetZ = GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().townCenterZ - 1;
            target = null;
            behavior = VillagerBehavior.GLOBALMEETING;
            actiontime = 60f;
            lastTime = Time.time;
        }

    }

    void FixedUpdate()
    {
        animator.SetFloat("Walk", v);
        //animator.SetFloat("Run", run);
        animator.SetBool("isRunning", isRunning);
        animator.SetFloat("Turn", h);
    }
    

    enum VillagerBehavior
    {
        IDLE,
        RANDOM,
        HOME,
        WOODCUTTING,
        SOCIAL,
        TALKING,
        FIELDWORKING,
        GLOBALMEETING
    };
}
