using UnityEngine;
using System.Collections;
using Cubiquity;
using UnityEngine.UI;
using System.IO;

public class CreateVoxelLandscape : MonoBehaviour
{
    public int week;
    public bool muteVoiceOver;
    
    public LandscapeType landscapeType;

    public float maxHeight;
    public float noiseMagnitude;
    int maxX = 512;
    int maxZ = 512;
    public int randomseed;
    public float treeMultiplier;
    ColoredCubesVolumeData data;
    ColoredCubesVolume ccv;
    public ArrayList SpawnableObjectList;
    public ArrayList SpawnableBuildingsList;
    public ArrayList VillagersList;
    public GameObject waterDay;
    public GameObject waterNight;
    public GameObject mainCam;
    public GameObject selectionCam;
    public GameObject mainLight;
    public bool isDay;
    public bool isRaining;
    public Material daysky;
    public Material nightsky;
    public Material rainsky;
    bool[,] obstructedMap;
    public int townCenterX;
    public int townCenterZ;
    public GameObject[] RainModes;
    public GameObject RainAudio;
    public GameObject RainParticles;
    public GameObject[] villagerPrefabs;
    public GameObject mayorPrefab;
    public bool VillagersRunning;
    public GameObject templateCastle;
    public GameObject templateHouse;
    public GameObject templateWindmill;
    public GameObject templateField;
    public GameObject templateSilo;
    public GameObject templatePool;
    public GameObject templateSolar;
    public GameObject templateWatertower;
    public GameObject templateWindturbine;
    public GameObject templateShip;
    public GameObject templateChurch;
    public float windForce;
    public float waterRained;
    public int amountRohstoffe;
    public int amountNahrung;
    public float amountMunterkeit;
    bool isSelecting;
    public GameObject walkmapNode;
    public bool globalMeetingDorfzentrum;
    public GameObject MayorCamera;
    Vector3 CameraInitialPosition;
    Vector3 CameraTargetPosition;
    Vector3 CameraStartPosition;
    Vector3 CameraInitialRotation;
    Vector3 CameraStartRotation;
    Vector3 CameraTargetRotation;
    bool CameraLerping;
    bool CameraAway;
    bool CameraBackToStart;
    float lerpProgress;
    float lerpMultiplicator;
    public float daytime;
    public GameObject SunMoonCamera;
    public GameObject TimeLine;
    public GameObject TimeLineNight;
    float[,] islandHeight;
    public GameObject sprechblase;
    bool currentlyUnderConstruction;
    public GameObject CanvasLadebildschirm;
    bool heliMode;
    public bool CommandWoodcutting;
    public bool tutorial_CraneDisplayed;
    string _endFunction;
    public bool holzTutorial;
    public int buildingsRequired_Houses;
    public int buildingsRequired_Fields;
    public int buildingsRequired_Mills;
    public int buildingsRequired_Solar;
    public int buildingsRequired_WindWheel;
    public int buildingsRequired_WaterTower;
    public Spawnable.BuildingType selectionCamLeftEntry;
    public Spawnable.BuildingType selectionCamRightEntry;
    public bool rainTutorialDone;
    public bool aufgabenTutorialDone;
    public bool tutorial_chooseSelection;
    float speechBubbleTimeout;
    bool villagerApproachingFieldDone;
    public bool windTutorialDone;
    public bool tagnachtTutorial;
    public bool firstNightTutorial;
    public bool mission1done;
    public bool allow_boxenImStand;

    // exercise requirements
    public float timeReq_Rumpfbeuge;
    float timeReq_GehenAufDerStelle;
    float timeReq_EinBeinStand;
    int reptReq_EinBeinStandLinks;
    int reptReq_EinBeinStandRechts;
    int reptReq_Rumpfrotation;
    int reptReq_Hueftkreisen;
    float timeReq_BoxenImStand;
    int reptReq_DiagonaleRumpfbeugeLinks;
    int reptReq_DiagonaleRumpfbeugeRechts;
    int reptReq_Rumpfbeuge;
    int reptReq_SeitneigeRechts;
    int reptReq_SeitneigeLinks;
    public int reptReq_SeitschrittRechts;
    public int reptReq_SeitschrittLinks;
    // completed:
    public float timeDone_Rumpfbeuge;
    float timeDone_GehenAufDerStelle;
    float timeDone_EinBeinStand;
    int reptDone_EinBeinStandLinks;
    int reptDone_EinBeinStandRechts;
    int reptDone_Rumpfrotation;
    int reptDone_Hueftkreisen;
    float timeDone_BoxenImStand;
    int reptDone_DiagonaleRumpfbeugeLinks;
    int reptDone_DiagonaleRumpfbeugeRechts;
    int reptDone_Rumpfbeuge;
    int reptDone_SeitneigeRechts;
    int reptDone_Seitneigelinks;
    public int reptDone_SeitschrittRechts;
    public int reptDone_SeitschrittLinks;
    // mission planning:
    float globalProgress;
    int woodNeededTotal;
    int woodCollected;
    public int buildingsRequired;
    public float regenBuffer;
    public int fieldworkRequired;
    float rainSecondsPerRepetition;

    string logger;

    // Use this for initialization
    void Start()
    {
        //CanvasLadebildschirm.SetActive(true);
        randomseed = Random.Range(0, 10000);
        Physics.gravity = Vector3.zero;

        data = transform.gameObject.GetComponent<ColoredCubesVolume>().data;
        ccv = transform.gameObject.GetComponent<ColoredCubesVolume>();
        isDay = true;
        SpawnableObjectList = new ArrayList();
        SpawnableBuildingsList = new ArrayList();
        VillagersList = new ArrayList();
        obstructedMap = new bool[maxX, maxZ];

        if (landscapeType == LandscapeType.valley)
            createValleyLandscape();
        if (landscapeType == LandscapeType.island)
            createIslandLandscape();

        windForce = 0f;
        waterRained = 0f;
        //spawnBorder();
        //buildWalkmap();
        globalMeetingDorfzentrum = false;

        amountRohstoffe = 0;
        addRohstoffe(0);
        addMunterkeit(100);
        addNahrung(10);
        CameraInitialPosition = mainCam.transform.position;
        CameraInitialRotation = mainCam.transform.rotation.eulerAngles;
        CameraLerping = false;
        CameraAway = false;
        daytime = 90f;
        SunMoonCamera.SetActive(true);
        currentlyUnderConstruction = false;
        //welcomeMessage();
        initLadescreen();
        heliMode = false;
        CommandWoodcutting = false;
        tutorial_CraneDisplayed = false;
        _endFunction = "";
        speechBubbleTimeout = 0;
        lerpMultiplicator = 1;

        logger = "";

        initEASYBeginner();
    }

    // Update is called once per frame
    void Update()
    {
        // ZOOM BY DISTANCE
        if (!CameraAway && GameObject.Find("KinectGetDistance").GetComponent<KinectGetDistance>().enabled)
        {
            if (GameObject.Find("KinectGetDistance").GetComponent<KinectGetDistance>()._distance > 0)
            {
                mainCam.transform.position = getAdaptiveCameraPosition();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            /*if (MayorCamera.activeSelf)
                MayorCamera.SetActive(false);
            else
                MayorCamera.SetActive(true);*/
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Villager"))
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 10, go.transform.position.z);
            }

        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            welcomeMessage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Closed_smile;
            MayorCamera.SetActive(true);
            sprechblase.SetActive(true);
            sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
            sprechblase.GetComponent<normalizeRotation>().resetDuration();
            sprechblase.GetComponentInChildren<Text>().text = "Das Gebäude ist fertig!";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Closed_happy;
            MayorCamera.SetActive(true);
            sprechblase.SetActive(true);
            sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
            sprechblase.GetComponent<normalizeRotation>().resetDuration();
            sprechblase.GetComponentInChildren<Text>().text = "Alle Dorfbewohner haben sich für Dich versammelt!";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Closed;
            MayorCamera.SetActive(true);
            sprechblase.SetActive(true);
            sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
            sprechblase.GetComponent<normalizeRotation>().resetDuration();
            sprechblase.GetComponentInChildren<Text>().text = "Deine Dorfbewohner sind müde... Wenn es doch nur Nacht wäre!";
        }


