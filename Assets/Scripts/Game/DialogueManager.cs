using DG.Tweening;
using I2.Loc;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
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
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;



    public Text profileText;
    Transform profileTextParent;
    Localize loc;
    CreatePopUp textPop;


    public bool isAlwaysTrigger;

    public bool isTriggerRunning;
    string tempFixText;

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
        if (profileTextParent.gameObject.activeSelf) { profileText.text = tempFixText; }
    }

    public void EnableDialouge( string term, bool isPop, string addStr = null)
    {
        if (!profileTextParent.gameObject.activeSelf) { profileTextParent.gameObject.SetActive(true); }

        if (isPop) { textPop.ResetPop(); }

        if (term != loc.Term) { loc.Term = term; }
     
        tempFixText = profileText.text + addStr;
    }
    public void DisableDialogue()
    {
        isAlwaysTrigger = false;
        loc.Term = null;
        profileTextParent.gameObject.SetActive(false);
    }


    public void GuideFlashCoroutine(string term) { if (!isTriggerRunning) { StartCoroutine(CoroutineFlashDialogue(term)); } }
    public IEnumerator CoroutineFlashDialogue(string triggerTerm)
    {
        isTriggerRunning = true;

        float timeCount = 0;

        string tempTerm = loc.Term;

        EnableDialouge(triggerTerm, true);


        bool isActiving = profileTextParent.gameObject.activeSelf;

        profileText.DOKill(true);
        profileText.DOColor(ColorManager.instance.textFailColor, 1.0f).SetEase(Ease.Flash, 8, 0);

        while (true)
        {
            if (timeCount > 1.1 || triggerTerm != loc.Term) { profileText.DOKill(true); break; }

            timeCount += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (!isActiving) { DisableDialogue(); }
        else { EnableDialouge(tempTerm, true); }
      
        isTriggerRunning = false;
    }
}
