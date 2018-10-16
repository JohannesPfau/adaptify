#pragma strict
var mySkin : GUISkin;
var rotationSpeed:float=10.0;
var char1:GameObject;
var char2:GameObject;
var char3:GameObject;
var char4:GameObject;
var char5:GameObject;
var char6:GameObject;
var char7:GameObject;
var char8:GameObject;
var char9:GameObject;
var char10:GameObject;
var char11:GameObject;
var char12:GameObject;
var char13:GameObject;
var char14:GameObject;
var char15:GameObject;
var char16:GameObject;

private var curChar:GameObject;
private var curAnimator:Animator;
private var lastChar:GameObject;

var firstView:Transform;
var secondView:Transform;

var cameraMain:Camera;

private var animations1:Character_Animations;
private var animations2:Character_Animations;
private var animations3:Character_Animations;
private var animations4:Character_Animations;
private var animations5:Character_Animations;
private var animations6:Character_Animations;
private var animations7:Character_Animations;
private var animations8:Character_Animations;
private var animations9:Character_Animations;
private var animations10:Character_Animations;
private var animations11:Character_Animations;
private var animations12:Character_Animations;
private var animations13:Character_Animations;
private var animations14:Character_Animations;
private var animations15:Character_Animations;
private var animations16:Character_Animations;

private var anim1:Animator;
private var anim2:Animator;
private var anim3:Animator;
private var anim4:Animator;
private var anim5:Animator;
private var anim6:Animator;
private var anim7:Animator;
private var anim8:Animator;
private var anim9:Animator;
private var anim10:Animator;
private var anim11:Animator;
private var anim12:Animator;
private var anim13:Animator;
private var anim14:Animator;
private var anim15:Animator;
private var anim16:Animator;

//face expressions
private var faceExpression1:FaceAnimations;
private var faceExpression2:FaceAnimations;
private var faceExpression3:FaceAnimations;
private var faceExpression4:FaceAnimations;
private var faceExpression5:FaceAnimations;
private var faceExpression6:FaceAnimations;
private var faceExpression7:FaceAnimations;
private var faceExpression8:FaceAnimations;
private var faceExpression9:FaceAnimations;
private var faceExpression10:FaceAnimations;
private var faceExpression11:FaceAnimations;
private var faceExpression12:FaceAnimations;
private var faceExpression13:FaceAnimations;
private var faceExpression14:FaceAnimations;
private var faceExpression15:FaceAnimations;
private var faceExpression16:FaceAnimations;


private var cameraMove:Vector3;
private var cameraRotate:Quaternion;

private var fullView;

//Eyes and mouths
private var curMouth:int;
private var oldMouth:int;
private var curEye:int;
private var oldEye:int;


function Start () {
	curMouth=0;
	oldMouth=0;
	curEye=0;
	oldEye=0;
	animations1=char1.GetComponent(Character_Animations);
	animations2=char2.GetComponent(Character_Animations);
	animations3=char3.GetComponent(Character_Animations);
	animations4=char4.GetComponent(Character_Animations);
	animations5=char5.GetComponent(Character_Animations);
	animations6=char6.GetComponent(Character_Animations);
	animations7=char7.GetComponent(Character_Animations);
	animations8=char8.GetComponent(Character_Animations);
	animations9=char9.GetComponent(Character_Animations);
	animations10=char10.GetComponent(Character_Animations);
	animations11=char11.GetComponent(Character_Animations);
	animations12=char12.GetComponent(Character_Animations);
	animations13=char13.GetComponent(Character_Animations);
	animations14=char14.GetComponent(Character_Animations);
	animations15=char15.GetComponent(Character_Animations);
	animations16=char16.GetComponent(Character_Animations);
	
	
	//animator controllers
	anim1=char1.GetComponent(Animator);
	anim2=char2.GetComponent(Animator);
	anim3=char3.GetComponent(Animator);
	anim4=char4.GetComponent(Animator);
	anim5=char5.GetComponent(Animator);
	anim6=char6.GetComponent(Animator);
	anim7=char7.GetComponent(Animator);
	anim8=char8.GetComponent(Animator);
	anim9=char9.GetComponent(Animator);
	anim10=char10.GetComponent(Animator);
	anim11=char11.GetComponent(Animator);
	anim12=char12.GetComponent(Animator);
	anim13=char13.GetComponent(Animator);
	anim14=char14.GetComponent(Animator);
	anim15=char15.GetComponent(Animator);
	anim16=char16.GetComponent(Animator);
	
	
	cameraMain=Camera.main;
	cameraMove=firstView.position;
	cameraRotate=firstView.rotation;
	
	curChar=char1;
	curAnimator=curChar.GetComponent(Animator);
	
	fullView=true;
}

function Update () {
	cameraMain.transform.position = cameraMove;
	cameraMain.transform.rotation = cameraRotate;
	if (!fullView){
		if (curChar.transform.position!=Vector3.zero){
			HoldCharOrigin(curChar);
		}	
		cameraMove=Vector3.Lerp(cameraMain.transform.position,secondView.transform.position,Time.deltaTime);
		cameraRotate=Quaternion.Lerp(cameraMain.transform.rotation,secondView.transform.rotation,Time.deltaTime);
		if (Input.GetAxis("Horizontal")){
			curChar.transform.eulerAngles.y+=Input.GetAxis("Horizontal")*rotationSpeed*Time.deltaTime;
			//Debug.Log(curChar.transform.eulerAngles.y);
		}
	}
	if (fullView){
		cameraMove=Vector3.Lerp(cameraMain.transform.position,firstView.transform.position,Time.deltaTime);
		cameraRotate=Quaternion.Lerp(cameraMain.transform.rotation,firstView.transform.rotation,Time.deltaTime);
	}
}
function ResetPosition(CurChar:GameObject){
	CurChar.transform.position=Vector3.zero;
	//CurChar.transform.localEulerAngles=Vector3.zero;
}

