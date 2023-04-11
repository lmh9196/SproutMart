using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTable : Table
{
    public GameObject tableObj;

    [Space(20f)]
    public GetArea getArea;
    public float respawnTime;
    public override void Awake()
    {
        base.Awake();

        UnlockInit();


        getArea.InitTableArea(this);

        createWork.Init(getArea, respawnTime);
    }


    public override void Start() 
    {
        base.Start();

        RegistNeedTable(this);
    }
    public virtual void Update() 
    {
        UpdateUnlock(tableObj, lockArea.gameObject);

        if (data.IsUnlock) { createWork.Work(); }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
