using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimationsCS : MonoBehaviour {

    public class eyeExpressions
    {
        public GameObject rightEye;
        public GameObject leftEye;
    }
    private int curEye;
    private int oldEye;
    private int curMouth;
    private int oldMouth;

    public GameObject[] faceExpressionsLeft;
    public GameObject[] faceExpressionsRight;

    public GameObject[] mouths;

    public void Start()
    {
        curEye = 0;
        oldEye = 0;
        curMouth = 0;
        oldMouth = 0;
        if (faceExpressionsLeft.Length > 0)
        {
            faceExpressionsLeft[0].SetActive(true);
            faceExpressionsRight[0].SetActive(true);
        }
        if (mouths.Length > 0)
        {
            mouths[0].SetActive(true);
        }
    }

    public void Update()
    {

    }
    public void switchMouthExpressions(int prevMouth, int nextMouth)
    {
        mouths[prevMouth].SetActive(false);
        mouths[nextMouth].SetActive(true);
    }
    public void switchEyeExpressions(int prevEye, int nextEye)
    {
        faceExpressionsLeft[prevEye].SetActive(false);
        faceExpressionsRight[prevEye].SetActive(false);

        faceExpressionsLeft[nextEye].SetActive(true);
        faceExpressionsRight[nextEye].SetActive(true);
    }

}
