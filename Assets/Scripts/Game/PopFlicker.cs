using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PopFlicker : MonoBehaviour
{
    [SerializeField] float flickTime;

    float count;


    public float startDelay;
    float tempStartDelay;

    [SerializeField] GameObject childObj;

    private void Start()
    {
        count = 0;
        childObj.SetActive(false);
        tempStartDelay = startDelay;
    }

    private void Update()
    {
        if (startDelay <= 0)
        {
            if (count < flickTime) count += Time.unscaledDeltaTime;
            else
            {
                count = 0;
                childObj.SetActive(GameManager.instance.ConditionCheck(childObj.activeSelf, true));
            }
        }
        else startDelay -= Time.unscaledDeltaTime;
    }

    private void OnDisable()
    {
        childObj.SetActive(false);
        startDelay = tempStartDelay;
    }
}
