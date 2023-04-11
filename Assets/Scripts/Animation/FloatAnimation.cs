using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float weight;

    Vector3 startPos;
    Vector3 updatePos;

    float maxRange;
    float minRange;

    int num = 1;

    float tempWeight;
    private void Start()
    {
        tempWeight = weight;
        startPos = transform.localPosition;
        updatePos = startPos;
        maxRange = startPos.y + weight;
        minRange = startPos.y - weight;
    }

    void Update()
    { 
        if (updatePos.y > maxRange) num = -1;

        if (updatePos.y < minRange) num = 1;


        updatePos.y += speed * Time.deltaTime * num;
        transform.localPosition = updatePos;



        if (weight != tempWeight)
        {
            maxRange = startPos.y + weight;
            minRange = startPos.y - weight;
        }
        tempWeight = weight;
    }

}
