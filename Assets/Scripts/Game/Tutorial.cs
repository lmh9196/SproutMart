using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    Player player;

    [SerializeField] GameObject dirCanvas;
    [SerializeField] Transform chaseDir;
    [SerializeField] Transform[] dirTarget;

    [SerializeField] NpcManager npcManager;
    [SerializeField] ThiefEvent thiefEvent;
    int stepCount;
    int tempCount;

    [SerializeField] GameObject dirEffect;
    [SerializeField] GameObject stageEffect;

    [SerializeField] GameObject[] stage1Table;

    [Space(10f)]
    [Header("Target")]
    [SerializeField] GameObject tomatoCreateLock;
    [SerializeField] GameObject tomatoSalesLock;
    [SerializeField] Customer customer;
    [SerializeField] Transform camPoint;

    [Space(20f)]
    [Header("VideoClip")]
    [SerializeField] VideoClip moveGuideClip;
    [SerializeField] VideoClip thiefGuideClip;



    private void Awake() { 
        if(instance == null) instance = this;
       }
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();

        if (GameManager.instance.checkList.IsTutorialEnd)
        {
            gameObject.SetActive(false); return;
        }

        StartCoroutine(Init());
    }

    IEnumerator TomatoSapwnDelay()
    {
        yield return new WaitForSeconds(2f);
        tomatoCreateLock.GetComponentInParent<CreateTable>().createWork.CreateAct();
    }
    private void Update()
    {
       
        tempCount = stepCount;

        switch (stepCount)
        {
            case 0: CheckAct(ActiveCheck(tomatoCreateLock, false));
                break;

            case 1: CheckAct(CountCheck(player.itemBox.childCount
                , player.charData.maxHandsCount)); break;

            case 2: CheckAct(ActiveCheck(tomatoSalesLock , false)); break;

            case 3: CheckAct(CountCheck(player.itemBox.childCount, 0)); break;

            case 4: if(customer.maxHandsCount > 0)
                    CheckAct(CountCheck(customer.itemBox.childCount, customer.maxHandsCount)); break;

            case 5: CheckAct(CountCheck(customer.itemBox.childCount, 0)); break;

            case 6: CheckAct(CountPlusCheck(GameManager.instance.data.gold, 0)); break;

            case 7: CheckAct(MainCamera.instance.TargetCamInCheck(thiefEvent.thief.transform, 0.2f)) ; break;

            case 8: CheckAct(ActiveCheck(thiefEvent.thief, false)); break;
        }

        if (tempCount != stepCount) FinishAct(tempCount);


        if (stepCount < dirTarget.Length) DirSign();
        else chaseDir.gameObject.SetActive(false);
    }

    IEnumerator Init()
    {
        yield return null;
        player.transform.position = new Vector3(5, -1, 0);
        MainCamera.instance.transform.position = new Vector3(5, -1, -10);
        Screen.orientation = ScreenOrientation.Portrait;
        Camera.main.orthographicSize = 10;
        GameManager.instance.canvasList.ActiveTutorialCanvas(true);
        GameManager.instance.data.gold = 50;
        tomatoSalesLock.transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < stage1Table.Length; i++) stage1Table[i].SetActive(false);

        VideoManager.instance.VideoPlay(moveGuideClip, 0);
    }

    Transform SwitchTarget(int step) { return dirTarget[step]; }
    void DirSign()
    {
        chaseDir.parent.position = SwitchTarget(stepCount).position;

        if (CheckCameraOut(SwitchTarget(stepCount), chaseDir))
        {
            LimitCamera(chaseDir, player.transform);
            chaseDir.GetComponent<FloatAnimation>().enabled = false;
        }
        else
        {
            chaseDir.rotation = Quaternion.identity;
            if (!chaseDir.GetComponent<FloatAnimation>().enabled)
            {
                chaseDir.localPosition = Vector3.zero;
                chaseDir.GetComponent<FloatAnimation>().enabled = true;
            }
        }
    }
    void CheckAct(bool isState) { if (isState) stepCount++; }

    void FinishAct(int _stepCount) 
    {
        SoundManager.instance.PlaySfx("RightChoice");

        StartCoroutine(DirDelay());
        switch(_stepCount)
        {
            case 0: StartCoroutine(TomatoSapwnDelay()); break;

            case 1: tomatoSalesLock.transform.parent.gameObject.SetActive(true); break;

            case 3: npcManager.Spawn(1); break;

            case 6: thiefEvent.Spawn(); break;

            case 7: VideoManager.instance.VideoPlay(thiefGuideClip, 0); break;

            case 8: StartCoroutine(End()); break;
        }
    }

    public IEnumerator End()
    {
        dirCanvas.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        MainCamera.instance.CamEvent(camPoint.transform.gameObject, 5f, true);
        yield return new WaitForSeconds(2f);
        Instantiate(stageEffect, camPoint.transform.position, Quaternion.identity);
        SoundManager.instance.PlaySfx("Smoke");

        for (int i = 0; i < stage1Table.Length; i++) stage1Table[i].SetActive(true);
        yield return new WaitForSeconds(2f);


        GameManager.instance.canvasList.ActiveTutorialCanvas(false);
        GameManager.instance.checkList.IsTutorialEnd = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
        Camera.main.orthographicSize = 10;
        gameObject.SetActive(false);
    }

    IEnumerator DirDelay()
    {
        Instantiate(dirEffect, chaseDir.transform.position, Quaternion.identity);
        chaseDir.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        chaseDir.gameObject.SetActive(true);
    }


    Vector3 targetPos;
    Vector3 viewPos;
    Vector3 worldPos;

    
    public bool CheckCameraOut(Transform target, Transform moveCharacter)
    {
        targetPos = Camera.main.WorldToViewportPoint(target.position);
        viewPos = Camera.main.WorldToViewportPoint(moveCharacter.position);

        if (targetPos.x < 0.05f) viewPos.x = 0.05f;

        if (targetPos.x > 0.95f) viewPos.x = 0.95f;

        if (targetPos.y < 0.05f) viewPos.y = 0.05f;

        if (targetPos.y > 0.95f) viewPos.y = 0.95f;

        worldPos = Camera.main.ViewportToWorldPoint(viewPos);

        if (targetPos.x >= 0 && targetPos.x <= 1 &&
targetPos.y >= 0 && targetPos.y <= 1) return false;

        else return true;
    }

    public void LimitCamera(Transform target, Transform player)
    {
        target.position = worldPos;
        float angle = Mathf.Atan2(player.position.y - target.position.y, player.position.x - target.position.x) * Mathf.Rad2Deg;
        target.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }





    public bool ActiveCheck(GameObject targetObj, bool boolHope)
    {
        if (boolHope)
        {
            if (targetObj.activeSelf) return true;
            else return false;
        }
        else
        {
            if (!targetObj.activeSelf) return true;
            else return false;
        }
    }

    public bool CountCheck(int currentCount, int targetCount)
    {
        if (currentCount == targetCount) return true;
        else return false;
    }

    public bool CountPlusCheck(int currentCount, int targetCount)
    {
        if (currentCount > targetCount) return true;
        else return false;
    }

    public bool ArrivalCheck(Transform player, Transform targetForm)
    {
        if (Vector3.Distance(player.position, targetForm.position) < 0.1f) return true;
        else return false;
    }
}
