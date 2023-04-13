using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffWork
{
    public Transform target;

    GameObject timerFinisheffect;
    Transform waitingPos;
    Image timerImage;
    List<TableArea> setList = new();
    List<TableArea> getList = new();

    float timer;

    bool isHarvest;
    bool isWaiting;

    TimerImageAct timerImageAct = new();
    public void SetStart(List<TableArea> _setList, List<TableArea> _getList, Transform _waitingPos, Image _timerImage, GameObject _timerFinisheffect)
    {
        setList = _setList;
        getList = _getList;
        waitingPos = _waitingPos;
        timerImage = _timerImage;
        timerFinisheffect = _timerFinisheffect;
        timerImageAct.Init(timerImage.transform.parent.gameObject, timerImage);
        timerImageAct.ResetTimer();
    }

    float arrivalTimer = 0;
    TableArea crtGetArea = null;
    TableArea actArea;
    public void WorkStart(bool isCheckArrival, Transform itemBox, CharData charData)
    {
        if (!isHarvest) { Harvest(itemBox, charData); }
        else if (!isWaiting) { WaitCoolTime(isCheckArrival, charData); }
        else { CarryCrops(itemBox, isCheckArrival); }
    }

    void Harvest(Transform itemBox, CharData charData)
    {
        if (itemBox.childCount == charData.maxHandsCount)
        {
            target = waitingPos;
            crtGetArea = null;
            isHarvest = true; return;
        }
        else
        {
            if (crtGetArea == null) { FindCloseGetArea(itemBox.parent.position); }
            else
            {
                if (crtGetArea.itemBox.childCount == 0) { crtGetArea = null; }
            }
        }
    }
    void WaitCoolTime(bool isCheckArrival, CharData charData)
    {
        if (isCheckArrival)
        {
            arrivalTimer += Time.deltaTime;

            if (arrivalTimer > 0.33f)
            {
                timerImage.transform.parent.gameObject.SetActive(true);

                timer = Time.deltaTime / charData.coolTime;

                timerImageAct.FillTimerImage(timerImage.transform.parent.gameObject, timerImage, timerImage.transform.parent.gameObject.activeSelf,
                    timer, DetectSetArea);
            }
        }
    }
    void CarryCrops(Transform itemBox, bool isArrival)
    {
        if (itemBox.childCount > 0)
        {
            if (actArea.SetNeedCount() == 0 && isArrival) { DetectSetArea(); }
            else { return; }
        }
        else { StateReset(); return; }
    }
    void DetectSetArea()
    {
        if (!isWaiting) { FindLessSetArea(WaitingFinishAct); }
        else { FindLessSetArea(null); }
    }
    void WaitingFinishAct()
    {
        if (timerImage.transform.parent.gameObject.activeSelf) timerFinisheffect.SetActive(true);

        isWaiting = true;
        timer = 0;
        arrivalTimer = 0;
        timerImageAct.ResetTimer();
    }
    void StateReset()
    {
        isHarvest = false;
        isWaiting = false;
    }
    List<T> ListShuffle<T>(List<T> list)
    {
        int random;
        T temp;
        for (int i = 0; i < list.Count; ++i)
        {
            random = Random.Range(0, list.Count);

            temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }

        return list;
    }
    void FindLessSetArea(System.Action WaitingFinishAct)
    {
        actArea = null;
        List<TableArea> ableTableAreaList = new();

        ListShuffle(setList);

        for (int i = 0; i < setList.Count; i++)
        {
            if (setList[i].SetNeedCount() > 0 && setList[i].parentTable.data.IsUnlock && setList[i].parentTable.transform.parent.gameObject.activeSelf) { ableTableAreaList.Add(setList[i]); }

            if (setList.Count - 1 == i)
            {
                if (ableTableAreaList.Count == 0) { StateReset(); return; }
                else
                {
                    for (int j = 0; j < ableTableAreaList.Count; j++)
                    {
                        if (j == 0) { actArea = ableTableAreaList[j]; }
                        else if (GetCountRatio(ableTableAreaList[j]) < GetCountRatio(actArea)) { actArea = ableTableAreaList[j]; }
                   
                        if (ableTableAreaList.Count - 1 == j)
                        {
                            if (actArea == null) { StateReset(); return; }
                            else
                            {
                                target = actArea.transform;
                                WaitingFinishAct?.Invoke();
                            }
                        }
                    }
                }
            }
        }
    }
    void FindCloseGetArea(Vector3 staffPos)
    {
        float tempDis = 0;
        TableArea tempGetArea = null;



        for (int i = 0; i < getList.Count; i++)
        {
            if (getList[i].itemBox.childCount > 0 && getList[i].parentTable.data.IsUnlock && getList[i].parentTable.transform.parent.gameObject.activeSelf)
            {
                if (tempGetArea == null)
                {
                    tempDis = Vector2.Distance(getList[i].transform.position, staffPos);
                    tempGetArea = getList[i];
                }
                else
                {
                    if (tempDis > Vector2.Distance(getList[i].transform.position, staffPos))
                    {
                        tempDis = Vector2.Distance(getList[i].transform.position, staffPos);
                        tempGetArea = getList[i];
                    }
                }
            }

            if (i == getList.Count - 1)
            {
                crtGetArea = tempGetArea;

                if (crtGetArea != null)
                {
                    target = crtGetArea.transform;
                }
            }
        }
    }

    float GetCountRatio(TableArea setArea)
    {
        int maxCount = setArea.maxCount;
        int currentCount = setArea.itemBox.childCount;

        if (setArea.parentTable is CraftSalesTable craftSalesTable) 
        {
            maxCount += craftSalesTable.finishArea.maxCount;
            currentCount += craftSalesTable.finishArea.itemBox.childCount;
        }
        else if (setArea.parentTable is CraftGetTable craftGetTable) 
        {
            maxCount += craftGetTable.finishArea.maxCount;
            currentCount += craftGetTable.finishArea.itemBox.childCount;
        }


        return (float)currentCount / (float)maxCount;
    }
}
