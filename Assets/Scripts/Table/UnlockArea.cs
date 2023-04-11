using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UnlockArea : MonoBehaviour
{
    [HideInInspector] public Table parentTable;

    [Space(20f)]
    public ParticleSystem unlockedEffect;
    public CreatePopUp create;

    [HideInInspector] public bool isTouch;

    float touchTimer = 0;
    [Space(10f)]
    [SerializeField] Text goldText;
    [SerializeField] Animator anim;

   
    private void Start()
    {
        RegistPriceText(parentTable.data.unlockDefaultCount);
    }

    private void Update()
    {
        UpdateCurrentText();

        if (isTouch)
        {
            touchTimer += Time.deltaTime;
            if (touchTimer >= 0.5f && !UnlockManager.instance.isAct) 
            {
                UnlockManager.instance.CheckUnlockType(parentTable); 
            }
        }

        if (!parentTable.data.IsUnlock) 
        {
            if (parentTable.data.unlockNeedCount <= 0) { FinishAct(); }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player)) { CollisionEnterPlayer(); }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player)) { CollisionExitPlayer(); }
    }


    public void UpdateCurrentText() { goldText.text = GameManager.instance.PriceText(parentTable.data.unlockNeedCount, 0); }

    void CollisionEnterPlayer()
    {
        GameManager.instance.ClickVib();
        isTouch = true;
        anim.SetBool("isEnter", true);
    }
    void CollisionExitPlayer()
    {
        touchTimer = 0;
        isTouch = false;
        UnlockManager.instance.ResetRegist();
        anim.SetBool("isEnter", false);
    }
  
    void FinishAct()
    {
        SoundManager.instance.PlaySfx("Smoke");
        SoundManager.instance.PlaySfx("Buy");


        touchTimer = 0;
        create.ResetPop();
        parentTable.data.IsUnlock = true;
        unlockedEffect.Play();

        GameManager.instance.AstarScanDelay();
    }

    public void RegistPriceText(int defalutUnlockCount)
    {
        RectTransform textParent = goldText.transform.parent.GetComponent<RectTransform>();

        int count = (defalutUnlockCount.ToString()).Length;

        textParent.sizeDelta = new Vector2(95 + (count * 5), textParent.sizeDelta.y);
    }
}
