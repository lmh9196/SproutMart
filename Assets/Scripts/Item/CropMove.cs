using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropMove
{

    public void Init(List<CropMoveStat> cropStatList, IMoveCrop area)
    {
        if (cropStatList.Count == 0)
        {
            CropMoveStat newCropArea = new CropMoveStat();
            cropStatList.Add(newCropArea);
            newCropArea.Init(area);


        }
        else
        {
            for (int i = 0; i < cropStatList.Count; i++)
            {
                if (cropStatList[i].cropsArea == area) { break; }

                if (cropStatList.Count - 1 == i)
                {
                    CropMoveStat newCropArea = new CropMoveStat();
                    cropStatList.Add(newCropArea);
                    newCropArea.Init(area);
                }
            }
        }
    }
    public void Remove(List<CropMoveStat> cropStatList, IMoveCrop area)
    {
        for (int i = cropStatList.Count - 1; i >= 0; i--)
        {
            if (cropStatList[i].cropsArea == area) { cropStatList.RemoveAt(i); }
        }
    }

    public void CropsMoveLimit(List<CropMoveStat> cropStatList, Transform charItemBox, int maxCount, Action FeedBack = null)
    {
        for (int i = 0; i < cropStatList.Count; i++)
        {
            cropStatList[i].timer += Time.deltaTime;
            if (cropStatList[i].timer >= 0.2f)
            {
                cropStatList[i].cropsArea.Move(charItemBox, maxCount, FeedBack);
                cropStatList[i].timer = 0;
            }
        }
    }
}

public class CropMoveStat
{
    public float timer;
    public IMoveCrop cropsArea;
    public void Init(IMoveCrop area)
    {
        cropsArea = area;
        timer = 0.2f;
    }
}

public interface IMoveCrop
{
    public void Move(Transform charItemBox, int charMaxCount, Action FeedBackAct = null);
}


