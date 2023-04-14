using DG.Tweening;
using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class TableWork
{
    public Transform lookPoint;

    abstract public bool CheckArea();
    abstract public void Work();
}
public class CraftWork : TableWork
{
    bool isWorking;

    Action CraftCorutine;

    TableArea[] setAreas;
    TableArea finishAreas;
    List<Crops> craftingCropsList = new();
    public void Init(TableArea[] _setAreas, TableArea _finishAreas, Action _CraftCorutine)
    {
        setAreas = _setAreas;
        finishAreas = _finishAreas;
        CraftCorutine = _CraftCorutine;
    }
    public override void Work() 
    {
        if (CheckArea() && !isWorking) { CraftCorutine?.Invoke(); }
    }
    override public bool CheckArea()
    {
        if (isWorking) { return false; }

        int count = 0;

        for (int i = 0; i < setAreas.Length; i++)
        {
            if (setAreas[i].itemBox.childCount > 0) { count++; }
        }
        if (finishAreas.itemBox.childCount < finishAreas.maxCount) { count++; }

        if (count == setAreas.Length + 1) { return true; }
        else { return false; }
    }

    public IEnumerator Crafting(CraftData craftData,Animator anim, Transform throwPoint, AudioSource workAudioSource, AudioSource removeAudioSource, ParticleSystem enterEffect, ParticleSystem exitEffect, ParticleSystem workingEffect)
    {
        isWorking = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < setAreas.Length; i++)
        {
            Crops crop = setAreas[i].itemBox.transform.GetChild(setAreas[i].itemBox.transform.childCount - 1).GetComponent<Crops>();

            crop.transform.SetParent(null);
            craftingCropsList.Add(crop);
            crop.transform.DOJump(throwPoint.position, 1f, 1, 1f).SetEase(Ease.Linear);
            crop.sort.enabled = true;
        }

        while (true)
        {
            for (int i = craftingCropsList.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(craftingCropsList[i].transform.position, throwPoint.position) < 0.01f)
                {
                    enterEffect.transform.position = craftingCropsList[i].transform.position;
                    enterEffect.Play();
                    removeAudioSource.Play();
                    craftingCropsList[i].transform.DOKill(false);
                    UnityEngine.Object.Destroy(craftingCropsList[i].gameObject);
                    craftingCropsList.RemoveAt(i);
                }
            }
            if (craftingCropsList.Count == 0) { break; }
            yield return null;
        }

        workAudioSource.Play();

        if (workingEffect != null) { workingEffect.Play(); }

        anim.SetBool("isMaking", true);

        yield return new WaitForSeconds(craftData.ResultMakeSpeed());

        Crops crops = UnityEngine.Object.Instantiate(finishAreas.cropsPre, finishAreas.itemBox).GetComponent<Crops>();
        crops.InitCrops(finishAreas.cropsData);

        yield return null;
        exitEffect.transform.position = crops.gameObject.transform.position;
        exitEffect.Play();
        exitEffect.GetComponent<AudioSource>().Play();

        if (workingEffect != null) { workingEffect.Stop(); }

        workAudioSource.Stop();
        anim.SetBool("isMaking", false);

        isWorking = false;
    }
}

public class CreateWork : TableWork
{
    float respawnTime;

    TableArea getArea;
    float timerCount;
    public void Init(TableArea _getArea, float _respawnTime) 
    {
        getArea = _getArea;
        respawnTime = _respawnTime;
    }
    override public bool CheckArea()
    {
        if (getArea.itemBox.childCount < getArea.maxCount) { return true; }
        else return false;
    }
    public override void Work()
    {
        if (CheckArea())
        {
            timerCount += Time.deltaTime;
            if (timerCount >= respawnTime) { CreateAct(); }
        }
        else { timerCount = 0; }
    }
    public void CreateAct()
    {
        for (int i = 0; i < getArea.maxCount; i++)
        {
            if (getArea.itemBox.transform.childCount <= i)
            {
                Crops crops = UnityEngine.Object.Instantiate(getArea.cropsPre, getArea.itemBox.transform).GetComponent<Crops>();
                crops.InitCrops(getArea.cropsData);
                //getArea.cropsData.CreateItem(cropObj.GetComponent<Crops>());
            }

            if (i == getArea.maxCount - 1) { timerCount = 0; }
        }
    }

    public bool HaveCheck()
    {
        if (getArea.itemBox.childCount > 0) { return true; }
        else return false;
    }
}

public class SalesWork : TableWork
{
    TableArea salesArea;
    public Sprite hopeSprite;

    MoveCrop moveCrop = new();

    float checkTimer = 0.33f;
    float checkTimerCount = 0;

    Table table;

    public void Init(Table _table , TableArea _salesArea , Transform _lookPoint, Sprite _hopeSprite)
    {
        table = _table;
        salesArea = _salesArea;
        lookPoint = _lookPoint;
        hopeSprite = _hopeSprite;
    }
    public override bool CheckArea()
    {

        if (table.customerList.Count > 0)
        {
            if (table.customerList[0].appearance.type == CustomerAppearance.Type.DRUNKEN) { return false; }

            if (table.customerList[0].GetComponent<AILerp>().reachedEndOfPath)
            {
                checkTimerCount += Time.deltaTime;

                if (checkTimerCount >= checkTimer) { return true; }
            }
        }
        return false;
    }
    public override void Work()
    {
        checkTimerCount = 0;

        if (table.customerList[0].currentHandsCount < table.customerList[0].maxHandsCount) { FillCustomerHands(); }
        else { FinishCustomer(); }
    }

