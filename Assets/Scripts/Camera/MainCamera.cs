using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class CameraData
{
    public float camSize;
}
public class MainCamera : MonoBehaviour
{
    public static MainCamera instance = null;
    public CameraData data;
    [Space(10f)]
    [Header("Pre")]
    public Player player;
    public Tutorial tutorial;

 
    [HideInInspector] public Transform targetForm;
    public float xMinLimit;
    public float xMaxLimit;
    public float yMinLimit;
    public float yMaxLimit;
    public float match;

    [Space(10f)]
    [Header("CameraEvent")]
    public float eventTime;
    [Space(10f)]
    [Header("DragZoomInOut")]
    [HideInInspector] public float zoomMinSize;
    [HideInInspector] public float zoomMaxSize;
    float resoultionWeight;
    CameraTouchAct cameraTouchAct = new();

    public LayerMask uiLayer;
    private void Awake()
    {
        if (null == instance) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetResolution();
        targetForm = player.transform;
    }

    void FixedUpdate() { CameraChasing(); }
    private void Update()
    {
        if (GameManager.instance.checkList.isCamEvent || GameManager.instance.checkList.isBuildMode) { camSpeed = 0.05f; } 
        else camSpeed = 0.2f;

        cameraTouchAct.CheckTouch();
        DoubleTouchCheck();
        CameraSize();
    }
   
    void DoubleTouchCheck()
    {
        if (Input.touchCount == 2 && !GameManager.instance.checkList.isLookAround)
        {
            if (EventSystem.current.currentSelectedGameObject == null && GameManager.instance.checkList.CheckDoubleTabZoom())
            {
                GameManager.instance.canvasList.ActiveJoystickCanvas(false);
                data.camSize = cameraTouchAct.TouchDragZoom(data.camSize, zoomMaxSize, zoomMinSize);
            }
        }
        else { GameManager.instance.canvasList.ActiveJoystickCanvas(true); }
    }
    float FindMatch()
    {
        float matchNum = 0;
        float currentRes;

        currentRes = (float)Screen.width / (float)Screen.height;

        if (currentRes > (16f / 9f))
        {
            currentRes = Mathf.RoundToInt(currentRes * 10) - Mathf.RoundToInt((16f / 9f) * 10);

            for (int i = 0; i < currentRes; i++) { matchNum += 0.02f; }
        }
        else
        {
            currentRes = Mathf.RoundToInt((16f / 9f) * 10) - Mathf.RoundToInt(currentRes * 10);

            for (int i = 0; i < currentRes; i++) { matchNum -= 0.02f; }
        }

        return matchNum + 0.64f;
    }
    public void CamEventBtn(GameObject target)
    {
        CamEvent(target, 1.5f);

        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();
    }

    public float camSpeed;
    public void CamEvent(GameObject target, float duration, bool isSimple = false)
    {
        camSpeed = 0.05f;

        if (GameManager.instance.checkList.isBuildMode || GameManager.instance.checkList.isCamEvent) { return; }
        GameManager.instance.checkList.isCamEvent = true;


        if (!isSimple) { GameManager.instance.canvasList.ActiveCameraEventCanvas(true); }


        player.GetComponent<CapsuleCollider2D>().enabled = false;

        player.playerMove.isStop = true;
        player.playerMove.joyDir = Vector3.zero;
        targetForm = target.transform;

        StartCoroutine(Delay(duration, isSimple));
    }
    IEnumerator Delay(float duration, bool isSimple)
    {
        int limitTime = 0;
        while(true)
        {
            if ((Vector2.Distance(targetForm.position, transform.position) < 0.1f) || limitTime > 5) break;
            else limitTime++;
          
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(duration);
        CamReturn();
    }
    public void CamReturn()
    {
        targetForm = player.transform;
        player.GetComponent<CapsuleCollider2D>().enabled = true;
        player.playerMove.isStop = false;

        GameManager.instance.canvasList.ActiveCameraEventCanvas(false);

        StartCoroutine(WaitReturn());
    }
    IEnumerator WaitReturn()
    {
        while(true)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
            {
                GameManager.instance.checkList.isCamEvent = false; break;
            }
            yield return null;
        }
    }
    void SetResolution()
    {
        int setWidth = 1080;
        //int setHeight = 1920;

        float deviceWidth = Screen.width;
        float deviceHeight = Screen.height;

        resoultionWeight = (float)Math.Round(deviceWidth > deviceHeight ? deviceHeight / deviceWidth : deviceWidth / deviceHeight, 4);

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);
    }
    public float ResoultionWeight(bool isPlus)
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) { return 1; }
        else 
        {
            if (isPlus) { return resoultionWeight + 1; }
            else { return resoultionWeight; }
        }
    }
    public void ChangeCamTarget(bool otherTarget = false, Transform target = null)
    {
        if (otherTarget)
        {
            targetForm = target;
        }
        else targetForm = player.transform;
    }

   
    public bool TargetCamInCheck(Transform target, float weight)
    {
        Vector3 targetPos = Camera.main.WorldToViewportPoint(target.position);

        if (targetPos.x >= 0 + weight && targetPos.x <= 1 - weight &&
targetPos.y >= 0 + weight && targetPos.y <= 1 - weight) return true;
        else return false;
    }
    void CameraChasing()
    {
        if (targetForm != null)
        {
            Vector3 targetPos = targetForm.position;
            targetPos.x = Mathf.Clamp(targetForm.position.x, xMinLimit, xMaxLimit);
            targetPos.y = Mathf.Clamp(targetForm.position.y, yMinLimit, yMaxLimit);

            transform.position = Vector3.Lerp(transform.position, targetPos + new Vector3(0, 0, -10), camSpeed);
        }
        else
            transform.position = Vector3.Lerp(transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), 0.05f);
    }

    ScreenOrientation defalutScreen;
    public void Preview(GameObject previewCam)
    {
        previewCam.SetActive(true);
        defalutScreen = Screen.orientation;

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    public void ReturnPreview(GameObject previewCam)
    {
        Screen.orientation = defalutScreen;
        previewCam.SetActive(false);

        StartCoroutine(ScreenDelay());
    }
    IEnumerator ScreenDelay()
    {
        yield return new WaitForSeconds(1f);
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void CameraSize()
    {
        zoomMinSize = 5;
        zoomMaxSize = 15;
        data.camSize = Mathf.Clamp(data.camSize, zoomMinSize, zoomMaxSize);

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Camera.main.orthographicSize = data.camSize;
            for (int i = 0; i < GameManager.instance.canvasList.matchCanvasScalerList.Count; i++) { GameManager.instance.canvasList.matchCanvasScalerList[i].matchWidthOrHeight = 0; }
        }
        else 
        {
            Camera.main.orthographicSize = data.camSize * resoultionWeight;

            if (match == 0) { match = FindMatch(); }
            for (int i = 0; i < GameManager.instance.canvasList.matchCanvasScalerList.Count; i++) { GameManager.instance.canvasList.matchCanvasScalerList[i].matchWidthOrHeight = match; }
        }
    }

    public void MoveDragCamAct(Camera cam) 
    {
        targetForm = null;
        cameraTouchAct.TouchDragMove(cam.transform, cam, xMaxLimit, xMinLimit, yMaxLimit, yMinLimit); 
    }
}
