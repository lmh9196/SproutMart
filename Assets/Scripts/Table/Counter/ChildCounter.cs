using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ChildCounter : CountTable
{
    bool isChildArrival;
    public override void Awake()
    {
        wayPoint.InitWayList(saleWayList, wayPoint.wayPointParent);

        countWork.Init(this, CountAct, finishPlayer, cropsPlayer, lookPoint, packBoxPoint, packBoxPre, hopeSprite);
    }
    public override void Start()
    {
        wayPoint.ActiveWayPoint();
    }

    private void Update()
    {
        isChildArrival = countWork.CheckArea();

        if (isChildArrival && !countWork.isCounting) { countWork.Work(); }
    }

    void CountAct() { StartCoroutine(countWork.CountCoroutine(destroyEffect)); }
}
