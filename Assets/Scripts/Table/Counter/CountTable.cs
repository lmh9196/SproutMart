using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTable : Table
{
    public Sprite hopeSprite;

    public AudioSource finishPlayer;
    public AudioSource cropsPlayer;

    public ArrangementWayPoint wayPoint;

    public Transform lookPoint;
    public Transform packBoxPoint;
    public GameObject packBoxPre;

    public DetectArea detectArea;

    protected bool isArrival;

    public ChildCounter childCounter;

    public CharData CashierStaffData;

    public GameObject destroyEffect;
    public override void Awake()
    {
        base.Awake();

        wayPoint.InitWayList(saleWayList, wayPoint.wayPointParent);

        detectArea.Init(this);

        countWork.Init(this, CountAct, finishPlayer, cropsPlayer, lookPoint, packBoxPoint, packBoxPre, hopeSprite);

    }
    public override void Start()
    {
        wayPoint.ActiveWayPoint();
    }

    private void Update()
    {
        isArrival = countWork.CheckArea();

        if (CashierStaffData.SetMenuType(CharData.MenuType.COUNT).Level > 1) { childCounter.gameObject.SetActive(true); }
        else { childCounter.gameObject.SetActive(false); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) || collision.TryGetComponent(out CashierStaff staff)) 
        {
            if (isArrival && !countWork.isCounting) { countWork.Work(); }
        }
    }

    void CountAct() { StartCoroutine(countWork.CountCoroutine(destroyEffect)); }
}
