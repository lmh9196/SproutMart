using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGuideArrow : MonoBehaviour
{
    public FloatAnimation floatAnim;
    float yWeight;
    float xWeight;
    public Transform target;

    bool isChase;
    public bool IsChase
    {
        get { return isChase; }
        set 
        {
            if(!value)
            {
                transform.localRotation = Quaternion.identity;
                floatAnim.enabled = true;
            }
            isChase = value; 
        }
    }

    Vector3 startLocalPos;
    private void Start()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
       if(IsChase)
       {
            if (!MainCamera.instance.TargetCamInCheck(target, 0))
            {
                transform.localPosition = startLocalPos;
                TutorialManger.instance.ArrowTargetRotate(target);
                floatAnim.enabled = false;
            }
            else
            {
                transform.position = new Vector3(target.transform.position.x + xWeight, target.transform.position.y + yWeight, 0);

                TutorialManger.instance.playerGuideArrow.transform.localRotation = Quaternion.identity;
                floatAnim.enabled = true;
            }
        }
    }

    public void ChaseOrder(bool isChase , Transform target = null, float xWeight = 0,float yWeight = 0)
    {
        IsChase = isChase;
        this.target = target;
        this.yWeight = yWeight;
        this.xWeight = xWeight;
    }
}
