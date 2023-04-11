using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierStaff : Staff
{
    public override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        SetStartPos();
    }
    void Update()
    {
        SetAnimation();
    }

    public override void SetAnimation()
    {
        animDir.lookDir = Vector3.up;
        base.SetAnimation();
    }
    
}
