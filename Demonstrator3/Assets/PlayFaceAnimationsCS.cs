using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFaceAnimationsCS : MonoBehaviour {

    //@script ExecuteInEditMode();
    public enum Eyes_Expressions { Happy = 0, Mad = 1, Sad = 2, Tired = 3, Closed = 4, Closed_happy = 5, Closed_smile = 6, Closed_mad = 7 };
    public enum Mouth_Expressions { Happy_open = 0, Terrified_open = 1, Surprised_open = 2, Surprised2_open = 3, Unconcerned_closed = 4, Sad_closed = 5, Happy_closed = 6, Cute = 7 };
    public Eyes_Expressions Eye;
    public Mouth_Expressions Mouth;

    public FaceAnimationsCS faceAnims;

    public void Start()
    {
        faceAnims = transform.gameObject.GetComponent<FaceAnimationsCS>();
    }

    public void Update()
    {
        if (faceAnims.faceExpressionsLeft.Length > 0)
        {
            if (Eye == Eyes_Expressions.Happy)
            {
                faceAnims.faceExpressionsLeft[0].SetActive(true);
                faceAnims.faceExpressionsRight[0].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[0].SetActive(false);
                faceAnims.faceExpressionsRight[0].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Mad)
            {
                faceAnims.faceExpressionsLeft[1].SetActive(true);
                faceAnims.faceExpressionsRight[1].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[1].SetActive(false);
                faceAnims.faceExpressionsRight[1].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Sad)
            {
                faceAnims.faceExpressionsLeft[2].SetActive(true);
                faceAnims.faceExpressionsRight[2].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[2].SetActive(false);
                faceAnims.faceExpressionsRight[2].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Tired)
            {
                faceAnims.faceExpressionsLeft[3].SetActive(true);
                faceAnims.faceExpressionsRight[3].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[3].SetActive(false);
                faceAnims.faceExpressionsRight[3].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Closed)
            {
                faceAnims.faceExpressionsLeft[4].SetActive(true);
                faceAnims.faceExpressionsRight[4].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[4].SetActive(false);
                faceAnims.faceExpressionsRight[4].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Closed_happy)
            {
                faceAnims.faceExpressionsLeft[5].SetActive(true);
                faceAnims.faceExpressionsRight[5].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[5].SetActive(false);
                faceAnims.faceExpressionsRight[5].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Closed_smile)
            {
                faceAnims.faceExpressionsLeft[6].SetActive(true);
                faceAnims.faceExpressionsRight[6].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[6].SetActive(false);
                faceAnims.faceExpressionsRight[6].SetActive(false);
            }
            if (Eye == Eyes_Expressions.Closed_mad)
            {
                faceAnims.faceExpressionsLeft[7].SetActive(true);
                faceAnims.faceExpressionsRight[7].SetActive(true);
            }
            else
            {
                faceAnims.faceExpressionsLeft[7].SetActive(false);
                faceAnims.faceExpressionsRight[7].SetActive(false);
            }
        }
        if (faceAnims.mouths.Length > 0)
        {
            if (Mouth == Mouth_Expressions.Happy_open) faceAnims.mouths[0].SetActive(true);
            else faceAnims.mouths[0].SetActive(false);
            if (Mouth == Mouth_Expressions.Terrified_open) faceAnims.mouths[1].SetActive(true);
            else faceAnims.mouths[1].SetActive(false);
            if (Mouth == Mouth_Expressions.Surprised_open) faceAnims.mouths[2].SetActive(true);
            else faceAnims.mouths[2].SetActive(false);
            if (Mouth == Mouth_Expressions.Surprised2_open) faceAnims.mouths[3].SetActive(true);
            else faceAnims.mouths[3].SetActive(false);
            if (Mouth == Mouth_Expressions.Unconcerned_closed) faceAnims.mouths[4].SetActive(true);
            else faceAnims.mouths[4].SetActive(false);
            if (Mouth == Mouth_Expressions.Sad_closed) faceAnims.mouths[5].SetActive(true);
            else faceAnims.mouths[5].SetActive(false);
            if (Mouth == Mouth_Expressions.Happy_closed) faceAnims.mouths[6].SetActive(true);
            else faceAnims.mouths[6].SetActive(false);
            if (Mouth == Mouth_Expressions.Cute) faceAnims.mouths[7].SetActive(true);
            else faceAnims.mouths[7].SetActive(false);

        }
    }


}