        // Camera Lerp system:
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            MayorCamera.SetActive(true);
            GameObject.Find("TGJ").GetComponent<AudioSource>().Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && !CameraLerping)
        {
            CameraStartPosition = mainCam.transform.position;
            CameraTargetPosition = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.position;
            CameraStartRotation = mainCam.transform.rotation.eulerAngles;
            CameraTargetRotation = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
            lerpProgress = 0f;
            lerpMultiplicator = 0.05f;
            CameraLerping = true;
            mission1done = true;
            globalMeetingDorfzentrum = true;
            Invoke("fadeEndscreen", 20);
        }*/
        if (heliMode)
        {
            if ((float)reptDone_Rumpfrotation / (float)reptReq_Rumpfrotation > GameObject.Find("Heli").transform.localPosition.y / 271)
            {
                GameObject.Find("Heli").GetComponentInChildren<HeliWingsSpin>().spinning = true;
                mainCam.transform.Translate(0, 0, -Time.deltaTime * 10);
                GameObject.Find("Heli").transform.Translate(0, Time.deltaTime * 10, 0);
                /*if (mainCam.transform.position.y >= 300f)
                {
                    CameraLerpToDefault();
                    heliMode = false;
                    Destroy(GameObject.Find("Heli"));

                    GameObject go = spawnMayor(townCenterX - 3, 50, townCenterZ + 15);
                    go.GetComponent<VillagerMovement>().homeX = townCenterX - 3;
                    go.GetComponent<VillagerMovement>().homeZ = townCenterZ + 15;
                    VillagersList.Add(go);
                    MayorCamera = GameObject.Find("MayorCamera");
                    MayorCamera.SetActive(false);
                    //speechbubbleText("Prima! Jetzt können wir anfangen, diese Insel zu bebauen!", "BoxenMessage",false);
                    GameObject.Find("RedArrow_1").GetComponent<RectTransform>().localScale = Vector3.zero;
                    displayMessage("Prima! Jetzt können wir anfangen, diese Insel zu bebauen!", 5, "BoxenMessage");
                }*/
            }
        }
        if (CameraLerping)
        {
            if (lerpMultiplicator == 0f)
                lerpMultiplicator = 1f;

            lerpProgress += Time.deltaTime * lerpMultiplicator;
            if (lerpProgress >= 1)
            {
                lerpProgress = 1f;
                CameraLerping = false;
                lerpMultiplicator = 1f;
                if(CameraBackToStart)
                {
                    CameraBackToStart = false;
                    CameraAway = false;
                }
            }
            mainCam.transform.position = Vector3.Lerp(CameraStartPosition, CameraTargetPosition, lerpProgress);
            mainCam.transform.rotation = Quaternion.Euler(Vector3.Lerp(CameraStartRotation, CameraTargetRotation, lerpProgress));

        }



        if (Input.GetKey(KeyCode.F1))
        {
            // Boxen im Stand: Zusammentrommeln
            globalMeetingDorfzentrum = true;

            //// Boxen im Stand: spawn trees
            //int x = Random.Range(0, maxX);
            //int z = Random.Range(0, maxZ);
            //int y = (int)getTerrainHeight(x, z);
            //spawnTree(x, y, z);
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            globalMeetingDorfzentrum = false;
        }

        if (Input.GetKeyUp(KeyCode.F12))
        {
            // debug: spawn
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnHouse(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnWindmill(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnField(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnSilo(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnSolar(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnPool(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnWaterTower(loc[0], y, loc[1]);

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnWindTurbine(loc[0], y, loc[1]);


            //
            spawnShip();

            //
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnChurch(loc[0], y, loc[1]);
        }

        // Rain
        if (isRaining)
            regenBuffer -= Time.deltaTime;
        if (regenBuffer > 0 && !isRaining)
        {
            int r = Random.Range(0, RainModes.Length - 1);
            RainModes[r].SetActive(true);
            RainAudio.SetActive(true);
            RainParticles.SetActive(true);
            isRaining = true;
            UpdateSkyAndLight();
        }
        if (isRaining && regenBuffer <= 0)
        {
            foreach (GameObject go in RainModes)
                go.SetActive(false);
            RainAudio.SetActive(false);
            RainParticles.SetActive(false);
            isRaining = false;
            UpdateSkyAndLight();
        }

        /*if(isRaining)
        {
            if (waterRained < 4)
                waterRained = waterRained += Time.deltaTime;
        }
        else
        {
            if (waterRained > 0)
                waterRained = waterRained -= Time.deltaTime / 10;
        }*/

        // Hüftkreisen: increase wind force
        if (Input.GetKey(KeyCode.F5))
        {
            if (windForce < 8)
                windForce = windForce += Time.deltaTime;
        }
        else
        {
            if (windForce > 0)
                windForce = windForce -= Time.deltaTime / 10;
        }

        if (isSelecting)
        {
            // Linke Auswahl treffen
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                GameObject.Find("BlackFade").GetComponent<Animator>().Play("LadebildschirmBlackFade");
                Invoke("selectionCam_chooseLeft",0.75f);
            }

            // rechte Auswahl treffen
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                GameObject.Find("BlackFade").GetComponent<Animator>().Play("LadebildschirmBlackFade");
                Invoke("selectionCam_chooseRight", 0.75f);
            }


        }
        if (Input.GetKey(KeyCode.F10))
        {
            addRohstoffe(1);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LadescreenBlackFade();
        }

        // ----------------------- Exercises ---------------------------------- //
        if (Input.GetKey(KeyCode.Keypad0))
        {
            act_GehenAufDerStelle(Time.deltaTime);
            VillagersRunning = true;
        }
        else
            VillagersRunning = false;

        if (mission1done)
        {
            VillagersRunning = true;
            globalMeetingDorfzentrum = true;
            CameraTargetPosition = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.position;
            CameraTargetRotation = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
        }

        // Raise / lower sun:
        //float SunRaiseMultiplicator = 10.0f;
        // Morgen: x = -180 / 0
        // Mittag: x = 115 / 64
        // Nachtanfang: x = 0 / 360
        // Mitternacht: x = / 295

        // bsp: timeReq_EinBeinStand = 20 sek
        // daytime links: 0 bis -90 ; 0 bis 90
        // daytime rechts: 90 bis 180 ; -90 bis -180 
        // also: 90 daytime in 20 sekunden
        // pro sekunde: 4,5 daytime
        if (Input.GetKey(KeyCode.Keypad1))
        {
            act_EinBeinStandLinks(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            act_EinBeinStandRechts(Time.deltaTime);
        }

        // Rumpfrotation:
        if (heliMode && Input.GetKeyDown(KeyCode.Keypad3))
            act_Rumpfrotation();
        // Boxen im Stand:
        if (Input.GetKey(KeyCode.Keypad4))
        {
            act_BoxenImStand(Time.deltaTime);
        }
        else
            CommandWoodcutting = false;
        if (Input.GetKeyDown(KeyCode.Keypad5))
            act_DiagonaleRumpfbeugeLinks();
        if (Input.GetKeyDown(KeyCode.Keypad6))
            act_DiagonaleRumpfbeugeRechts();

        // Rumpfbeuge: REGEN
        if (Input.GetKey(KeyCode.Keypad7))
        {
            act_Rumpfbeuge(Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
            act_SeitneigeRechts();
        if (Input.GetKeyDown(KeyCode.Keypad9))
            act_SeitneigeLinks();
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            act_SeitschrittRechts();
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            act_SeitschrittLinks();
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            act_Hueftkreisen();

        if (!isDay && amountMunterkeit < 100)
        {
            addMunterkeit(Time.deltaTime * 2);
        }

        //updating progress text
        GameObject.Find("Rotation_CompletionImage").GetComponent<Image>().fillAmount = ((float)reptDone_Rumpfrotation / (float)reptReq_Rumpfrotation);
        GameObject.Find("StandingPunches_CompletionImage").GetComponent<Image>().fillAmount = ((float)timeDone_BoxenImStand / (float)timeReq_BoxenImStand);
        GameObject.Find("BodyBend_CompletionImage").GetComponent<Image>().fillAmount = ((float)timeDone_Rumpfbeuge / (float)timeReq_Rumpfbeuge);
        GameObject.Find("StandingOneLeggedRight_CompletionImage").GetComponent<Image>().fillAmount = ((float)reptDone_EinBeinStandRechts / (float)reptReq_EinBeinStandRechts);
        GameObject.Find("StandingOneLeggedLeft_CompletionImage").GetComponent<Image>().fillAmount = ((float)reptDone_EinBeinStandLinks / (float)reptReq_EinBeinStandLinks);
        GameObject.Find("HipCirculation_CompletionImage").GetComponent<Image>().fillAmount = ((float)reptDone_Hueftkreisen / (float)reptReq_Hueftkreisen);
        GameObject.Find("SideStep_CompletionImage").GetComponent<Image>().fillAmount = (((float)reptDone_SeitschrittLinks + (float)reptDone_SeitschrittRechts) / ((float)reptReq_SeitschrittLinks + (float)reptReq_SeitschrittRechts));
        GameObject.Find("WalkOnTheSpot_CompletionImage").GetComponent<Image>().fillAmount = ((float)timeDone_GehenAufDerStelle / (float)timeReq_GehenAufDerStelle);
        float progress = 100 * ((timeDone_GehenAufDerStelle / timeReq_GehenAufDerStelle) + ((float)reptDone_EinBeinStandLinks / (float)reptReq_EinBeinStandLinks) + ((float)reptDone_EinBeinStandRechts / (float)reptReq_EinBeinStandRechts) + ((float)reptDone_Rumpfrotation / (float)reptReq_Rumpfrotation) + (timeDone_BoxenImStand / timeReq_BoxenImStand) + ((float)reptDone_DiagonaleRumpfbeugeLinks / (float)reptReq_DiagonaleRumpfbeugeLinks) + ((float)reptDone_DiagonaleRumpfbeugeRechts / (float)reptReq_DiagonaleRumpfbeugeRechts) + ((float)reptDone_Rumpfbeuge / (float)reptReq_Rumpfbeuge) + ((float)reptDone_SeitneigeRechts / (float)reptReq_SeitneigeRechts) + ((float)reptDone_Seitneigelinks / (float)reptReq_SeitneigeLinks) + ((float)reptDone_SeitschrittLinks / (float)reptReq_SeitschrittLinks) + ((float)reptDone_SeitschrittRechts / (float)reptReq_SeitschrittRechts)) / 12;
        //GameObject.Find("ProgressText").GetComponent<Text>().text = "<size=40>Progress: "+ Mathf.Round(progress) + "%</size>\n\n(0) GehenAufDerStelle = "+ Mathf.Round(timeDone_GehenAufDerStelle) + " / " + timeReq_GehenAufDerStelle + "\nEinBeinStand = " + Mathf.Round(timeDone_EinBeinStand) + " / " + timeReq_EinBeinStand + "\n(1) EinBeinStandLinks = " + reptDone_EinBeinStandLinks + " / " + reptReq_EinBeinStandLinks + "\n(2) EinBeinStandRechts = " + reptDone_EinBeinStandRechts + " / " + reptReq_EinBeinStandRechts + "\n(3) Rumpfrotation = " + reptDone_Rumpfrotation + " / " + reptReq_Rumpfrotation + "\n(4) BoxenImStand = " + Mathf.Round(timeDone_BoxenImStand) + " / " + timeReq_BoxenImStand + "\n(5) DiagonaleRumpfbeugeLinks = " + reptDone_DiagonaleRumpfbeugeLinks + " / " + reptReq_DiagonaleRumpfbeugeLinks + "\n(6) DiagonaleRumpfbeugeRechts = " + reptDone_DiagonaleRumpfbeugeRechts + " / " + reptReq_DiagonaleRumpfbeugeRechts + "\n(7) Rumpfbeuge = " + reptDone_Rumpfbeuge + " / " + reptReq_Rumpfbeuge + "\n(8) SeitneigeRechts = " + reptDone_SeitneigeRechts + " / "+reptReq_SeitneigeRechts+"\n(9) SeitneigeLinks = " + reptDone_Seitneigelinks + " / "+reptReq_SeitneigeLinks+"\n(*) SeitschrittRechts = " + reptDone_SeitschrittRechts + " / " + reptReq_SeitschrittRechts + "\n(-) SeitschrittLinks = " + reptDone_SeitschrittLinks + " / " + reptReq_SeitschrittLinks + "";
    }

    public float getTerrainHeight(float x, float y)
    {
        float noise = Mathf.PerlinNoise((x / noiseMagnitude) + randomseed, (y / noiseMagnitude) + randomseed);

        float distanceToCenter = Mathf.Sqrt(Mathf.Pow((x - (maxX / 2)), 2) + Mathf.Pow((y - (maxZ / 2)), 2));
        float distanceToCenterNorm = distanceToCenter / (maxX);

        //Return the noise value
        float ret = distanceToCenterNorm * noise * maxHeight;
        if (ret < 0) ret = 0;
        return ret;

    }

    public QuantizedColor height2QColor(int height)
    {
        QuantizedColor qc;
        float heightColorDistortion = 1 - (height / maxHeight);
        qc = new QuantizedColor(82, (byte)(heightColorDistortion * 140), (byte)(heightColorDistortion * 72), 255);
        if (height == 0)
            qc = new QuantizedColor(0, 0, 255, 255);
        //if (height > 40)
        //    qc = new QuantizedColor(255, 255, 255, 255);
        return qc;
    }
    public QuantizedColor height2QColorIsland(int height)
    {
        return height2QColor(height);
        //TODO?
        QuantizedColor qc = new QuantizedColor(0, 0, 255, 255);

        if (height > 4)
        {
            float heightColorDistortion = 1 - (height / maxHeight);
            qc = new QuantizedColor(82, (byte)(heightColorDistortion * 140), (byte)(heightColorDistortion * 72), 255);
        }
        else
        {
            if (height == 1)
                qc = new QuantizedColor(202, 200, 12, 255);
            if (height == 2)
                qc = new QuantizedColor(162, 180, 32, 255);
            if (height == 3)
                qc = new QuantizedColor(122, 160, 52, 255);
            if (height == 4)
                qc = new QuantizedColor(82, 140, 72, 255);

        }
        return qc;
    }

    public void spawnObjects()
    {
        // trees:
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxZ; j++)
            {
                int height = (int)getTerrainHeight(i, j);
                float heightDistortion = (height / maxHeight);
                int r = Random.Range(0, 10000);
                if (r < (int)(treeMultiplier * 100 * heightDistortion))
                {
                    spawnTree(i, height, j);
                }
            }
        }

        buildTown();
    }

    public void spawnObjectsIsland()
    {
        // trees:
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxZ; j++)
            {
                int height = (int)islandHeight[i, j];
                float heightDistortion = (height / maxHeight);
                int r = Random.Range(0, 10000);
                if (r < (int)(treeMultiplier * 100 * heightDistortion))
                {
                    spawnTree(i, height, j);
                }
            }
        }

        buildTownIsland();
    }

    public void spawnTree(int x, int y, int z)
    {
        SpawnableTree tree = new SpawnableTree(x, y, z, ccv);
        //SpawnableTree tree = (SpawnableTree)ScriptableObject.CreateInstance("SpawnableTree");
        //tree.x = x;
        // tree.y = y;
        //tree.z = z;
        // tree.data = data;
        // tree.spawn();
        SpawnableObjectList.Add(tree);
        for (int i = x - tree.occXneg; i <= x + tree.occXpos; i++)
            for (int j = z - tree.occZneg; j <= z + tree.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

    }
    public void buildTown()
    {
        // rathaus:
        // try to find the most central point that is not obstructed by water (radius 5)
        int ci = maxX / 2;
        int cj = maxZ / 2;
        bool found = false;
        while (!found)
        {
            bool obstructed = false;
            for (int ii = ci - 15; ii < ci + 15; ii++)
                for (int jj = cj - 20; jj < cj + 20; jj++)
                    if ((int)(getTerrainHeight(ii, jj)) <= 0)
                        obstructed = true;
            if (!obstructed)
                found = true;
            else
            {
                ci++;
                cj++;
            }
        }
        spawnTownHall(ci, (int)getTerrainHeight(ci, cj), cj);
        townCenterX = ci;
        townCenterZ = cj;

        // houses
        int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
        int y = 0;
        if (landscapeType == LandscapeType.valley)
            y = (int)getTerrainHeight(loc[0], loc[1]);
        spawnHouse(loc[0], y, loc[1], true);

        loc = findSpaceForSpawnable(11, 11, 10, 10);
        if (landscapeType == LandscapeType.valley)
            y = (int)getTerrainHeight(loc[0], loc[1]);
        spawnHouse(loc[0], y, loc[1], true);
    }
    public void buildTownIsland()
    {
        // rathaus:
        // try to find the most central point that is not obstructed by water (radius 5)
        int ci = maxX / 2;
        int cj = maxZ / 2;
        bool found = false;
        while (!found)
        {
            bool obstructed = false;
            for (int ii = ci - 15; ii < ci + 15; ii++)
                for (int jj = cj - 20; jj < cj + 20; jj++)
                    if ((int)(islandHeight[ii, jj]) <= 0)
                        obstructed = true;
            if (!obstructed)
                found = true;
            else
            {
                ci++;
                cj++;
            }
        }
        spawnTownHall(ci, (int)islandHeight[ci, cj], cj);
        townCenterX = ci;
        townCenterZ = cj;

        // houses
        int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
        int y = 0;
        if (landscapeType == LandscapeType.island)
            y = (int)islandHeight[loc[0], loc[1]];
        spawnHouse(loc[0], y, loc[1], true);

        loc = findSpaceForSpawnable(11, 11, 10, 10);
        if (landscapeType == LandscapeType.island)
            y = (int)islandHeight[loc[0], loc[1]];
        spawnHouse(loc[0], y, loc[1], true);

        if(week == 2)
        {
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnHouse(loc[0], y, loc[1], true);
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnHouse(loc[0], y, loc[1], true);
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnHouse(loc[0], y, loc[1], true);

            loc = findSpaceForSpawnable(11, 11, 10, 10);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnWindmill(loc[0], y, loc[1], true);
            loc = findSpaceForSpawnable(11, 11, 10, 10);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            spawnField(loc[0], y, loc[1], true);
        }
    }
    public void spawnTownHall(int x, int y, int z)
    {
        // find minimum y
        int minY = y;
        for (int i = x - SpawnableTownHall._occXneg; i <= x + SpawnableTownHall._occXpos; i++)
            for (int j = z - SpawnableTownHall._occZneg; j <= z + SpawnableTownHall._occZpos; j++)
                if ((int)getTerrainHeight(i, j) < minY && (int)getTerrainHeight(i, j) > 1)
                {
                    minY = (int)getTerrainHeight(i, j);
                }
        y = minY;

        SpawnableTownHall townhall = new SpawnableTownHall(x, y, z, ccv, templateCastle);
        SpawnableObjectList.Add(townhall);
        SpawnableBuildingsList.Add(townhall);
        for (int i = x - townhall.occXneg; i <= x + townhall.occXpos; i++)
            for (int j = z - townhall.occZneg; j <= z + townhall.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        // TODO: spawn Mayor later
        //GameObject go = spawnMayor(x - 3, (int)getTerrainHeight(x, z) + 10, z + 15);
        //go.GetComponent<VillagerMovement>().homeX = x - 3;
        //go.GetComponent<VillagerMovement>().homeZ = z + 15;
        //VillagersList.Add(go);
        //MayorCamera = GameObject.Find("MayorCamera");
        //MayorCamera.SetActive(false);
    }
    public SpawnableHouse spawnHouse(int x, int y, int z)
    {
        return spawnHouse(x, y, z, false);
    }
    public SpawnableHouse spawnHouse(int x, int y, int z, bool instantFinished)
    {
        SpawnableHouse house = new SpawnableHouse(x, y, z, ccv, templateHouse);
        SpawnableObjectList.Add(house);
        SpawnableBuildingsList.Add(house);
        for (int i = x - house.occXneg; i <= x + house.occXpos; i++)
            for (int j = z - house.occZneg; j <= z + house.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;
        //GameObject go = spawnVillager(x, (int)getTerrainHeight(x, z) + 10, z);
        //go.GetComponent<VillagerMovement>().homeX = x;
        //go.GetComponent<VillagerMovement>().homeZ = z;
        //VillagersList.Add(go);

        if (instantFinished)
            house.gameObject.GetComponentInChildren<moveCrane>().finishConstruction(false);

        return house;
    }
    public SpawnableWindmill spawnWindmill(int x, int y, int z)
    {
        return spawnWindmill(x, y, z, false);
    }
    public SpawnableWindmill spawnWindmill(int x, int y, int z, bool instantFinished)
    {
        SpawnableWindmill windmill = new SpawnableWindmill(x, y, z, ccv, templateWindmill);
        SpawnableObjectList.Add(windmill);
        SpawnableBuildingsList.Add(windmill);
        for (int i = x - windmill.occXneg; i <= x + windmill.occXpos; i++)
            for (int j = z - windmill.occZneg; j <= z + windmill.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        if (instantFinished)
            windmill.gameObject.GetComponentInChildren<moveCrane>().finishConstruction(false);

        return windmill;
    }
    public SpawnableField spawnField(int x, int y, int z)
    {
        return spawnField(x, y, z, false);
    }
    public SpawnableField spawnField(int x, int y, int z, bool instantFinished)
    {
        SpawnableField field = new SpawnableField(x, y, z, ccv, templateField);
        SpawnableObjectList.Add(field);
        SpawnableBuildingsList.Add(field);
        for (int i = x - field.occXneg; i <= x + field.occXpos; i++)
            for (int j = z - field.occZneg; j <= z + field.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        if (instantFinished)
            field.gameObject.GetComponentInChildren<moveCrane>().finishConstruction(false);

        return field;
    }
    public void spawnSilo(int x, int y, int z)
    {
        SpawnableSilo silo = new SpawnableSilo(x, y, z, ccv, templateSilo);
        SpawnableObjectList.Add(silo);
        SpawnableBuildingsList.Add(silo);
        for (int i = x - silo.occXneg; i <= x + silo.occXpos; i++)
            for (int j = z - silo.occZneg; j <= z + silo.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;
    }
    public SpawnableSolar spawnSolar(int x, int y, int z)
    {
        SpawnableSolar solar = new SpawnableSolar(x, y, z, ccv, templateSolar);
        SpawnableObjectList.Add(solar);
        SpawnableBuildingsList.Add(solar);
        for (int i = x - solar.occXneg; i <= x + solar.occXpos; i++)
            for (int j = z - solar.occZneg; j <= z + solar.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        return solar;
    }
    public void spawnPool(int x, int y, int z)
    {
        SpawnablePool pool = new SpawnablePool(x, y, z, ccv, templatePool);
        SpawnableObjectList.Add(pool);
        SpawnableBuildingsList.Add(pool);
        for (int i = x - pool.occXneg; i <= x + pool.occXpos; i++)
            for (int j = z - pool.occZneg; j <= z + pool.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;
    }
    public SpawnableWaterTower spawnWaterTower(int x, int y, int z)
    {
        SpawnableWaterTower wt = new SpawnableWaterTower(x, y, z, ccv, templateWatertower);
        SpawnableObjectList.Add(wt);
        SpawnableBuildingsList.Add(wt);
        for (int i = x - wt.occXneg; i <= x + wt.occXpos; i++)
            for (int j = z - wt.occZneg; j <= z + wt.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        return wt;
    }
    public SpawnableWindTurbine spawnWindTurbine(int x, int y, int z)
    {
        SpawnableWindTurbine wt = new SpawnableWindTurbine(x, y, z, ccv, templateWindturbine);
        SpawnableObjectList.Add(wt);
        SpawnableBuildingsList.Add(wt);
        for (int i = x - wt.occXneg; i <= x + wt.occXpos; i++)
            for (int j = z - wt.occZneg; j <= z + wt.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;

        return wt;
    }
    public void spawnShip()
    {
        SpawnableShip sh = new SpawnableShip(80, 0, 80, ccv, templateShip);
    }
    public void spawnChurch(int x, int y, int z)
    {
        SpawnableChurch spawn = new SpawnableChurch(x, y, z, ccv, templateChurch);
        SpawnableObjectList.Add(spawn);
        SpawnableBuildingsList.Add(spawn);
        for (int i = x - spawn.occXneg; i <= x + spawn.occXpos; i++)
            for (int j = z - spawn.occZneg; j <= z + spawn.occZpos; j++)
                if (i >= 0 && i < maxX && j >= 0 && j < maxZ)
                    obstructedMap[i, j] = true;
    }



    public int[] findSpaceForSpawnable(int sizeXneg, int sizeXpos, int sizeZneg, int sizeZpos)
    {
        int x = townCenterX;
        int z = townCenterZ;
        int dist = 1;
        bool found = false;

        while (!found)
        {
            for (int i = x - dist; i <= x + dist; i++)
                for (int j = z - dist; j <= z + dist; j++)
                {
                    if (!fieldIsObstructed(i, j))
                    {
                        found = true;
                        for (int ii = i - sizeXneg; ii <= i + sizeXpos && found; ii++)
                            for (int jj = j - sizeZneg; jj <= j + sizeZpos && found; jj++)
                                if (fieldIsObstructed(ii, jj))
                                    found = false;
                    }
                    if (found)
                        return new int[2] { i, j };
                }

            dist++;
        }

        return new int[2] { -1, -1 };
    }
    /*
    public int[] findSpaceForSpawnableOld(int sizeXneg, int sizeXpos, int sizeZneg, int sizeZpos)
    {
        Debug.Log("finding start");
        float[,] distmap = new float[maxX, maxZ];
        for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxZ; j++)
                distmap[i, j] = Mathf.Infinity;
        Debug.Log("finding map done");

        // fill distmap with least distances to town center
        for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxZ; j++)
                foreach (Spawnable s in SpawnableBuildingsList)
                {
                    if (s.description.Equals("Rathaus"))
                    {
                        float dist = Mathf.Sqrt(Mathf.Pow((i - s.x), 2) + Mathf.Pow((j - s.z), 2));
                        if (dist < distmap[i, j])
                            distmap[i, j] = dist;
                    }
                }

        Debug.Log("finding: least distances done");
        // erase possibilities where buildings are occupying space
        foreach (Spawnable s in SpawnableBuildingsList)
        {
            for (int i = s.x - s.occXneg; i < s.x + s.occXpos; i++)
                for (int j = s.z - s.occZneg; j < s.z + s.occZpos; j++)
                {
                    for (int ii = -sizeXneg; ii < sizeXpos; ii++)
                        for (int jj = -sizeZneg; jj < sizeZpos; jj++)
                            if (i + ii > 0 && i + ii < maxX && j + jj > 0 && j + jj < maxZ)
                                distmap[i + ii, j + jj] = Mathf.Infinity;
                }
        }

        Debug.Log("finding: occupying done");
        // search point with closest distance
        float mindist = Mathf.Infinity;
        int minX = 0;
        int minZ = 0;
        for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxZ; j++)
                if(distmap[i, j] < mindist)
                {
                    mindist = distmap[i, j];
                    minX = i;
                    minZ = j;
                }

        Debug.Log("finding: all done");
        return new int[2] { minX, minZ };
    }
    */

    public bool fieldIsObstructed(int x, int z)
    {
        // case: water
        if (landscapeType == LandscapeType.valley)
            if ((int)getTerrainHeight(x, z) <= 0)
                return true;

        if (landscapeType == LandscapeType.island)
            if ((int)islandHeight[x, z] <= 0)
                return true;

        return obstructedMap[x, z];
    }

    public void UpdateSkyAndLight()
    {
        if (isDay)
        {
            if (isRaining)
            {
                mainLight.GetComponent<Light>().intensity = 0.5f;
                mainCam.GetComponent<Skybox>().material = rainsky;
            }
            else
            {
                mainLight.GetComponent<Light>().intensity = 0.8f;
                mainCam.GetComponent<Skybox>().material = daysky;
            }
            TimeLine.transform.localPosition = new Vector3(-50.50008f, 58.20006f, 119.3001f);
            TimeLineNight.transform.localPosition = new Vector3(300, 58.20006f, 119.3001f);
        }
        else
        {
            mainLight.GetComponent<Light>().intensity = 0.5f;
            mainCam.GetComponent<Skybox>().material = nightsky;
            TimeLineNight.transform.localPosition = new Vector3(-50.50008f, 58.20006f, 119.3001f);
            TimeLine.transform.localPosition = new Vector3(300, 58.20006f, 119.3001f);
        }
    }

    public void spawnBorder()
    {
        int x = 0;
        for (int z = 0; z < maxZ; z++)
            for (int y = 0; y < maxHeight * 0.66f; y++)
                data.SetVoxel(x, y, z, new QuantizedColor(50, 50, 50, 255));
        x = maxX - 1;
        for (int z = 0; z < maxZ; z++)
            for (int y = 0; y < maxHeight * 0.66f; y++)
                data.SetVoxel(x, y, z, new QuantizedColor(50, 50, 50, 255));
        int zz = 0;
        for (int xx = 0; xx < maxX; xx++)
            for (int y = 0; y < maxHeight * 0.66f; y++)
                data.SetVoxel(xx, y, zz, new QuantizedColor(50, 50, 50, 255));
        zz = maxZ - 1;
        for (int xx = 0; xx < maxX; xx++)
            for (int y = 0; y < maxHeight * 0.66f; y++)
                data.SetVoxel(xx, y, zz, new QuantizedColor(50, 50, 50, 255));

    }

    public GameObject spawnVillager(int x, int z)
    {
        if (landscapeType == LandscapeType.island)
            return spawnVillager(x, (int)islandHeight[x, z] + 5, z);
        else
            return spawnVillager(x, (int)maxHeight, z);
    }
    public GameObject spawnVillager(int x, int y, int z)
    {
        GameObject go = Instantiate(villagerPrefabs[Random.Range(0, villagerPrefabs.Length - 1)], new Vector3(x, y, z), Quaternion.identity, GameObject.Find("Villagers").transform);
        go.GetComponent<VillagerMovement>().homeX = x;
        go.GetComponent<VillagerMovement>().homeZ = z;
        VillagersList.Add(go);
        return go;
    }
    public GameObject spawnMayor(int x, int y, int z)
    {
        return Instantiate(mayorPrefab, new Vector3(x, y, z), Quaternion.identity, GameObject.Find("Villagers").transform);
    }

    public void addRohstoffe(int amount)
    {
        amountRohstoffe += amount;
        //GameObject.Find("RohstoffeAmount").GetComponent<Text>().text = amountRohstoffe + " / 100";
        GameObject.Find("HolzBarFilling").GetComponent<Image>().fillAmount = (((float)amountRohstoffe) / 100f);
        if (amountRohstoffe >= 100 && !isSelecting && !currentlyUnderConstruction)
        {
            showSelectionCam();
        }
    }

    public void addNahrung(int amount)
    {
        amountNahrung += amount;
        //GameObject.Find("NahrungAmount").GetComponent<Text>().text = amountNahrung + " / 100";
        GameObject.Find("NahrungBarFilling").GetComponent<Image>().fillAmount = (((float)amountNahrung) / 100f);
    }

    public void addMunterkeit(float amount)
    {
        amountMunterkeit += amount;
        GameObject.Find("MunterkeitBarFilling").GetComponent<Image>().fillAmount = (((float)amountMunterkeit) / 100f);
        if (amountMunterkeit <= 0 && !tagnachtTutorial)
        {
            tagnachtTutorial = true;
            amountMunterkeit = 0;
            displayMessage("Oh nein! Deine Bewohner sind müde von der ganzen Arbeit.\r\nBenutze den Einbeinstand rechts, um die Sonne untergehen zu lassen.", 10, "tagnachtTutorialDone");
            playVoiceOver("1_intro_standingOneLegged");
            GameObject.Find("RedArrow_9").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("RedArrow_10").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("StandingOneLeggedRight").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("StandingOneLeggedLeft").GetComponent<Animator>().Play("Exercise_RollIn");
        }
    }

    public void buildWalkmap()
    {
        GameObject parentNode = GameObject.Find("PCGNodes");
        //AstarPath AStar = GameObject.Find("A*").GetComponent<AstarPath>();
        for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxZ; j++)
            {
                GameObject go = Instantiate(walkmapNode, parentNode.transform);
                go.transform.position = new Vector3(i, getTerrainHeight(i, j), j);
            }

        // calculate walkmap
        //AStar.graphs[0].ScanGraph();
    }

    public enum LandscapeType
    {
        valley, island
    }

    public void createValleyLandscape()
    {
        for (int i = 0; i < maxX; i++)
            for (int j = 0; j < maxZ; j++)
            {
                int height = (int)getTerrainHeight(i, j);
                data.SetVoxel(i, height, j, height2QColor(height));
                if (height > 0)
                    data.SetVoxel(i, height - 1, j, height2QColor(height - 1));
                if (height > 1)
                    data.SetVoxel(i, height - 2, j, height2QColor(height - 2));
            }

        spawnObjects();
    }

    public void createIslandLandscape()
    {
        int xmax = maxX;
        int zmax = maxZ;

        int xcentermax = 256;
        int zcentermax = 256;


        float levels = 30;

        // nr of gaussians:
        int n = 35;

        // std deviation range:
        int stdMin = 15;
        int stdMax = 20;

        float[,] height = new float[xmax, zmax];
        float maxHeight = 0;

        //int[,] posn = new int[n,2];
        //int[] std = new int[n];

        //int[,] xz = new int[xmax, zmax];
        //for (int i = 0; i < xmax; i++)
        //    for (int j = 0; j < zmax; j++)
        //        xz[i, j] = i;

        // accumulate gaussians
        for (int i = 0; i < n; i++)
        {
            float xCenter = xcentermax * Random.value + (xmax - xcentermax) / 2;
            float zCenter = zcentermax * Random.value + (zmax - zcentermax) / 2;
            float std = stdMin + (stdMax - stdMin) * Random.value;

            for (int x = 0; x < xmax; x++)
                for (int z = 0; z < zmax; z++)
                {
                    float addHeight = Mathf.Exp(-(Mathf.Pow((x - xCenter), 2) + Mathf.Pow((z - zCenter), 2)) / (2 * Mathf.Pow(std, 2)));
                    height[x, z] = height[x, z] + addHeight;
                    if (height[x, z] > maxHeight)
                        maxHeight = height[x, z];
                }
        }

        // chunk them:
        for (int x = 0; x < xmax; x++)
            for (int z = 0; z < zmax; z++)
            {
                float ratio = height[x, z] / maxHeight;
                for (float i = 0; i < levels; i++)
                    if (ratio > i / levels)
                        height[x, z] = i;
            }

        // create VoxelLandscape:
        for (int x = 0; x < xmax; x++)
            for (int z = 0; z < zmax; z++)
            {
                data.SetVoxel(x, (int)height[x, z], z, height2QColorIsland((int)height[x, z]));
                if (height[x, z] > 0)
                    data.SetVoxel(x, (int)height[x, z] - 1, z, height2QColorIsland((int)height[x, z] - 1));
                if (height[x, z] > 1)
                    data.SetVoxel(x, (int)height[x, z] - 2, z, height2QColorIsland((int)height[x, z] - 2));
            }

        islandHeight = height;
        spawnObjectsIsland();
    }

    public void buildingFinished(Spawnable.BuildingType buildingType, int x, int z, bool sendMessage)
    {
        if (sendMessage)
        {
            GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Closed_smile;
            /*MayorCamera.SetActive(true);
            sprechblase.SetActive(true);
            sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
            sprechblase.GetComponent<normalizeRotation>().resetDuration();
            sprechblase.GetComponentInChildren<Text>().text = "Das Gebäude ist fertig!";*/
            displayMessage("Das Gebäude ist fertig.\r\nWeiter so!", 5, "checkTutorialAfterBuildingFinished");
            playVoiceOver("0_construction_finished");
            if (buildingType == Spawnable.BuildingType.House)
                buildingsRequired_Houses--;
            if (buildingType == Spawnable.BuildingType.WindMill)
                buildingsRequired_Mills--;
            if (buildingType == Spawnable.BuildingType.Field)
                buildingsRequired_Fields--;
            if (buildingType == Spawnable.BuildingType.Solar)
                buildingsRequired_Solar--;
            if (buildingType == Spawnable.BuildingType.WindWheel)
                buildingsRequired_WindWheel--;
            if (buildingType == Spawnable.BuildingType.WaterTower)
                buildingsRequired_WaterTower--;
            checkMissionProgress();

            /*CameraStartPosition = mainCam.transform.position;
            CameraTargetPosition = CameraInitialPosition;
            CameraStartRotation = mainCam.transform.rotation.eulerAngles;
            CameraTargetRotation = CameraInitialRotation;
            lerpProgress = 0f;
            CameraLerping = true;*/
            CameraLerpToDefault();
            currentlyUnderConstruction = false;
        }

        if (buildingType == Spawnable.BuildingType.House)
            spawnVillager(x, z);
    }


    public void welcomeMessage()
    {
        GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Happy;
        if(week == 2)
        {
            displayMessage("Willkommen zurück!\r\nBist Du gespannt, wie es Deinen Dorfbewohnern heute geht?", 6, "CameraLerpToHeliTop");
            playVoiceOver("2_prolog");
        }
        else
        {
            displayMessage("Willkommen auf Adaptify Island!\r\nIch bin der Bürgermeister unserer neuen Siedlung.", 6, "welcomeMessage2");
            playVoiceOver("1_prolog");
        }

        /*
        sprechblase.SetActive(true);
        sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
        sprechblase.GetComponent<normalizeRotation>().HideAfterDisplay = false;
        sprechblase.GetComponent<normalizeRotation>().resetDuration();
        sprechblase.GetComponent<normalizeRotation>().EndFunction = "CameraLerpToHeliTop";
        sprechblase.GetComponentInChildren<Text>().text = "Willkommen in der Welt von Adaptify!";
        */
        mainCam.transform.position = GameObject.Find("Heli").GetComponentInChildren<Camera>().transform.position;
        mainCam.transform.rotation = GameObject.Find("Heli").GetComponentInChildren<Camera>().transform.rotation;
    }

    public void welcomeMessage2()
    {
        displayMessage("Du musst mir und den anderen Bewohnern helfen, unser Dorf aufzubauen.\r\nMithilfe der Übungen kannst Du die Welt rund um diese Insel beeinflussen.",9, "CameraLerpToHeliTop");
        playVoiceOver("1_prolog_1");
    }

    public void levelField(int x, int z, int height, int xdir, int zdir)
    {
        int direction;
        // recursively level to "height" in steps of 1 difference
        if (GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().landscapeType == CreateVoxelLandscape.LandscapeType.island)
        {
            if (x < 0 || z < 0 || x >= islandHeight.Length || z >= islandHeight.Length)
                return;
            if (islandHeight[x, z] == height || islandHeight[x, z] == height + 1 || islandHeight[x, z] == height - 1)
                return;
            if (islandHeight[x, z] > height)
                direction = +1;
            else
                direction = -1;

            // destroy old voxels
            //data.SetVoxel(x, (int)islandHeight[x, z], z, new QuantizedColor());
            for (int k = height - 5; k <= height + 20; k++)
                data.SetVoxel(x, k, z, new QuantizedColor(0, 0, 0, 0));

            // set new voxel
            islandHeight[x, z] = height + direction;
            data.SetVoxel(x, height + direction, z, height2QColorIsland(height + direction));

            // propagate
            levelField(x + xdir, z + zdir, height + direction, xdir, zdir);
            if (Mathf.Abs(xdir) + Mathf.Abs(zdir) == 2)
            {
                levelField(x + xdir, z, height + direction, xdir, 0);
                levelField(x, z + zdir, height + direction, 0, zdir);
            }
        }

    }
    public void speechbubbleText(string text, string EndFunction, bool HideAfterDisplay)
    {
        GameObject.Find("VillagerMayor(Clone)").GetComponent<PlayFaceAnimationsCS>().Eye = PlayFaceAnimationsCS.Eyes_Expressions.Happy;
        if (MayorCamera == null)
            MayorCamera = GameObject.Find("MayorCamera");
        if (MayorCamera != null)
            MayorCamera.SetActive(true);
        sprechblase.SetActive(true);
        sprechblase.transform.localRotation = Quaternion.Euler(50, 0, 0);
        sprechblase.GetComponent<normalizeRotation>().resetDuration();
        sprechblase.GetComponent<normalizeRotation>().EndFunction = EndFunction;
        sprechblase.GetComponent<normalizeRotation>().HideAfterDisplay = HideAfterDisplay;
        sprechblase.GetComponentInChildren<Text>().text = text;
    }
    public void BoxenMessage()
    {
        //speechbubbleText("Um Gebäude zu errichten, benötigen wir Holz.","BoxenMessage2",false);
        if (week < 2)
        {
            displayMessage("Um Gebäude zu errichten, benötigen wir Holz. Deine Dorfbewohner können Dir helfen, es zu besorgen.", 8, "BoxenMessage2");
            playVoiceOver("1_intro_standingPunches");
        }
        else
        {
            displayMessage("Beim letzten Mal hast Du gelernt, wie die Welt auf Deine Übungen reagiert.", 5, "BoxenMessage2");
            playVoiceOver("2_prolog_1");
        }
    }
    public void BoxenMessage2()
    {
        //speechbubbleText("Wenn Du \"Boxen im Stand\" anwendest, werden die Bewohner Holz abbauen.", "", true);
        if(week < 2)
        {
            GameObject.Find("RedArrow_2").GetComponent<RectTransform>().localScale = Vector3.one;
            GameObject.Find("StandingPunches").GetComponent<Animator>().Play("Exercise_RollIn");
            displayMessage("Wenn Du \"Boxen im Stand\" anwendest, werden sie anfangen, Bäume zu fällen.", 6, "");
            playVoiceOver("1_intro_standingPunches_2");
            allow_boxenImStand = true;
        }
        else
        {
            GameObject.Find("StandingPunches").GetComponent<Animator>().Play("Exercise_RollIn");
            displayMessage("Setze ein, was Du gelernt hast, um die neuen Gebäude zu errichten und die Aufgaben zu erfüllen.", 7, "");
            playVoiceOver("2_prolog_2");
            allow_boxenImStand = true;
            holzTutorial = true;
            GameObject.Find("WalkOnTheSpot").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("BodyBend").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("HipCirculation").GetComponent<Animator>().Play("Exercise_RollIn");
            rainTutorialDone = true;
            windTutorialDone = true;
            tutorial_CraneDisplayed = true;
            firstNightTutorial = true;
            GameObject.Find("SideStep").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("StandingOneLeggedRight").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("StandingOneLeggedLeft").GetComponent<Animator>().Play("Exercise_RollIn");
        }
    }

    public void CameraLerpToHeliTop()
    {
        CameraStartPosition = mainCam.transform.position;
        CameraTargetPosition = GameObject.Find("HeliCamera2").GetComponentInChildren<Camera>().transform.position;
        CameraStartRotation = mainCam.transform.rotation.eulerAngles;
        CameraTargetRotation = GameObject.Find("HeliCamera2").GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
        lerpProgress = 0f;
        CameraLerping = true;
        heliMode = true;
        //speechbubbleText("Schauen wir uns doch erstmal die Insel an! Führe die Rumpfrotation durch, um den Heli steigen zu lassen.", "",true);
        if(week == 2)
        {
            displayMessage("Schauen wir doch einfach mal nach! Führe wie letzte Woche die Rumpfrotation durch, um Dir die Insel von oben anzusehen.", 8, "");
            playVoiceOver("2_intro_rotation");
        }   
        else
        {
            displayMessage("Schauen wir sie uns doch erstmal von oben an!\r\n Führe die Rumpfrotation durch, um den Heli steigen zu lassen.", 8, "");
            playVoiceOver("1_intro_rotation");
        }
        GameObject.Find("Rotation").GetComponent<Animator>().Play("Exercise_RollIn");
        GameObject.Find("RedArrow_1").GetComponent<RectTransform>().localScale = Vector3.one;
        switchGravityOn();
    }
    public void CameraLerpToDefault()
    {
        if(GameObject.Find("KinectGetDistance").GetComponent<KinectGetDistance>()._distance > 0)
            CameraTargetPosition = getAdaptiveCameraPosition();
        else
            CameraTargetPosition = CameraInitialPosition;

        CameraStartPosition = mainCam.transform.position;
        CameraStartRotation = mainCam.transform.rotation.eulerAngles;
        CameraTargetRotation = CameraInitialRotation;
        lerpProgress = 0f;
        CameraLerping = true;
        CameraBackToStart = true;
    }

    public void initEASYBeginner()
    {
        timeReq_Rumpfbeuge = 15;    // 25
        timeReq_GehenAufDerStelle = 40; // 60
        timeReq_EinBeinStand = 14;  // 20
        reptReq_EinBeinStandLinks = 2;
        reptReq_EinBeinStandRechts = 2;
        reptReq_Rumpfrotation = 10;
        timeReq_BoxenImStand = 90;     // 120
        reptReq_DiagonaleRumpfbeugeLinks = 8;
        reptReq_DiagonaleRumpfbeugeRechts = 8;
        reptReq_Rumpfbeuge = 5;
        reptReq_Hueftkreisen = 20;
        reptReq_SeitneigeRechts = 8;
        reptReq_SeitneigeLinks = 8;
        reptReq_SeitschrittRechts = 10;
        reptReq_SeitschrittLinks = 10;
        woodNeededTotal = 500;
        buildingsRequired = 5;

        if(week < 2)
        {
            buildingsRequired_Houses = 3;
            buildingsRequired_Fields = 1;
            buildingsRequired_Mills = 1;

            fieldworkRequired = 1;
        }
        else
        {
            buildingsRequired_Solar = 2;
            buildingsRequired_WindWheel = 2;
            buildingsRequired_WaterTower = 1;
        }

        rainSecondsPerRepetition = 4.0f;
        // completed:
        timeDone_Rumpfbeuge = 0;
        timeDone_GehenAufDerStelle = 0;
        timeDone_EinBeinStand = 0;
        reptDone_EinBeinStandLinks = 0;
        reptDone_EinBeinStandRechts = 0;
        reptDone_Rumpfrotation = 0;
        reptDone_Hueftkreisen = 0;
        timeDone_BoxenImStand = 0;
        reptDone_DiagonaleRumpfbeugeLinks = 0;
        reptDone_DiagonaleRumpfbeugeRechts = 0;
        reptDone_Rumpfbeuge = 0;
        reptDone_SeitneigeRechts = 0;
        reptDone_Seitneigelinks = 0;
        reptDone_SeitschrittRechts = 0;
        reptDone_SeitschrittLinks = 0;
        woodCollected = 0;
        regenBuffer = 0;
    }

    public void mapExerciseToWood(VillagerMovement villager)
    {
        if (!holzTutorial && timeDone_BoxenImStand > 0)
        {
            CameraLerpTo(GameObject.Find("WoodCam"));

            holzTutorial = true;
            GameObject.Find("RedArrow_3").GetComponent<RectTransform>().localScale = Vector3.one;
            displayMessage("Wenn Deine Dorfbewohner Holz fällen, wird dies hier gesammelt. Sobald die Leiste gefüllt ist, kannst Du ein neues Gebäude bauen.", 9, "holzTutorialEnde");
            playVoiceOver("1_intro_standingPunches_3");
        }

        float boxenProgress = timeDone_BoxenImStand / timeReq_BoxenImStand;
        if (boxenProgress < 1)
        {
            boxenProgress = boxenProgress * woodNeededTotal;
            boxenProgress = boxenProgress - woodCollected;
            addRohstoffe((int)boxenProgress);
            woodCollected += (int)boxenProgress;

            addMunterkeit(-boxenProgress / 4);
        }
        else
            // if player is already above requirements
            addRohstoffe(Random.Range(1, 9));


    }

    public void switchGravityOn()
    {
        Physics.gravity = new Vector3(0, -9.8f, 0);
    }

    public void act_EinBeinStandLinks(float deltaTime)
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;

        // EinbeinStandLinks
        GameObject.Find("StandingOneLeggedLeft_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
        if (!GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().isPlaying)
            GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        if ((isDay && daytime >= 0 && daytime <= 90) || (!isDay && daytime <= 0 && daytime >= -90))
        {
            // record exercise:
            //timeDone_EinBeinStand += Time.deltaTime;
            timeDone_EinBeinStand += deltaTime;

            //if (reptReq_EinBeinStandLinks < reptDone_EinBeinStandLinks)


            if (timeDone_EinBeinStand * 1.05f >= timeReq_EinBeinStand)
            {
                timeDone_EinBeinStand = 0;
                if (reptReq_EinBeinStandLinks < reptDone_EinBeinStandLinks)
                {
                    reptDone_EinBeinStandLinks++;
                    if (reptDone_EinBeinStandLinks == reptReq_EinBeinStandLinks)
                        GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
                }
                else
                    reptDone_EinBeinStandLinks++;
            }
            // raise sun/moon:
            float sunTimeProgress = Time.deltaTime * 90 / timeReq_EinBeinStand;
            GameObject.Find("Sun").transform.Rotate(-sunTimeProgress, 0f, 0f);
            if (isDay)
                daytime = daytime + sunTimeProgress;
            else
                daytime = daytime - sunTimeProgress;

            if (isDay && GameObject.Find("Sun").transform.eulerAngles.x > 350)
            {
                // dusk
                waterDay.SetActive(false);
                waterNight.SetActive(true);
                isDay = false;
                UpdateSkyAndLight();
                daytime = 0;
            }
            else if (!isDay && GameObject.Find("Sun").transform.eulerAngles.x < 10)
            {
                // dawn
                waterDay.SetActive(true);
                waterNight.SetActive(false);
                isDay = true;
                UpdateSkyAndLight();
                daytime = 0;
            }
        }

        logger += Time.time + ";standingOneLegged_Left\r\n";
    }

    public void act_EinBeinStandRechts(float deltaTime)
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        GameObject.Find("StandingOneLeggedRight_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
        if (!GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().isPlaying)
            GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        // EinbeinStandRechts
        if ((isDay && daytime >= 90 && daytime <= 180) || (!isDay && daytime <= -90 && daytime >= -180))
        {
            // record exercise:
            //timeDone_EinBeinStand += Time.deltaTime;
            timeDone_EinBeinStand += deltaTime;

            //if (reptReq_EinBeinStandRechts < reptDone_EinBeinStandRechts)

            if (timeDone_EinBeinStand * 1.05f >= timeReq_EinBeinStand)
            {
                timeDone_EinBeinStand = 0;
                if (reptReq_EinBeinStandRechts < reptDone_EinBeinStandRechts)
                {
                    reptDone_EinBeinStandRechts++;
                    if (reptDone_EinBeinStandRechts == reptReq_EinBeinStandRechts)
                        GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();

                }
                else
                    reptDone_EinBeinStandRechts++;
            }
            // lower sun/moon:
            float sunTimeProgress = Time.deltaTime * 90 / timeReq_EinBeinStand;
            GameObject.Find("Sun").transform.Rotate(-sunTimeProgress, 0f, 0f);
            if (isDay)
                daytime = daytime + sunTimeProgress;
            else
                daytime = daytime - sunTimeProgress;

            if (isDay && GameObject.Find("Sun").transform.eulerAngles.x > 350)
            {
                // dusk
                waterDay.SetActive(false);
                waterNight.SetActive(true);
                isDay = false;
                UpdateSkyAndLight();
                daytime = 0;
                if (!firstNightTutorial)
                {
                    displayMessage("Wenn es Nacht ist, ruhen wir uns aus.\r\nWechsle nun das Bein, um den Mond aufgehen zu lassen.", 9, "NightTutorial2");
                    playVoiceOver("1_intro_standingOneLegged_2");
                    firstNightTutorial = true;
                }
            }
            else if (!isDay && GameObject.Find("Sun").transform.eulerAngles.x < 10)
            {
                // dawn
                waterDay.SetActive(true);
                waterNight.SetActive(false);
                isDay = true;
                UpdateSkyAndLight();
                daytime = 0;
            }
        }
        logger += Time.time + ";standingOneLegged_Right\r\n";
    }

    public void act_GehenAufDerStelle(float deltaTime)
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        //timeDone_GehenAufDerStelle += Time.deltaTime;
        timeDone_GehenAufDerStelle += deltaTime;
        if (timeDone_GehenAufDerStelle - deltaTime < timeReq_GehenAufDerStelle)
        {
            GameObject.Find("WalkOnTheSpot_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
            if (timeDone_GehenAufDerStelle >= timeReq_GehenAufDerStelle)
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
            else
                if (!GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().isPlaying)
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        }

        logger += Time.time + ";walkingOnTheSpot\r\n";

    }

    public void act_Rumpfrotation()
    {
        if (heliMode && reptDone_Rumpfrotation < reptReq_Rumpfrotation)
        {
            if (week < 2 && reptDone_Rumpfrotation == 0)
            {
                displayMessage("Super! Mach weiter, bis der Heli eine angenehme Höhe erreicht hat. Die Symbole auf der rechten Seite und die Geräusche zeigen Dir, ob Deine Übungen erkannt werden.", 11, "");
                playVoiceOver("1_intro_rotation_2");
            }

            reptDone_Rumpfrotation++;
            GameObject.Find("Rotation_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");

            if (reptDone_Rumpfrotation == reptReq_Rumpfrotation)
            {
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
                CameraLerpToDefault();
                heliMode = false;
                Destroy(GameObject.Find("Heli"));
                GameObject.Find("KinectGetDistance").GetComponent<KinectGetDistance>().enabled = true;

                GameObject go = spawnMayor(townCenterX - 3, 50, townCenterZ + 15);
                go.GetComponent<VillagerMovement>().homeX = townCenterX - 3;
                go.GetComponent<VillagerMovement>().homeZ = townCenterZ + 15;
                VillagersList.Add(go);
                MayorCamera = GameObject.Find("MayorCamera");
                MayorCamera.SetActive(false);
                //speechbubbleText("Prima! Jetzt können wir anfangen, diese Insel zu bebauen!", "BoxenMessage",false);
                GameObject.Find("RedArrow_1").GetComponent<RectTransform>().localScale = Vector3.zero;
                if(week < 2)
                {
                    displayMessage("Prima! Jetzt können wir anfangen, diese Insel zu bebauen!", 5, "BoxenMessage");
                    playVoiceOver("1_intro_rotation_3");
                }
                else
                {
                    displayMessage("Prima! Deinen Dorfbewohnern geht es gut. Zum modernen Leben fehlt ihnen allerdings die Stromversorgung.", 8, "BoxenMessage");
                    playVoiceOver("2_intro_rotation_1");
                }
            }
            else
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        }
        logger += Time.time + ";rotation\r\n";
    }

    public void act_BoxenImStand(float deltaTime)
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation || !allow_boxenImStand)
            return;

        //timeDone_BoxenImStand += Time.deltaTime;
        GameObject.Find("RedArrow_2").GetComponent<RectTransform>().localScale = Vector3.zero;
        CommandWoodcutting = true;
        if (timeDone_BoxenImStand < timeReq_BoxenImStand)
        {
            timeDone_BoxenImStand += deltaTime;
            GameObject.Find("StandingPunches_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
            if (timeDone_BoxenImStand >= timeReq_BoxenImStand)
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
            else
                if (!GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().isPlaying)
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        }
        else
            timeDone_BoxenImStand++;
        logger += Time.time + ";standingPunches\r\n";
    }

    public void act_DiagonaleRumpfbeugeLinks()
    {
        reptDone_DiagonaleRumpfbeugeLinks++;
    }

    public void act_DiagonaleRumpfbeugeRechts()
    {
        reptDone_DiagonaleRumpfbeugeRechts++;
    }

    public void act_Rumpfbeuge(float deltaTime)
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        //reptDone_Rumpfbeuge++;
        //regenBuffer += rainSecondsPerRepetition;
        if (GameObject.Find("GrainFieldCropped(Clone)") == null)
            return;

        regenBuffer += deltaTime;
        timeDone_Rumpfbeuge += deltaTime;

        if (timeDone_Rumpfbeuge - deltaTime < timeReq_Rumpfbeuge)
        {
            GameObject.Find("BodyBend_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
            if (timeDone_Rumpfbeuge >= timeReq_Rumpfbeuge)
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
            else
                if (!GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().isPlaying)
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
            checkMissionProgress();
        }

        logger += Time.time + ";bodyBend\r\n";
        /*if (reptDone_Rumpfbeuge-1 < reptReq_Rumpfbeuge)
        {
            GameObject.Find("BodyBend_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
            if (reptDone_Rumpfbeuge >= reptReq_Rumpfbeuge)
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
            else
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        }*/
    }

    public void act_SeitneigeRechts()
    {
        reptDone_SeitneigeRechts++;
    }

    public void act_SeitneigeLinks()
    {
        reptDone_Seitneigelinks++;
    }

    public void act_SeitschrittRechts()
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        GameObject go = GameObject.Find("KranHeadParent");
        if (go != null)
        {
            go.GetComponent<moveCrane>().moveRight();
            reptDone_SeitschrittRechts++;
            if (reptDone_SeitschrittRechts - 1 < reptReq_SeitschrittRechts)
            {
                GameObject.Find("SideStep_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
                if (reptDone_SeitschrittRechts >= reptReq_SeitschrittRechts)
                    GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
                else
                    GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
            }
        }
        logger += Time.time + ";sideStep_right\r\n";
    }

    public void act_SeitschrittLinks()
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        GameObject go = GameObject.Find("KranHeadParent");
        if (go != null)
        {
            go.GetComponent<moveCrane>().moveLeft();
            reptDone_SeitschrittLinks++;
            if (reptDone_SeitschrittLinks - 1 < reptReq_SeitschrittLinks)
            {
                GameObject.Find("SideStep_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
                if (reptDone_SeitschrittLinks >= reptReq_SeitschrittLinks)
                    GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
                else
                    GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
            }
        }
        logger += Time.time + ";sideStep_left\r\n";
    }

    public void act_Hueftkreisen()
    {
        if (reptDone_Rumpfrotation < reptReq_Rumpfrotation)
            return;
        windForce = windForce += 1;

        reptDone_Hueftkreisen++;
        if (reptDone_Hueftkreisen - 1 < reptReq_Hueftkreisen)
        {
            GameObject.Find("HipCirculation_currentlyExecuting").GetComponent<Animator>().Play("RepetitionShine");
            if (reptDone_Hueftkreisen >= reptReq_Hueftkreisen && buildingsRequired_WindWheel == 0)
            {
                GameObject.Find("AudioController_ExerciseFinished").GetComponent<AudioSource>().Play();
                if(week < 2)
                {
                    displayMessage("Klasse! Die Windmühle ist in vollem Einsatz!", 5, "");
                    playVoiceOver("1_task_3_done");
                }
                else
                {
                    displayMessage("Klasse! Die Windräder bringen ordentlich Strom!", 5, "");
                    playVoiceOver("2_task_2_done");
                }

                checkMissionProgress();
            }
            else
                GameObject.Find("AudioController_ExerciseRepetitions").GetComponent<AudioSource>().Play();
        }

        logger += Time.time + ";hipCirculation\r\n";

    }

    public void initLadescreen()
    {
        GameObject.Find("CanvasLadebildschirm_Content").GetComponent<RectTransform>().localScale = Vector3.one;
        playVoiceOver("0_welcome");
        Invoke("LadescreenTeilaufgabe1FadeIn", 5);
        Invoke("LadescreenTeilaufgabe2FadeIn", 9);
        Invoke("LadescreenTeilaufgabe3FadeIn", 13);
        //Invoke("LadescreenBlackFade", 10);
    }

    public void LadescreenTeilaufgabe1FadeIn()
    {
        GameObject.Find("Teilaufgabe1").GetComponent<Animator>().Play("Teilaufgabe1FadeIn");
        if (week < 2)
            playVoiceOver("1_task_1");
        else
            playVoiceOver("2_task_1");
    }
    public void LadescreenTeilaufgabe2FadeIn()
    {
        GameObject.Find("Teilaufgabe2").GetComponent<Animator>().Play("Teilaufgabe2FadeIn");
        if (week < 2)
            playVoiceOver("1_task_2");
        else
            playVoiceOver("2_task_2");
    }
    public void LadescreenTeilaufgabe3FadeIn()
    {
        GameObject.Find("Teilaufgabe3").GetComponent<Animator>().Play("Teilaufgabe3FadeIn");
        if (week < 2)
            playVoiceOver("1_task_3");
        else
            playVoiceOver("2_task_3");
    }

    public void LadescreenBlackFade()
    {
        GameObject.Find("BlackFade").GetComponent<Animator>().Play("LadebildschirmBlackFade");
        Invoke("destroyLadescreen", 0.75f);
    }

    public void destroyLadescreen()
    {
        if (GameObject.Find("CanvasLadebildschirm_Content") != null)
        {
            Destroy(GameObject.Find("CanvasLadebildschirm_Content"));
            welcomeMessage();
        }
    }

    public void displayMessage(string message, float displayedTime, string EndFunction)
    {
        _endFunction = EndFunction;

        speechBubbleTimeout = Time.time + displayedTime;

        if (MayorCamera == null)
            MayorCamera = GameObject.Find("MayorCamera");
        MayorCamera.SetActive(true);

        GameObject.Find("SpeechBubble").GetComponent<RectTransform>().localScale = Vector3.one;
        GameObject.Find("SpeechBubbleText").GetComponent<Text>().text = message;

        Invoke("endMessage", displayedTime);
    }

    public void endMessage()
    {
        if (Time.time + 1 < speechBubbleTimeout)
            return;

        if (MayorCamera == null)
            MayorCamera = GameObject.Find("MayorCamera");
        MayorCamera.SetActive(false);

        GameObject.Find("SpeechBubble").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("SpeechBubbleText").GetComponent<Text>().text = "";
        if (_endFunction.Length > 0)
            SendMessage(_endFunction);
    }

    public void holzTutorialEnde()
    {
        //CameraLerpToDefault();
        GameObject.Find("RedArrow_3").GetComponent<RectTransform>().localScale = Vector3.zero;
        displayMessage("Wenn Du möchtest, dass sich deine Dorfbewohner schneller bewegen, wende \"Gehen auf der Stelle\" an.", 6, "GehenTutorialEnde");
        playVoiceOver("1_intro_walkOnTheSpot");
        GameObject.Find("WalkOnTheSpot").GetComponent<Animator>().Play("Exercise_RollIn");
        GameObject.Find("RedArrow_4").GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void GehenTutorialEnde()
    {
        GameObject.Find("RedArrow_4").GetComponent<RectTransform>().localScale = Vector3.zero;
        CameraLerpToDefault();
    }

    public void TutorialSideStepEnd()
    {
        GameObject.Find("RedArrow_5").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void checkMissionProgress()
    {
        int mission1 = 3;
        if (week < 2)
        {
            GameObject go = GameObject.Find("HouseTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_Houses;
                if (buildingsRequired_Houses <= 0)
                {
                    GameObject.Find("HouseTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                    mission1--;
                }
            }
            go = GameObject.Find("FieldTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_Fields;
                if (buildingsRequired_Fields == 0)
                    if (fieldworkRequired == 0)
                    {
                        GameObject.Find("FieldTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                        go.GetComponent<Text>().text = "";
                        mission1--;
                    }
                    else
                        go.GetComponent<Text>().text = "Bewässern (Rumpfbeuge).";
            }
            go = GameObject.Find("MillTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_Mills;
                if (buildingsRequired_Mills == 0)
                    if (reptDone_Hueftkreisen >= reptReq_Hueftkreisen)
                    {
                        GameObject.Find("MillTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                        go.GetComponent<Text>().text = "";
                        mission1--;
                    }
                    else
                        go.GetComponent<Text>().text = "Antreiben (Hüftkreisen).";
            }
        }
        else
        {
            // mission week 2:

            GameObject go = GameObject.Find("HouseTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_Solar;
                if (buildingsRequired_Solar <= 0)
                {
                    GameObject.Find("HouseTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                    mission1--;
                }
            }
            go = GameObject.Find("FieldTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_WindWheel;
                if (buildingsRequired_WindWheel == 0)
                    if (reptDone_Hueftkreisen >= reptReq_Hueftkreisen)
                    {
                        GameObject.Find("FieldTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                        go.GetComponent<Text>().text = "";
                        mission1--;
                    }
                    else
                        go.GetComponent<Text>().text = "Antreiben (Hüftkreisen).";
            }
            go = GameObject.Find("MillTodoText");
            if (go != null)
            {
                go.GetComponent<Text>().text = "x" + buildingsRequired_WaterTower;
                if (buildingsRequired_WaterTower == 0)
                    if (timeDone_Rumpfbeuge >= timeReq_Rumpfbeuge)
                    {
                        GameObject.Find("MillTodoDone").GetComponent<RectTransform>().localScale = Vector3.one;
                        go.GetComponent<Text>().text = "";
                        mission1--;
                    }
                    else
                        go.GetComponent<Text>().text = "Auffüllen (Rumpfbeuge).";
            }
        }

        if (mission1 == 0)
        {
            Debug.Log("Mission done!");
            mission1done = true;
            displayMessage("Hurra! Du hast alle Aufgaben gemeistert!\r\nDie Bewohner versammeln sich auf dem Dorfplatz, um Dir zu danken!", 8, "");
            playVoiceOver("0_all_tasks_done");
            globalMeetingDorfzentrum = true;
            CameraStartPosition = mainCam.transform.position;
            CameraTargetPosition = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.position;
            CameraStartRotation = mainCam.transform.rotation.eulerAngles;
            CameraTargetRotation = GameObject.Find("TownHall(Clone)").GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
            lerpProgress = 0f;
            lerpMultiplicator = 0.05f;
            CameraLerping = true;

            //GameObject.Find("Holo").GetComponent<SpriteRenderer>().enabled = true;
            Invoke("fadeEndscreen", 20);
        }
    }

    public void checkTutorialAfterBuildingFinished()
    {
        if (buildingsRequired_Fields == 0 && !rainTutorialDone)
        {
            displayMessage("Felder produzieren Getreide, allerdings nur, wenn sie gegossen wurden.\r\nFühre die Rumpfbeuge aus, um es regnen zu lassen.", 9, "rainTutorialFinish");
            playVoiceOver("1_intro_bodyBend");
            GameObject.Find("BodyBend").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("RedArrow_6").GetComponent<RectTransform>().localScale = Vector3.one;
            rainTutorialDone = true;
        }
        else
            if (buildingsRequired_Mills == 0 && !windTutorialDone)
        {
            displayMessage("Deine Windmühle funktioniert natürlich nur mit Wind.\r\nErzeuge welchen, indem Du das Hüftkreisen anwendest!", 8, "windTutorialFinish");
            playVoiceOver("1_intro_hipCirculation");
            GameObject.Find("HipCirculation").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("RedArrow_8").GetComponent<RectTransform>().localScale = Vector3.one;
            windTutorialDone = true;
        }
        else
                    if (!aufgabenTutorialDone)
        {
            displayMessage("Konzentriere Dich nun weiter auf die Aufgaben, die vor Dir liegen.", 5, "aufgabenTutorialFinish");
            playVoiceOver("0_proceed");
            GameObject.Find("RedArrow_7").GetComponent<RectTransform>().localScale = Vector3.one;
            aufgabenTutorialDone = true;
        }
    }

    public void rainTutorialFinish()
    {
        GameObject.Find("RedArrow_6").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    public void aufgabenTutorialFinish()
    {
        GameObject.Find("RedArrow_7").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    public void windTutorialFinish()
    {
        GameObject.Find("RedArrow_8").GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void fieldworkDone()
    {
        if (fieldworkRequired > 0)
        {
            displayMessage("Deine Dorfbewohner haben das Feld erfolgreich geerntet!", 5, "CameraLerpToDefault");
            playVoiceOver("1_task_2_done");
            fieldworkRequired--;
        }
        checkMissionProgress();
    }

    public void tagnachtTutorialDone()
    {
        GameObject.Find("RedArrow_9").GetComponent<RectTransform>().localScale = Vector3.zero;
        GameObject.Find("RedArrow_10").GetComponent<RectTransform>().localScale = Vector3.zero;
    }
    public void NightTutorial2()
    {
        displayMessage("Wenn der Mond am höchsten Punkt steht, wechsle wieder das Bein, um ihn untergehen zu lassen.", 7, "");
        playVoiceOver("1_intro_standingOneLegged_3");
    }

    public void villagerApproachingField(GameObject field)
    {
        if (!villagerApproachingFieldDone)
        {
            villagerApproachingFieldDone = true;
            CameraLerpTo(field);
        }
    }

    public void CameraLerpTo(GameObject target)
    {
        CameraStartPosition = mainCam.transform.position;
        CameraTargetPosition = target.GetComponentInChildren<Camera>().transform.position;
        CameraStartRotation = mainCam.transform.rotation.eulerAngles;
        CameraTargetRotation = target.GetComponentInChildren<Camera>().transform.rotation.eulerAngles;
        lerpProgress = 0f;
        CameraLerping = true;
        CameraAway = true;
    }

    public void fadeEndscreen()
    {
        GameObject.Find("BlackFade").GetComponent<Animator>().Play("LadebildschirmBlackFade");
        Invoke("showEndscreen", 0.75f);
    }

    public void showEndscreen()
    {
        GameObject.Find("CanvasEndbildschirm_Content").GetComponent<RectTransform>().localScale = Vector3.one;
        playVoiceOver("0_all_tasks_done_2");
        File.WriteAllText(System.DateTime.Now.Year.ToString() +"-"+ System.DateTime.Now.Month.ToString()+"-"+System.DateTime.Now.Day.ToString()+"."+ System.DateTime.Now.Hour.ToString()+"."+ System.DateTime.Now.Minute.ToString()+"_log.csv", logger);
    }

    public void showSelectionCam()
    {
        GameObject.Find("BlackFade").GetComponent<Animator>().Play("LadebildschirmBlackFade");
        Invoke("showSelectionCam2", 0.75f);
    }

    public void showSelectionCam2()
    {
        mainCam.SetActive(false);
        selectionCam.SetActive(true);
        isSelecting = true;

        GameObject.Find("selectionCamRightEntry_Field").transform.localScale = Vector3.zero;
        GameObject.Find("selectionCamRightEntry_Windmill").transform.localScale = Vector3.zero;
        GameObject.Find("selectionCamRightEntry_House").transform.localScale = Vector3.zero;

        if (week < 2)
        {
            selectionCamLeftEntry = Spawnable.BuildingType.House;
            if (buildingsRequired_Fields > 0)
            {
                selectionCamRightEntry = Spawnable.BuildingType.Field;
                GameObject.Find("selectionCamRightEntry_Field").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Feld";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Erzeugt nach Regen Getreide, das in einer Windmühle gemahlen werden kann.";
            }
            else
                if (buildingsRequired_Mills > 0)
            {
                selectionCamRightEntry = Spawnable.BuildingType.WindMill;
                GameObject.Find("selectionCamRightEntry_Windmill").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Windmühle";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Mahlt Getreide, wenn Wind weht.";
            }
            else
            {
                selectionCamRightEntry = Spawnable.BuildingType.House;
                GameObject.Find("selectionCamRightEntry_House").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Haus";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Ein neuer Bewohner schließt sich dem Dorf an.";
            }
        }
        else
        {
            // week 2 buildings
            GameObject.Find("selectionCamRightEntry_Solar").transform.localScale = Vector3.zero;
            GameObject.Find("selectionCamRightEntry_WindWheel").transform.localScale = Vector3.zero;
            GameObject.Find("selectionCamRightEntry_WaterTower").transform.localScale = Vector3.zero;
            GameObject.Find("Alternative1Text").GetComponent<Text>().text = "Solaranlage";
            GameObject.Find("Alternative1Subtext").GetComponent<Text>().text = "Erzeugt Strom, wenn die Sonne scheint.";

            selectionCamLeftEntry = Spawnable.BuildingType.Solar;
            if (buildingsRequired_WindWheel > 0)
            {
                selectionCamRightEntry = Spawnable.BuildingType.WindWheel;
                GameObject.Find("selectionCamRightEntry_WindWheel").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Windrad";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Erzeugt Strom, wenn Wind weht.";
            }
            else
            if (buildingsRequired_WaterTower > 0)
            {
                selectionCamRightEntry = Spawnable.BuildingType.WaterTower;
                GameObject.Find("selectionCamRightEntry_WaterTower").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Wasserturm";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Speichert Regenwasser.";
            }
            else
            {
                selectionCamRightEntry = Spawnable.BuildingType.Solar;
                GameObject.Find("selectionCamRightEntry_Solar").transform.localScale = Vector3.one;
                GameObject.Find("Alternative2Text").GetComponent<Text>().text = "Solaranlage";
                GameObject.Find("Alternative2Subtext").GetComponent<Text>().text = "Erzeugt Strom, wenn die Sonne scheint.";
            }
        }
        
        if(!tutorial_chooseSelection)
        {
            displayMessage("Du hast genügend Holz gesammelt, um ein Gebäude zu errichten!\r\nUm auszuwählen, bewege Deine Hand zur rechten oder linken Seite.", 9, "");
            playVoiceOver("0_construction_select");
            tutorial_chooseSelection = true;
        }
    }

    public void selectionCam_chooseLeft()
    {
        if(week < 2)
        {
            // spawn house, TODO: modular
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = (int)getTerrainHeight(loc[0], loc[1]);
            SpawnableHouse house = spawnHouse(loc[0], y, loc[1]);
            isSelecting = false;
            selectionCam.SetActive(false);
            mainCam.SetActive(true);

            CameraLerpTo(house.gameObject);

            currentlyUnderConstruction = true;
            addRohstoffe(-100);

            if (!GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().tutorial_CraneDisplayed)
            {
                displayMessage("Neue Gebäude werden mit dem Kran gebaut. Führe den Seitschritt abwechselnd nach links und rechts aus, um ihn zu bedienen.", 8, "TutorialSideStepEnd");
                playVoiceOver("1_intro_sideStep");

                GameObject.Find("SideStep").GetComponent<Animator>().Play("Exercise_RollIn");
                GameObject.Find("RedArrow_5").GetComponent<RectTransform>().localScale = Vector3.one;

                GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().tutorial_CraneDisplayed = true;
            }
        }
        else
        {
            // week 2: Solars
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = (int)getTerrainHeight(loc[0], loc[1]);
            SpawnableSolar solar = spawnSolar(loc[0], y, loc[1]);
            isSelecting = false;
            selectionCam.SetActive(false);
            mainCam.SetActive(true);

            CameraLerpTo(solar.gameObject);

            currentlyUnderConstruction = true;
            addRohstoffe(-100);
        }
        
    }

    public void selectionCam_chooseRight()
    {
        if (selectionCamRightEntry == Spawnable.BuildingType.House)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = (int)getTerrainHeight(loc[0], loc[1]);
            SpawnableHouse house = spawnHouse(loc[0], y, loc[1]);

            CameraLerpTo(house.gameObject);
        }

        if (selectionCamRightEntry == Spawnable.BuildingType.Field)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            SpawnableField field = spawnField(loc[0], y, loc[1]);

            CameraLerpTo(field.gameObject);
        }


        if (selectionCamRightEntry == Spawnable.BuildingType.WindMill)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            SpawnableWindmill windmill = spawnWindmill(loc[0], y, loc[1]);


            CameraLerpTo(windmill.gameObject);
        }

        if (selectionCamRightEntry == Spawnable.BuildingType.WaterTower)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            SpawnableWaterTower wt = spawnWaterTower(loc[0], y, loc[1]);
            
            CameraLerpTo(wt.gameObject);
        }
        if (selectionCamRightEntry == Spawnable.BuildingType.Solar)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            SpawnableSolar solar = spawnSolar(loc[0], y, loc[1]);
            
            CameraLerpTo(solar.gameObject);
        }
        if (selectionCamRightEntry == Spawnable.BuildingType.WindWheel)
        {
            int[] loc = findSpaceForSpawnable(11, 11, 10, 10);
            int y = 0;
            if (landscapeType == LandscapeType.valley)
                y = (int)getTerrainHeight(loc[0], loc[1]);
            if (landscapeType == LandscapeType.island)
                y = (int)islandHeight[loc[0], loc[1]];
            SpawnableWindTurbine wt = spawnWindTurbine(loc[0], y, loc[1]);
            
            CameraLerpTo(wt.gameObject);
        }

        currentlyUnderConstruction = true;
        addRohstoffe(-100);
        isSelecting = false;
        selectionCam.SetActive(false);
        mainCam.SetActive(true);

        if (!GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().tutorial_CraneDisplayed)
        {
            displayMessage("Neue Gebäude werden mit dem Kran gebaut. Führe den Seitschritt abwechselnd nach links und rechts aus, um ihn zu bedienen.", 5, "TutorialSideStepEnd");
            playVoiceOver("1_intro_sideStep");

            GameObject.Find("SideStep").GetComponent<Animator>().Play("Exercise_RollIn");
            GameObject.Find("RedArrow_5").GetComponent<RectTransform>().localScale = Vector3.one;

            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().tutorial_CraneDisplayed = true;
        }
    }

    Vector3 getAdaptiveCameraPosition()
    {
        float percentage = GameObject.Find("KinectGetDistance").GetComponent<KinectGetDistance>()._distance;
        // transform 0...3 to 0...100%
        if (percentage < 1)
            percentage = 1;
        if (percentage > 4)
            percentage = 4;
        percentage = percentage - 1;
        percentage *= 33.33f / 100f;

        // 6.32     24.3     -6.7     ganz nah dran
        //Vector3 nah = new Vector3(6.32f, 24.3f, -6.7f);
        Vector3 nah = new Vector3(228.5482f, 53.57352f, 177.8844f);
        //TODO: set = townHall
        // 191.9534 136.4338 85.67752 ganz weit weg
        Vector3 weit = new Vector3(144.3185f, 203.8974f, 76.19938f);

        return Vector3.Lerp(nah, weit, percentage);
    }

    void playVoiceOver(string filename)
    {
        if(!muteVoiceOver)
        {
            Debug.Log(filename);
            AudioClip clip = Resources.Load<AudioClip>("VoiceOver/"+ filename);
            GameObject.Find("AudioController").GetComponent<AudioSource>().Stop();
            GameObject.Find("AudioController").GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }

}
