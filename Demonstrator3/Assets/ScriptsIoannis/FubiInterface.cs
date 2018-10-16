using UnityEngine;
using System.Collections;
using FubiNET;

// Created by Ioannis Bikas in cooperation with Johannes Pfau at University of Bremen
// for the project Adaptify 2017.
public class FubiInterface : MonoBehaviour {

	#region Variables and Stuff

	// General properties
	public bool DisableFubi = false;

	// BoxenImStand index= 0
	// DiagonaleRumpfbeuge index= 1 (L) und 2 (R)
	// EinbeinStandLinks index = 3 (L) und 4 (R)
	// GehenAufDerStelle index = 5
	// Hueftkreisen index = 6 (L) und 7 (R)
	// Rumpfbeuge index = 8
	// Rumpfrotation index = 9 (RL) und 10 (LR)
	// Seitneige index = 11 (L) und 12 (R)
	// Seitschritt index = 13
	public string[] GestureRecognizerNames;

	public string GestureRecognizerXML = "UnitySampleRecognizers.xml";

	public string currentlySelectedRecognizer = "UnitySampleRecognizers.xml";

	// FubiUnity is a singleton with the instance here
	static FubiInterface instance;

	// The current user for all controls
	uint m_currentUser;

	#endregion

	#region Initialization

	// Use this for initialization
	void Start () {

		// Initialising GestureRecognizerNames. If one wishes to register a new recognizer to the system
		// please add it here to integrate it into the system.
		GestureRecognizerNames = new string[14] 
		{
			"BoxenImStand.xml",
			"DiagonaleRumpfbeugeLinks.xml",
			"DiagonaleRumpfbeugeRechts.xml",
			"EinBeinStandLinksOhneArmTracking.xml",
			"EinBeinStandRechtsOhneArmTracking.xml",
			"GehenAufDerStelleRechtsLinks.xml",
			"HueftkreisenLinks.xml",
			"HueftkreisenRechts.xml",
			"Rumpfbeuge.xml",
			"RumpfrotationLinksRechts.xml",
			"RumpfrotationRechtsLinks.xml",
			"SeitneigeLinks.xml",
			"SeitneigeRechts.xml",
			"Seitschritt.xml"
		};

		// First set instance so Fubi.release will not be called while destroying old objects
		instance = this;
		// Remain this instance active until new one is created
		DontDestroyOnLoad(this);

		// Destroy old instance of Fubi
		object[] objects = GameObject.FindObjectsOfType(typeof(FubiInterface));
		if (objects.Length > 1)
			Destroy(((FubiInterface)objects[0]));

		// Init FUBI
		if (!DisableFubi)
		{
			// Only if not already done
			if (!Fubi.isInitialized())
			{
				// Load one of the available sensors
				FubiUtils.SensorType sensorToLoad = FubiUtils.SensorType.NONE;
				var avSensors = Fubi.getAvailableSensors();
				if ((avSensors & (int)FubiUtils.SensorType.KINECTSDK2) != 0)
					sensorToLoad = FubiUtils.SensorType.KINECTSDK2;
				else if ((avSensors & (int)FubiUtils.SensorType.OPENNI2) != 0)
					sensorToLoad = FubiUtils.SensorType.OPENNI2;
				else if ((avSensors & (int)FubiUtils.SensorType.KINECTSDK) != 0)
					sensorToLoad = FubiUtils.SensorType.KINECTSDK;
				else if ((avSensors & (int)FubiUtils.SensorType.OPENNI1) != 0)
					sensorToLoad = FubiUtils.SensorType.OPENNI1;
				if (!Fubi.init(new FubiUtils.SensorOptions(new FubiUtils.StreamOptions(), new FubiUtils.StreamOptions(),
					new FubiUtils.StreamOptions(-1), sensorToLoad), new FubiUtils.FilterOptions()))
				{
					Debug.Log("Fubi: FAILED to initialize Fubi!");
				}
				else
				{
					Debug.Log("Fubi: initialized!");
				}
			}
		}

		if (Fubi.isInitialized())
		{
			// Clear old gesture recognizers
			Fubi.clearUserDefinedRecognizers();

			// And (re)load them
			if (Fubi.loadRecognizersFromXML(GestureRecognizerXML))
				Debug.Log("Fubi: gesture recognizers loaded from '" + GestureRecognizerXML + "'!");
			else
				Debug.Log("Fubi ERROR: FAILED loading recognizer from '" + GestureRecognizerXML + "'!");
		}
        


    }

