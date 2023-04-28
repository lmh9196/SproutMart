using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TutorialManger : MonoBehaviour
{
    public static TutorialManger instance = null;
    public GameObject playerGuideArrow;
    public GameObject targetGuideArrow;
    FloatAnimation targetGuideArrowAnim;
    DialogueTerm term = new();
    MainListMenu mainListMenu;

    public List<Transform> targetList = new();

    public Dictionary<Transform, Vector3> targetDic = new();
    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);

        targetGuideArrowAnim = targetGuideArrow.transform.GetChild(0).GetComponent<FloatAnimation>();
    }

    private void Start()
    {
        mainListMenu = MenuManager.instance.mainInputMenu;
    }

    private void Update()
    {
        if (!GameManager.instance.checkList.IsTutorial_UpgradeMenu) { TutorialUpgradeMenu(); }


        if (targetDic.Count > 0) { UpdateArrowGuide(); }
        else 
        {
            targetGuideArrow.gameObject.SetActive(false);
            playerGuideArrow.gameObject.SetActive(false);
        }
    }

    public void UpdateArrowGuide()
    {
        bool isEnter;
        if (playerGuideArrow.transform.parent != null)
        {
            KeyValuePair<Transform, Vector3> entry = targetDic.ElementAt(0);

            if (playerGuideArrow.transform.parent != entry.Key)
            {
                targetGuideArrow.transform.SetParent(entry.Key);
                targetGuideArrow.transform.localPosition = entry.Value;
                targetGuideArrow.transform.localScale = new Vector3(0.01f, 0.01f, 1);
            }

            isEnter = MainCamera.instance.TargetCamInCheck(entry.Key, 0.05f) ? true : false;

            CamInOutArrow(isEnter, entry.Key);
        }
    }

    void CamInOutArrow(bool isEnter, Transform target)
    {
        playerGuideArrow.gameObject.SetActive(!isEnter);
        targetGuideArrow.gameObject.SetActive(isEnter);

        targetGuideArrowAnim.enabled = isEnter;

        if (!isEnter) { ArrowTargetRotate(target); }
    }

    public void ActiveTargetNoticeArrow(Transform target, Vector3 spacing) 
    {
        targetDic.Add(target, spacing);
    }
    public void DiableTargetNoticeArrow(Transform target) 
    {
        if (target == null) { targetDic.Remove(targetDic.ElementAt(0).Key); return; }

        if (targetDic.ContainsKey(target)) { targetDic.Remove(target); }
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
            DialogueManager.instance.EnableDialouge(term.termUpgrade_GuideUpgradeMenu, true, true);
            GameManager.instance.canvasList.joyCanvas.enabled = false;
        }
    }

    bool playingTrashTutorial;
    public void TutorialTrashCan(Player player, Transform trashCan)
    {
        if (playingTrashTutorial) { return; };
        if (GameManager.instance.checkList.IsTutorialEnd && !GameManager.instance.checkList.IsTutorial_Full && player.itemBox.childCount == player.charData.maxHandsCount)
        {
            playingTrashTutorial = true;
            ActiveTargetNoticeArrow(trashCan, new Vector3(0, 1f, 0));
            DialogueManager.instance.EnableDialouge(term.termFull_GuideTrash, true, false);
        }
    }
}
