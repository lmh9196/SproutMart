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
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    public DialogueTerm term = new();


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

    public void EnableDialouge(string term)
    {
        profileTextParent.gameObject.SetActive(true);
        textPop.ResetPop();
        loc.Term = term;
        tempFixText = profileText.text;
    }
    public void DisableDialogue()
    {
        isAlwaysTrigger = false;
        loc.Term = null;
        profileTextParent.gameObject.SetActive(false);
    }

    public void AlwaysDialogue(Action alwaysPrint) 
    {
        if (!isAlwaysTrigger) { isAlwaysTrigger = true; }
        alwaysPrint();
    }
    public void Always_DestroySelect(int selectCount, int totalCount) { profileText.text = tempFixText + " (" + selectCount + "/" + totalCount + ")"; }
    public void AlwaysPrint() { profileText.text = tempFixText; }



    public void GuideTrigger(string term) { if (!isTriggerRunning) { StartCoroutine(CoroutineFlashDialogue(term)); } }
    public IEnumerator CoroutineFlashDialogue(string triggerTerm)
    {
        isTriggerRunning = true;

        float timeCount = 0;

        EnableDialouge(triggerTerm);

        string tempTerm = loc.Term;


        profileText.DOKill(true);
        profileText.DOColor(ColorManager.instance.textFailColor, 1.0f).SetEase(Ease.Flash, 8, 0);

        while(true)
        {
            if (timeCount > 1.1 || tempTerm != loc.Term) {  profileText.DOKill(true); break; }

            timeCount += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (tempTerm == loc.Term) { DisableDialogue(); }
        isTriggerRunning = false;
    }
}
