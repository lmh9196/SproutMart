using DG.Tweening;
using I2.Loc;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTerm
{
    public string termDestroy_Select = "Guide_Destroy_Select";
    public string termDestroy_NotEnough = "Guide_Destroy_NotEnough";

    public string termBuild_ChoicePos = "Guide_Build_Choice";
    public string termBuild_Crash = "Guide_Buuild_Crash";
    public string term_NotEnoughGold = "NotEnoughGold";

    public string termMove_ChoiceObj = "Guide_Move_Choice";

    public string termDrunken_GuideRightWay = "Guide_DrunkenWay";
    public string termDrunken_FirstNotice = "Guide_FirstNotice";

    public string termFull_GuideTrash = "Guide_Trash";

    public string termUpgrade_GuideUpgradeMenu = "Guide_UpgradeMenu";
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    public Text profileText;
    Transform profileTextParent;
    Localize loc;
    CreatePopUp textPop;

    bool isTriggerRunning;

    [SerializeField] List<string> termList = new();
    LocalizedString customString = "Term3";

    private void Awake()
    {
        if (instance == null) { instance = this; }

        profileTextParent = profileText.transform.parent;
        loc = profileText.GetComponent<Localize>();

        textPop = profileTextParent.GetComponent< CreatePopUp>();

        profileTextParent.gameObject.SetActive(false);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (termList.Count > 0) { profileText.text = termList[termList.Count - 1]; }
    }

    public void EnableDialouge(LocalizedString term, bool isPop, bool isInsert, string addStr = null)
    {
        if (termList.Contains(term + addStr)) { return; }

        if (termList.Count == 0) { profileTextParent.gameObject.SetActive(true); }


        Action action = isInsert ? () => 
        { 
            termList.Insert(0, term + addStr);
        }
        : () => 
        { 
            termList.Add(term + addStr);
            if (isPop) { textPop.ResetPop(); }
        };

        action?.Invoke();
    }


    public void DisableDialogue(LocalizedString term,  string addStr = null)
    {
        loc.Term = null;

        termList.Remove(term != null && termList.Contains(term + addStr) ? term + addStr : termList.Last());
        textPop.ResetPop();

        if (termList.Count == 0) { profileTextParent.gameObject.SetActive(false); }
    }


    public void GuideFlashCoroutine(string term) { if (!isTriggerRunning) { StartCoroutine(CoroutineFlashDialogue(term)); } }
    public IEnumerator CoroutineFlashDialogue(string triggerTerm)
    {
        isTriggerRunning = true;

        float timeCount = 0;

        EnableDialouge(triggerTerm, true, false);

        profileText.DOKill(true);
        profileText.DOColor(ColorManager.instance.textFailColor, 1.0f).SetEase(Ease.Flash, 8, 0);

        while (true)
        {
            if (timeCount > 1.1 || triggerTerm != loc.Term) { profileText.DOKill(true); break; }

            timeCount += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
       
        DisableDialogue(triggerTerm);
        isTriggerRunning = false;
    }
}
