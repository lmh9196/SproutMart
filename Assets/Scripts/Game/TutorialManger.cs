using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManger : MonoBehaviour
{
    public static TutorialManger instance = null;
    public WorldGuideArrow playerGuideArrow;
    DialogueTerm term = new();
    MainListMenu mainListMenu; 
    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainListMenu = MenuManager.instance.mainInputMenu;
    }

    private void Update()
    {
        if (!GameManager.instance.checkList.IsTutorial_UpgradeMenu) { TutorialUpgradeMenu(); }
    }

    public void ActiveTargetNoticeArrow() { playerGuideArrow.gameObject.SetActive(true); }
    public void DiableTargetNoticeArrow() 
    {
        playerGuideArrow.InitArrow(false);
        playerGuideArrow.gameObject.SetActive(false);
    }
    public void ArrowTargetRotate(Transform target)
    {
        Vector3 dir = (playerGuideArrow.transform.position - target.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerGuideArrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void LockTargetPos(Transform transform)
    {
        Vector3 transformPos = Camera.main.WorldToViewportPoint(transform.position);

        transformPos.x = Mathf.Clamp(transformPos.x, 0.1f, 0.9f);
        transformPos.y = Mathf.Clamp(transformPos.y, 0.1f, 0.9f);

        transform.transform.position = Camera.main.ViewportToWorldPoint(transformPos);
    }

    public void DistanceTutorial(bool isIf, Transform target ,float weight, Action act)
    {
        if (!isIf) { return; }
        else if (MainCamera.instance.TargetCamInCheck(target, weight)) { act?.Invoke(); }
    }

    public void TutorialUpgradeMenu()
    {
        if (GameManager.instance.data.gold > 30 && GameManager.instance.checkList.IsTutorialEnd)
        {
            mainListMenu.EnableNotice(mainListMenu.FindBtn(mainListMenu.upgradeBtn));
            DialogueManager.instance.EnableDialouge(term.termUpgrade_GuideUpgradeMenu, true, false);
        }
    }
}
