using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    bool isTouch;
    public bool IsTouch
    {
        get { return isTouch; }
        set
        {
            isTouch = value;
        }
    }

    float touchTimer = 0;
    [Space(10f)]
    [SerializeField] Text goldText;
    [SerializeField] Animator anim;

   
    private void Start()
    {
        InitPriceText(parentTable.data.unlockDefaultCount);
    }

    private void Update()
    {
        goldText.text = GameManager.instance.PriceText(parentTable.data.unlockNeedCount, 0);

        if (IsTouch)
        {
            touchTimer += Time.deltaTime;
            if (touchTimer >= 0.5f && !UnlockManager.instance.IsAct) 
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

    void CollisionEnterPlayer()
    {
        GameManager.instance.ClickVib();
        IsTouch = true;
        anim.SetBool("isEnter", true);
    }
    void CollisionExitPlayer()
    {
        touchTimer = 0;
        IsTouch = false;
        UnlockManager.instance.IsAct = false;
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

    public void InitPriceText(int defalutUnlockCount)
    {
        RectTransform textParent = goldText.transform.parent.GetComponent<RectTransform>();

        int count = (defalutUnlockCount.ToString()).Length;

        textParent.sizeDelta = new Vector2(95 + (count * 5), textParent.sizeDelta.y);
    }
}