	#endregion

	// Update is called once per frame
	void Update () {

		// Update FUBI
		Fubi.updateSensor();	

		// Take closest user
		uint userID = Fubi.getClosestUserID();
		if (userID != m_currentUser)
		{
			m_currentUser = userID;
		}

        if(CheckForGivenGesture("BoxenImStand.xml"))
        {
            //Debug.Log("BoxenImStand recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_BoxenImStand(1);
        }

        if (CheckForGivenGesture("DiagonaleRumpfbeugeLinks.xml"))
        {
            //Debug.Log("DiagonaleRumpfbeugeLinks recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_DiagonaleRumpfbeugeLinks();
        }
        if (CheckForGivenGesture("DiagonaleRumpfbeugeRechts.xml"))
        {
            //Debug.Log("DiagonaleRumpfbeugeRechts recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_DiagonaleRumpfbeugeRechts();
        }
        if (CheckForGivenGesture("EinBeinStandLinksOhneArmTracking.xml"))
        {
            //Debug.Log("EinBeinStandLinksOhneArmTracking recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_EinBeinStandLinks(1);
        }
        if (CheckForGivenGesture("EinBeinStandRechtsOhneArmTracking.xml"))
        {
            //Debug.Log("EinBeinStandRechtsOhneArmTracking recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_EinBeinStandRechts(1);
        }
        if (CheckForGivenGesture("GehenAufDerStelleRechtsLinks.xml"))
        {
            //Debug.Log("GehenAufDerStelleRechtsLinks recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_GehenAufDerStelle(1);
        }
        if (CheckForGivenGesture("HueftkreisenLinks.xml"))
        {
            //Debug.Log("HueftkreisenLinks recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_Hueftkreisen();
        }
        if (CheckForGivenGesture("HueftkreisenRechts.xml"))
        {
            //Debug.Log("HueftkreisenRechts recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_Hueftkreisen();
        }
        if (CheckForGivenGesture("Rumpfbeuge.xml"))
        {
            //Debug.Log("Rumpfbeuge recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_Rumpfbeuge(1);
        }
        if (CheckForGivenGesture("RumpfrotationLinksRechts.xml"))
        {
            //Debug.Log("RumpfrotationLinksRechts recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_Rumpfrotation();
        }
        if (CheckForGivenGesture("RumpfrotationRechtsLinks.xml"))
        {
            //Debug.Log("RumpfrotationRechtsLinks recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_Rumpfrotation();
        }
        if (CheckForGivenGesture("SeitneigeLinks.xml"))
        {
            //Debug.Log("SeitneigeLinks recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_SeitneigeLinks();
        }
        if (CheckForGivenGesture("SeitneigeRechts.xml"))
        {
            //Debug.Log("SeitneigeRechts recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_SeitneigeRechts();
        }
        if (CheckForGivenGesture("Seitschritt.xml"))
        {
            //Debug.Log("Seitschritt recognized");
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_SeitschrittLinks();
            GameObject.Find("Demo3CCV").GetComponent<CreateVoxelLandscape>().act_SeitschrittRechts();
        }














    }

	#region Main Functions

	// Loads the given recognizer identified by its name. The name must include the filename-extension:
	// Example input: recognizerName = "MyAwesomeRecognizer.xml". This function clears all currently used
	// recognizers before loading the given one !
	// Returns true, if the recognizer was successfully loaded, false otherwise.
	public bool LoadGivenRecognizerFromXML(string recognizerName){
		if(recognizerName == null){
			Debug.Log ("The given recognizer name was null! Please make sure to use a proper recognizer name!");
			return false;
		}

		if(Fubi.isInitialized()){

			// Clear old gesture recognizers
			Fubi.clearUserDefinedRecognizers ();

			// Try loading the given recognizer file
			if (Fubi.loadRecognizersFromXML (recognizerName)) {
				//Debug.Log ("Fubi: gesture recognizers loaded from '" + recognizerName + "'!");
				currentlySelectedRecognizer = recognizerName;
				return true;
			} else {
				Debug.Log ("Fubi ERROR: FAILED loading recognizer from '" + recognizerName + "'! Please make sure a recognizer with the given name exists!");
				return false;
			}
		}

		return false;
	}