function ActivateAnimations(Activate : boolean){
	animations1.enabled=Activate;
	animations2.enabled=Activate;
	animations3.enabled=Activate;
	animations4.enabled=Activate;
	animations5.enabled=Activate;
	animations6.enabled=Activate;
	animations7.enabled=Activate;
	animations8.enabled=Activate;
	animations9.enabled=Activate;
	animations10.enabled=Activate;
	animations11.enabled=Activate;
	animations12.enabled=Activate;
	animations13.enabled=Activate;
	animations14.enabled=Activate;
	animations15.enabled=Activate;
	animations16.enabled=Activate;
	
	anim1.applyRootMotion=Activate;
	anim2.applyRootMotion=Activate;
	anim3.applyRootMotion=Activate;
	anim4.applyRootMotion=Activate;
	anim5.applyRootMotion=Activate;
	anim6.applyRootMotion=Activate;
	anim7.applyRootMotion=Activate;
	anim8.applyRootMotion=Activate;
	anim9.applyRootMotion=Activate;
	anim10.applyRootMotion=Activate;
	anim11.applyRootMotion=Activate;
	anim12.applyRootMotion=Activate;
	anim13.applyRootMotion=Activate;
	anim14.applyRootMotion=Activate;
	anim15.applyRootMotion=Activate;
	anim16.applyRootMotion=Activate;
	
}
function ResetAnimValues(){
	curAnimator.SetFloat("Walk",0.0);
	curAnimator.SetFloat("Run",0.0);
	curAnimator.SetFloat("Turn",0.0);
}
function selectChar(CurChar:GameObject, LastChar:GameObject){
	curAnimator=CurChar.GetComponent(Animator);
	CurChar.transform.position=Vector3.zero;
	CurChar.transform.localEulerAngles=Vector3.zero;
	LastChar.SetActive(false);
	CurChar.SetActive(true);
}
function HoldCharOrigin(CurChar:GameObject){
	CurChar.transform.position=Vector3.zero;
	CurChar.transform.eulerAngles=Vector3.zero;
}

function OnGUI(){
	GUI.skin = mySkin;
	GUI.Label(Rect(65,40,200,30),"Characters");

		
		
	if (fullView){
		if (GUI.Button (Rect (30,205,135,30), "Closer look")){
			fullView=false;
			ActivateAnimations(false);
			ResetAnimValues();
			ResetPosition(curChar);
		}
		GUI.Label(Rect(65,240,200,30),"Instructions");
		GUI.Label(Rect(30,260,140,100),"Use WASD or the arrow keys to move around, hold shift to run, and press space while running to jump");
	}
	else{
		if (GUI.Button (Rect (30,205,135,30), "Full view")){
			fullView=true;
			ActivateAnimations(true);
		}
		if (GUI.Button (Rect(30,240,65,30),"Idle")){
			ResetAnimValues();
		}
		if (GUI.Button (Rect(30,275,65,30),"Walk")){
			curAnimator.SetFloat("Walk",0.5);
			curAnimator.SetFloat("Run",0.0);
		}
		if (GUI.Button (Rect(30,310,65,30),"Run")){
			curAnimator.SetFloat("Walk",0.5);
			curAnimator.SetFloat("Run",0.5);		
		}
		GUI.Label(Rect(65,345,200,30),"Instructions");
		GUI.Label(Rect(30,365,140,60),"Use AD or the arrow keys to rotate the character.");
	}
	if (GUI.Button (Rect (30,65,30,30), "1")){
		lastChar=curChar;
		curChar=char1;
		selectChar(curChar,lastChar);	
	}
	if (GUI.Button (Rect (65,65,30,30), "2")){
		lastChar=curChar;
		curChar=char2;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (100,65,30,30), "3")){
		lastChar=curChar;
		curChar=char3;
		selectChar(curChar,lastChar);	
	}
	if (GUI.Button (Rect (135,65,30,30), "4")){
		lastChar=curChar;
		curChar=char4;
		selectChar(curChar,lastChar);	
	}
	if (GUI.Button (Rect (30,100,30,30), "5")){
		lastChar=curChar;
		curChar=char5;
		selectChar(curChar,lastChar);	
	}
	if (GUI.Button (Rect (65,100,30,30), "6")){
		lastChar=curChar;
		curChar=char6;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (100,100,30,30), "7")){
		lastChar=curChar;
		curChar=char7;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (135,100,30,30), "8")){
		lastChar=curChar;
		curChar=char8;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (30,135,30,30), "9")){
		lastChar=curChar;
		curChar=char9;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (65,135,30,30), "10")){
		lastChar=curChar;
		curChar=char10;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (100,135,30,30), "11")){
		lastChar=curChar;
		curChar=char11;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (135,135,30,30), "12")){
		lastChar=curChar;
		curChar=char12;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (30,170,30,30), "13")){
		lastChar=curChar;
		curChar=char13;
		selectChar(curChar,lastChar);	
	}
	if (GUI.Button (Rect (65,170,30,30), "14")){
		lastChar=curChar;
		curChar=char14;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (100,170,30,30), "15")){
		lastChar=curChar;
		curChar=char15;
		selectChar(curChar,lastChar);		
	}
	if (GUI.Button (Rect (135,170,30,30), "16")){
		lastChar=curChar;
		curChar=char16;
		selectChar(curChar,lastChar);	
	}
	
}