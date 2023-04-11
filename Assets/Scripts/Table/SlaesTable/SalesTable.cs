using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesTable : Table
{
    
    public GameObject tableObj;

    public Sprite hopeSprite;

    public TableArea salesArea;
    public DetectArea detectArea;

    public Transform lookPoint;
    public ArrangementWayPoint wayPoint;

    public bool isRegistCheck;

    public override void Awake()
    {
        base.Awake();

        UnlockInit();

        salesArea.InitTableArea(this);

        detectArea.Init(this);

        wayPoint.InitWayList(saleWayList, wayPoint.wayPointParent);

        salesWork.Init(this, salesArea, lookPoint, hopeSprite);
    }

    public override void Start() 
    {
        base.Start();

        wayPoint.ActiveWayPoint();
    }

    void Update()
    {
        UpdateUnlock(tableObj, lockArea.gameObject);

        if (salesWork.CheckArea() && data.IsUnlock) { salesWork.Work(); }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (data.IsUnlock) { ReigstStageManager(); }
    }
}
