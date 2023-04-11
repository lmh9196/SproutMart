using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CreatorData", order = 8)]
public class CreatorData : ScriptableObject
{
    [Header("Table")]

    public GameObject cropsPre;

    public int currentCount;
    public int maxCount;

    public float createTime;
}
