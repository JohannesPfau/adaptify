//ropeJS by youtube.com/vulgerstal
//How to use:
//1) Drag and Drop RopeJS Prefab

#pragma strict

var parent : boolean;
var connectTo : Transform;

function Start () {

gameObject.AddComponent.<CharacterJoint>();
if(!parent) { GetComponent.<CharacterJoint>().connectedBody=transform.parent.GetComponent.<Rigidbody>();
transform.parent = null; }


        GetComponent.<CharacterJoint>().enableCollision = true;
        GetComponent.<CharacterJoint>().enableProjection = true;
        GetComponent.<CharacterJoint>().projectionDistance = 0.5f;
		
		var s : SoftJointLimitSpring = SoftJointLimitSpring();
		var s1 : SoftJointLimitSpring = SoftJointLimitSpring();
        s.spring = 33;
        s1.spring = 1;
        var s2a : SoftJointLimit =	 SoftJointLimit();
        s2a.limit = 1;
        var s2 : SoftJointLimit =	 SoftJointLimit();
        s2.limit = 33;
        
        GetComponent.<CharacterJoint>().twistLimitSpring = s1;
        GetComponent.<CharacterJoint>().lowTwistLimit = s2a;
        GetComponent.<CharacterJoint>().highTwistLimit = s2a;
        GetComponent.<CharacterJoint>().swingLimitSpring = s;
        GetComponent.<CharacterJoint>().swing1Limit = s2;
        GetComponent.<CharacterJoint>().swing2Limit = s2;

}

function Update()
{
if(parent && connectTo != null)transform.position=connectTo.position;
}
