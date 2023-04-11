using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCreateTable : CreateTable
{
    [SerializeField] GameObject readySign;

    public override void Awake() { base.Awake(); }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (createWork.HaveCheck()) { readySign.SetActive(true); }
        else { readySign.SetActive(false); }
    }
}
