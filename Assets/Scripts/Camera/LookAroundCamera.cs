using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundCamera : MonoBehaviour
{
    public Camera thisCamera;

    CameraTouchAct cameraTouchAct = new();
    float zoomWeight;
    float zoomMaxSize;
    float zoomMinSize;
    float camSize;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }
    private void Update()
    {
        cameraTouchAct.CheckTouch();

        switch (Input.touchCount)
        {
            case 1: MainCamera.instance.MoveDragCamAct(thisCamera); break;

            case 2: camSize = cameraTouchAct.TouchDragZoom(camSize, zoomMaxSize, zoomMinSize); break;
        }

        camSize = Mathf.Clamp(camSize, zoomMinSize, zoomMaxSize);

        thisCamera.orthographicSize = camSize * MainCamera.instance.ResoultionWeight(false);
    }
    public void StartPreivew()
    {
        zoomWeight = MainCamera.instance.zoomMaxSize * 0.2f;
        zoomMaxSize = MainCamera.instance.zoomMaxSize + zoomWeight;
        zoomMinSize = MainCamera.instance.zoomMinSize + zoomWeight;
        camSize = zoomMaxSize;

        GameManager.instance.canvasList.SetLookAround(true);
        Player player = GameObject.FindObjectOfType<Player>();
        Vector3 startPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        GameManager.instance.checkList.isLookAround = true;

        transform.position = startPos;
    }

    public void EndPreview()
    {
        MainCamera.instance.ChangeCamTarget(false);

        GameManager.instance.canvasList.SetLookAround(false);

        GameManager.instance.checkList.isLookAround = false;

        camSize = zoomMaxSize;
    }
}