	// Loads the given recognizer identified by its index. The corresponding index can be checked by 
	// reading the GestureRecognizerNames description or by using the getRecognizerIndex(string recognizerName)
	// function. 
	// This function clears all currently used recognizers before loading the given one !
	// Returns true, if the recognizer was succesfully loaded, false otherwise.
	public bool LoadGivenRecognizerByIndex(int recognizerIndex){
		if (recognizerIndex < 0) {
			Debug.Log ("The given recognizer index was a negative number: " + recognizerIndex + " Wut?");
			return false;
		}

		if (recognizerIndex > GestureRecognizerNames.Length) {
			Debug.Log ("The given recognizer index is greater than the number of existing recognizers! #Reigstered recognizers: "
				+ GestureRecognizerNames.Length + " vs Your input: " + recognizerIndex);
			return false;
		}

		// Try loading the given recognizer file
		string recognizerName = GestureRecognizerNames[recognizerIndex];
		return LoadGivenRecognizerFromXML (recognizerName);	
	}

	// Adds the given recognizer identified by its name. The name must include the filename-extension:
	// Example input: recognizerName = "MyAwesomeRecognizer.xml". This function does not clear the currently used recognizers! 
	// Returns true, if the recognizer was successfully loaded, false otherwise.
	public bool AddGivenRecognizerByName(string recognizerName){
		if(recognizerName == null){
			Debug.Log ("The given recognizer name was null! Please make sure to use a proper recognizer name!");
			return false;
		}

		if (Fubi.isInitialized ()) {
			
			// Try loading the given recognizer file
			if (Fubi.loadRecognizersFromXML (recognizerName)) {
				Debug.Log ("Fubi: gesture recognizers loaded from '" + recognizerName + "'!");
				currentlySelectedRecognizer = recognizerName;
				return true;
			} else {
				Debug.Log ("Fubi ERROR: FAILED loading recognizer from '" + recognizerName + "'! Please make sure a recognizer with the given name exists!");
				return false;
			}
		}
		return false;
	}

	// Adds the given recognizer identified by its index if it is registered. A recognizer is registered
	// if it is contained in the GestureRecognizerName Array.
	// This function does not clear the currently used recognizers! 
	// Returns true, if the recognizer was successfully loaded, false otherwise.
	public bool AddGivenRecognizerByIndex(int recognizerIndex){
		if (recognizerIndex < 0) {
			Debug.Log ("The given recognizer index was a negative number: " + recognizerIndex + " Wut?");
			return false;
		}

		if (recognizerIndex > GestureRecognizerNames.Length) {
			Debug.Log ("The given recognizer index is greater than the number of existing recognizers! #Reigstered recognizers: "
				+ GestureRecognizerNames.Length + " vs Your input: " + recognizerIndex);
			return false;
		}

		string recognizerName = GestureRecognizerNames[recognizerIndex];
		return AddGivenRecognizerByName (recognizerName);
	}

	// Loads all registered recognizers at once. A recognizer is registered if it is contained
	// in the GestureRecognizerNames Array.
	// This function clears all currently used recognizers before loading the new ones !
	public void LoadAllRecognizers(){
		if (Fubi.isInitialized ()) {

			// Clear old gesture recognizers
			Fubi.clearUserDefinedRecognizers ();

			for(int i = 0; i < GestureRecognizerNames.Length; i++){
				AddGivenRecognizerByIndex (i);
			}
		}
	}

	// Checks weather or not the given gesture is recognized. The name must include the filename-extension:
	// Example input: recognizerName = "MyAwesomeRecognizer.xml".
	// Returns true if the given gesture was recognized, false otherwise
	public bool CheckForGivenGesture(string recognizerName){
		if(recognizerName == null){
			Debug.Log ("The given recognizer name was null! Please make sure to use a proper recognizer name!");
			return false;
		}

		// Freshly loading recognizr for index consistency
		LoadGivenRecognizerFromXML (recognizerName);

		// Enabeling detection of user defined combination recognizers
		Fubi.enableCombinationRecognition (FubiPredefinedGestures.Combinations.NUM_COMBINATIONS, m_currentUser, true);

        if (Fubi.recognizeGestureOn((uint)0, m_currentUser) == FubiUtils.RecognitionResult.RECOGNIZED)
        {
            Debug.Log(recognizerName + " erkannt !!");
            return true;
        }
        else if (Fubi.recognizeGestureOn((uint)0, m_currentUser) == FubiUtils.RecognitionResult.TRACKING_ERROR)
        {
            Debug.Log("Tracking Error");
            return false;
        }
		return false;
	}

