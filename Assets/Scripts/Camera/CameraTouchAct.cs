using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchAct
{
    float zoomSpeed = 0.01f;


    public void CheckTouch()
    {
        switch(Input.touchCount)
        {
            case 0: isZooming = false;break;
            case 2: isZooming = true; break;
        }
    }

    bool isZooming;
    public float TouchDragZoom(float cameraSize, float _zoomMaxSize, float _zoomMinSize)
    {
        if(isZooming)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (Mathf.Abs(deltaMagnitudeDiff) > 4f)
            {
                if (deltaMagnitudeDiff > 0)
                {
                    if (cameraSize >= _zoomMaxSize) { return cameraSize; }
                }
                else
                {
                    if (cameraSize <= _zoomMinSize) { return cameraSize; }
                }

                return cameraSize += deltaMagnitudeDiff * (zoomSpeed * MainCamera.instance.ResoultionWeight(false));
            }
            else return cameraSize;
        }
        else return cameraSize;
    }

    Vector2 firstOffset;
    Vector2 secondOffset;
    Vector3 dragDir;

    float dis;
    public void TouchDragMove(Transform cameraPos,Camera camera, float xMaxLimit, float xMinLimit, float yMaxLimit, float yMinLimit)
    {
        if(!isZooming)
        {
            if (Input.GetMouseButtonDown(0)) { firstOffset = camera.ScreenToViewportPoint(Input.mousePosition); }
            else if (Input.GetMouseButton(0))
            {
                secondOffset = firstOffset;
                firstOffset = camera.ScreenToViewportPoint(Input.mousePosition);

                dis = Vector2.Distance(camera.ViewportToWorldPoint(secondOffset), camera.ViewportToWorldPoint(firstOffset));

                if (dis > 0)
                {
                    dragDir = (firstOffset - secondOffset).normalized;
                    if (cameraPos.position.x >= xMaxLimit)
                    {
                        if (dragDir.x < 0) { dragDir.x = 0; }
                    }
                    else if (cameraPos.position.x <= xMinLimit)
                    {
                        if (dragDir.x > 0) { dragDir.x = 0; }
                    }

                    if (cameraPos.position.y >= yMaxLimit)
                    {
                        if (dragDir.y < 0) { dragDir.y = 0; }
                    }
                    else if (cameraPos.position.y <= yMinLimit)
                    {
                        if (dragDir.y > 0) { dragDir.y = 0; }
                    }

                    cameraPos.Translate(-1 * dragDir * (camera.orthographicSize * MainCamera.instance.ResoultionWeight(true)) * dis * 3 * Time.deltaTime);
                }
            }
        }
    }
}