    void FillCustomerHands() { moveCrop.SalesCrop(salesArea, table.customerList[0]); }
    void FinishCustomer()
    {
        table.customerList[0].TargetChagne(table.customerList);
    }
}

public class CountWork : TableWork
{
    Table table;

    public Sprite hopeSprite;
    Transform packingBoxPoint;
    GameObject packingBoxPre;

    GameObject packBox;

    AudioSource audioFinishPlayer;
    public AudioSource audioCropsPlayer;

    public bool isCounting;

    Action CountingAct;

    public void Init(Table _table,Action _CountingAct, AudioSource _audioFinishPlayer, AudioSource _audioCropsPlayer, Transform _lookPoint, Transform _packingBoxPoint, GameObject _packingBoxPre, Sprite _hopeSprite)
    {
        table = _table;
        CountingAct = _CountingAct;
        lookPoint = _lookPoint;
        hopeSprite = _hopeSprite;
        packingBoxPoint = _packingBoxPoint;
        packingBoxPre = _packingBoxPre;
        audioFinishPlayer = _audioFinishPlayer;
        audioCropsPlayer = _audioCropsPlayer;
    }

    public override bool CheckArea()
    {
        if (table.customerList.Count > 0)
        {
            if (table.customerList[0].GetComponent<AILerp>().reachedEndOfPath && !isCounting) { return true; }
        }
        else { if(isCounting) isCounting = false; }

        return false;
    }

    public override void Work()
    {
        isCounting = true;
        CountingAct?.Invoke();
    }
    public IEnumerator CountCoroutine(GameObject DestroyEffect)
    {

        List<Crops> cropList = new();
        ReadyPackingBox();
        yield return new WaitForSeconds(0.5f);

        while(table.customerList[0].itemBox.childCount > 0)
        {
            ShootCrops(cropList);
            yield return new WaitForSeconds(0.2f);
        }
        table.customerList[0].itemBox.gameObject.SetActive(false);
        table.customerList[0].itemBoxCover.gameObject.SetActive(false);
        table.customerList[0].packingBox.transform.localPosition = Vector3.zero;


        while (cropList.Count > 0)
        {
            for (int i = cropList.Count - 1; i >= 0; i--)
            {
                if (packingBoxPoint.position.y >= cropList[i].transform.position.y)
                {
                    GameManager.instance.CalGold(GameManager.GoldType.BOXGOLD, cropList[i].cropsData.price);

                    UnityEngine.Object.Instantiate(DestroyEffect, cropList[i].transform.position, Quaternion.identity);

                    cropList[i].transform.DOKill();

                    UnityEngine.Object.Destroy(cropList[i].gameObject);
                    cropList.RemoveAt(i);
                    audioCropsPlayer.Play();
                    break;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        
        
        packBox.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        MoveBox();
        yield return new WaitForSeconds(0.2f);
        audioFinishPlayer.Play();

        yield return new WaitForSeconds(0.3f);

        table.customerList[0].TargetChagne(table.customerList);
        isCounting = false;
    }

    public void ReadyPackingBox()
    {
        if (packingBoxPoint.childCount > 0)
        {
            for (int i = 0; i < packingBoxPoint.childCount; i++)
            {
                UnityEngine.Object.Destroy(packingBoxPoint.GetChild(i).gameObject);
            } 
        }
        packBox = UnityEngine.Object.Instantiate(packingBoxPre, packingBoxPoint.position, Quaternion.identity);
        packBox.transform.SetParent(packingBoxPoint);
    }

    public void ShootCrops(List<Crops> cropList)
    {
        Crops crops = table.customerList[0].itemBox.GetChild(table.customerList[0].itemBox.childCount - 1).GetComponent<Crops>();
        cropList.Add(crops);

        crops.ChangeParent(crops.cropsData, packingBoxPoint.GetChild(0), Crops.ParentsType.TABLE);
        crops.sort.enabled = true;

        crops.transform.DOJump(packingBoxPoint.position, 1f,1, 0.6f).SetEase(Ease.Linear);
    }

    public void MoveBox()
    {
        packBox.SetActive(true);
        packBox.transform.SetParent(table.customerList[0].packingBox.transform);
        packBox.transform.localPosition = Vector3.zero;
    }

    public void CompareCustomer(Customer customer,ChildCounter childTable)
    {
        if(table.customerList.Count > childTable.customerList.Count)
        {
            table.customerList.Remove(customer);
            customer.shoppingList[0] = childTable;
            childTable.customerList.Add(customer);
        }
    }
}

public class ExitWork : TableWork
{
    public Customer customer;
    Sprite[] signSprites;

    public void Init(Sprite[] _signSprites) 
    {
        signSprites = _signSprites;
    }
    public override bool CheckArea() { return false; }
    public override void Work() { return; }
    
    public Sprite SelectSign()
    {
        int rand = UnityEngine.Random.Range(0, signSprites.Length);

        return signSprites[rand];
    }
}