	// Checks weather or not the given gesture is recognized. The name must include the filename-extension:
	// Example input: recognizerName = "MyAwesomeRecognizer.xml".
	// Returns true if the given gesture was recognized, false otherwise
	public bool CheckForGivenGestureWhenAllGesturesAreLoaded(string recognizerName){
		if(recognizerName == null){
			Debug.Log ("The given recognizer name was null! Please make sure to use a proper recognizer name!");
			return false;
		}

		// Freshly loading all recognizers for index consistency
		LoadAllRecognizers ();

		// Enabeling detection of user defined combination recognizers
		Fubi.enableCombinationRecognition (FubiPredefinedGestures.Combinations.NUM_COMBINATIONS, m_currentUser, true);

		// Finding the corresponding index
		int recognizerIndex = GetRecognizerIndexByName (recognizerName);

		if (Fubi.recognizeGestureOn ((uint)recognizerIndex, m_currentUser) == FubiUtils.RecognitionResult.RECOGNIZED) {
			Debug.Log (recognizerName + " erkannt !!");
			return true;
		}
		else if (Fubi.recognizeGestureOn ((uint)recognizerIndex, m_currentUser) == FubiUtils.RecognitionResult.TRACKING_ERROR)
			Debug.Log ("Tracking Error");

		return false;
	}

	#endregion

	#region Utility Functions

	// Returns the index of the given recognizer if the recognizer is registered. A recognizer is 
	// registered if it is contained in the GestureRecognizerNames Array.
	// Returns the recognizers index if found, -1 otherwise
	public int GetRecognizerIndexByName(string recognizerName){
		if(recognizerName == null){
			Debug.Log ("The given recognizer name was null! Please make sure to use a proper recognizer name!");
			return -1;
		}
			
		for(int i = 0; i < GestureRecognizerNames.Length; i++){
			if(GestureRecognizerNames[i].Equals(recognizerName) || GestureRecognizerNames[i].Equals(recognizerName + ".xml")){
				return i;
			}
		}
		return -1;
	}

	// Returns the name of the recognizer that corresponds to the given index if the recognizer is registered. A recognizer is 
	// registered if it is contained in the GestureRecognizerNames Array.
	// Returns the recognizers name without its file extension if found, empty string otherwise
	public string GetRecognizerNameByIndex(int recognizerIndex){
		if (recognizerIndex < 0) {
			Debug.Log ("The given recognizer index was a negative number: " + recognizerIndex + " Wut?");
			return "";
		}

		if (recognizerIndex > GestureRecognizerNames.Length) {
			Debug.Log ("The given recognizer index is greater than the number of existing recognizers! #Reigstered recognizers: "
				+ GestureRecognizerNames.Length + " vs Your input: " + recognizerIndex);
			return "";
		}

		string recognizerName = GestureRecognizerNames [recognizerIndex];

		Debug.Log("Recognizer Name: " + recognizerName + "\r\n Recognizer Index: " + recognizerIndex);
		return recognizerName;
	}
		
	#endregion

	#region Debug Functions

	// Prints a list of the user defined recognizers that are currently used by the Fubi System.
	public void PrintCurrentlyUsedRecognizers(){
		uint i = 0;
		string outputList = "";
		while(!Fubi.getUserDefinedCombinationRecognizerName(i).Equals("")){
			outputList = outputList + Fubi.getUserDefinedCombinationRecognizerName(i) + "\r\n";
			i++;
		}
		if (outputList.Equals (""))
			Debug.Log ("Currently there are no user defined recognizers being used.");
		else
			Debug.Log (outputList);
	}

	#endregion

	// Called on deactivation
	void OnDestroy()
	{
		if (this == instance)
		{
			Fubi.release();
			Debug.Log("Fubi released!");
		}
	}

}
