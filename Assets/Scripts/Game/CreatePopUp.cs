using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePopUp : MonoBehaviour
{

    public Vector3 defalutScale;
    private void Awake()
    {
        if (defalutScale == Vector3.zero) { defalutScale = transform.localScale; }
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    public void ResetPop()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    private void FixedUpdate()
    {
        if (transform.localScale.x < defalutScale.x) { transform.localScale += new Vector3(defalutScale.x / 10, defalutScale.y / 10, 0); }
        else { transform.localScale = defalutScale; }
    }

}
