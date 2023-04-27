using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using I2.Loc;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StatueManager : MonoBehaviour
{
    public static StatueManager instance = null;
    public enum BuildMode { NONE, BUILD, MOVE, DESTROY }
    public BuildMode buildMode;

    public bool isCollTrigger;
    bool isUICheck;

    public Transform storageParent;
    public Transform selectParent;
    public GameObject buildGrid;

    [HideInInspector] public Statue selectStatue;
    public List<Statue> statueList;

    [HideInInspector] public List<string> statueName;
    [HideInInspector] public List<Vector2> statuePos;

    public Dictionary<string, StatueSaveData> statueSaveDic = new();

    bool isPreTrigger;

    Vector2 mousePos;
    Vector3 viewPos;
    Vector3 worldPos;
    Vector3 defaultStatuePos;

    Action preAct;
    Action runningAct;

    Player player;
    public Statue[] statuePrefabs;

    int destroyCount;

    DialogueTerm term = new();
    public int DestroyCount
    {
        get {  return destroyCount; }
        set
        {
            DialogueManager.instance.DisableDialogue(term.termDestroy_Select, " (" + destroyCount + "/" + statueList.Count + ")");
            DialogueManager.instance.EnableDialouge(term.termDestroy_Select, false, false, " (" + value + "/" + statueList.Count + ")");

            destroyCount = value;
        }
    }
    private void Awake() 
    {
        if (instance == null) { instance = this; }

        player = GameObject.FindObjectOfType<Player>();
    }

    private void Start()
    {
        MenuManager.instance.statues.statueManager = this;
        GameManager.instance.checkList.isBuildMode = false;

        StatueLoad();
    }

    void Update()
    {
        if (GameManager.instance.checkList.isBuildMode) { UpdateModeAct(); }

        if (buildMode == BuildMode.DESTROY) 
        {
            UpdateDestroyMod();

            if (DestroyCount != destroyList.Count) { DestroyCount = destroyList.Count; }
        }
    }

    public void StatueLoad()
    {
        if(ES3.KeyExists(SceneManager.GetActiveScene().name + "Statue"))
        {
            statueSaveDic = ES3.Load(SceneManager.GetActiveScene().name + "Statue", statueSaveDic);

            for (int j = 0; j < statuePrefabs.Length; j++)
            {
                foreach (KeyValuePair<string, StatueSaveData> statueData in statueSaveDic)
                {
                    if (statueSaveDic[statueData.Key].ID == statuePrefabs[j].data.ID)
                    {
                        Statue statue = Instantiate(statuePrefabs[j].gameObject, storageParent).GetComponent<Statue>();
                        statue.LoadStatue(statueSaveDic[statueData.Key].pos);
                        statueList.Add(statue);
                    }
                }
            }
        
        }
    }

    public void StatueSave() { ES3.Save(SceneManager.GetActiveScene().name + "Statue", statueSaveDic); }
    public void InitStatueManager( BuildMode _buildMode, Statue _statue = null)
    {
        if (_statue != null) { selectStatue = _statue; }
        buildMode = _buildMode;
        isPreTrigger = false;
        isCollTrigger = false;

        StartBuildMode();
    }

    void StartBuildMode()
    {
        isPreTrigger = true;

        GameManager.instance.checkList.isBuildMode = true;
        GameManager.instance.canvasList.BuildModeCanvas(true);
        buildGrid.SetActive(true);
        //DialogueManager.instance.EnableDialouge(null, false, false);

        switch (buildMode)
        {
            case BuildMode.BUILD:
                SpawnStatue();
                DialogueManager.instance.EnableDialouge(term.termBuild_ChoicePos, true, false);

                preAct = NoneExistPre;
                runningAct += StatueMouseChasing;
                break;
            case BuildMode.MOVE:
                DialogueManager.instance.EnableDialouge(term.termMove_ChoiceObj, true, false);
                preAct = MoveModeFindStatue;
                runningAct = StatueMouseChasing;
                break;

            case BuildMode.DESTROY:

                DestroyModOn(true);
                DialogueManager.instance.EnableDialouge(term.termDestroy_Select, true, false, " (" + destroyCount + "/" + statueList.Count + ")");

                preAct = NoneExistPre;
                runningAct += CollectDestroyStatue;
                break;
        }
    }

    public void ResetStatueManager()
    {
        GameManager.instance.checkList.isBuildMode = false;
        GameManager.instance.canvasList.BuildModeCanvas(false);
        MainCamera.instance.ChangeCamTarget(false);
        DialogueManager.instance.DisableDialogue(null);


        buildGrid.SetActive(false);

        buildMode = BuildMode.NONE;
        preAct = null;
        runningAct = null;
        isPreTrigger = false;
        isCollTrigger = false;

        for (int i = 0; i < statueList.Count; i++) 
        {
            statueList[i].DestroySelect(false);
            statueList[i].detectParent.gameObject.SetActive(false);
        }

        destroyList.Clear();

       


        DestroyModOn(false);

        if (selectStatue != null) { selectStatue = null; }
    }

    public void EndPreAct()
    {
        switch(buildMode)
        {
            case BuildMode.MOVE:
                DialogueManager.instance.DisableDialogue(term.termMove_ChoiceObj);
                DialogueManager.instance.EnableDialouge(term.termBuild_ChoicePos, true, false); 
                break;
        }
        isPreTrigger = false;
    }

    //Ready
    void DestroyModOn(bool on) { for (int i = 0; i < statueList.Count; i++) { statueList[i].destroyImage.SetActive(on); } }
    void UpdateDestroyMod()
    {
        for (int i = 0; i < statueList.Count; i++)
        {
            if (destroyList.Contains(statueList[i])) { statueList[i].DestroySelect(true); }
            else { statueList[i].DestroySelect(false); }
        }
    }
    void UpdateModeAct()
    {
        if (isPreTrigger) { preAct?.Invoke(); }
        else runningAct?.Invoke();
    }
    void StatueMouseChasing()
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(GameManager.instance.PointerIDCheck()))
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                selectParent.position = new Vector3Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);

                viewPos = Camera.main.WorldToViewportPoint(selectParent.position);

                if (viewPos.x < 0.1f || viewPos.x > 0.9f || viewPos.y < 0.1f || viewPos.y > 0.9f) { MainCamera.instance.ChangeCamTarget(true, selectParent); }
                else MainCamera.instance.ChangeCamTarget(true, null);
            }
        }
    }

    public List<Statue> destroyList;
    void CollectDestroyStatue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(GameManager.instance.PointerIDCheck())) { isUICheck = false; }
            else { isUICheck = true; }

            mousePos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, transform.forward, 15f, 1 << LayerMask.NameToLayer("StatueDetectArea"));

            if (hit.collider != null)
            {
                GameManager.instance.ClickVib();
                SoundManager.instance.PlaySfx("Pop");
                Statue statue = hit.transform.GetComponent<Statue>();

                if (destroyList.Contains(statue)) { destroyList.Remove(statue); }
                else { destroyList.Add(statue); }
            }
        }
        if (Input.GetMouseButton(0) && !isUICheck) { MainCamera.instance.MoveDragCamAct(Camera.main); }
    }
    void MoveModeFindStatue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject(GameManager.instance.PointerIDCheck())) { isUICheck = false; }
            else { isUICheck = true; }

            mousePos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, transform.forward, 15f, 1 << LayerMask.NameToLayer("StatueDetectArea"));

            if (hit.collider != null)
            {
                GameManager.instance.ClickVib();
                SoundManager.instance.PlaySfx("Pop");

                selectStatue = hit.transform.GetComponent<Statue>();

                selectStatue.detectParent.gameObject.SetActive(true);

                selectParent.position = new Vector3(Mathf.RoundToInt(hit.transform.position.x), Mathf.RoundToInt(hit.transform.position.y), 0);

                defaultStatuePos = selectStatue.transform.position;

                selectStatue.transform.SetParent(selectParent);

                EndPreAct();
                return;
            }
        }

        if (Input.GetMouseButton(0) && !isUICheck)
        {
            if (selectStatue != null) { selectStatue.transform.SetParent(storageParent); }
            MainCamera.instance.MoveDragCamAct(Camera.main);
        }
    }

    void SpawnStatue()
    {
        selectParent.position
       = new Vector3Int(Mathf.RoundToInt(Player.instance.transform.position.x), Mathf.RoundToInt(Player.instance.transform.position.y), 0);

        selectStatue = Instantiate(selectStatue.gameObject).GetComponent<Statue>();
        selectStatue.transform.SetParent(selectParent);

        for (int i = 0; i < statuePrefabs.Length; i++)
        {
            if (selectStatue.name.Contains(statuePrefabs[i].name))
            {
                selectStatue.InitStatue(selectParent.position + statuePrefabs[i].defalutPos); break;
            }

            if (i == statuePrefabs.Length - 1) { Debug.Log("Error : Statue Prefab is Null"); }
        }

        selectStatue.detectParent.gameObject.SetActive(true);
    }
    void NoneExistPre() { isPreTrigger = false; }


    public bool EnableCheck(BuildMode buildMode, Statue selectStatue)
    {
        switch (buildMode)
        {
            case BuildMode.BUILD:
                if (GameManager.instance.SelectGold(selectStatue.goldType) >= selectStatue.data.price && !isCollTrigger) { return true; }
                else
                {
                    if (GameManager.instance.SelectGold(selectStatue.goldType) < selectStatue.data.price) 
                    { 
                        ColorManager.instance.GoldFail(selectStatue.goldType);
                        DialogueManager.instance.GuideFlashCoroutine(term.term_NotEnoughGold);

                        return false; 
                    }
                    else if (isCollTrigger) 
                    {
                        ColorManager.instance.BuildFail(selectStatue.detectSquareList);
                        DialogueManager.instance.GuideFlashCoroutine(term.termBuild_Crash);
                        return false; 
                    }
                }
                Debug.Log("Error : BuildMode Check"); return false;
            case BuildMode.MOVE:
                if (selectStatue != null && !isCollTrigger) { return true; }
                else
                {
                    if (isCollTrigger) 
                    {
                        ColorManager.instance.BuildFail(selectStatue.detectSquareList);
                        DialogueManager.instance.GuideFlashCoroutine(term.termBuild_Crash); return false;
                    }
                    else if (selectStatue == null) { DialogueManager.instance.GuideFlashCoroutine(term.termDestroy_NotEnough); return false; }
                }
                return false;

            case BuildMode.DESTROY:
                if (destroyList.Count > 0) { return true; }
                else
                {
                    DialogueManager.instance.GuideFlashCoroutine(term.termDestroy_NotEnough);
                    return false;
                }

            default: return false;
        }
    }

    //ActiveFinish
    public void FinishAct()
    {
        SoundManager.instance.PlaySfx("Smoke");

        switch (buildMode)
        {
            case BuildMode.BUILD: FinishBuild(); break;
            case BuildMode.MOVE: FinishMove(); break;
            case BuildMode.DESTROY:FInishDestroy(); break;
        }

        StatueSave();

        GameManager.instance.AstarScanDelay();
    }
    void FinishBuild()
    {
        GameObject effect = Instantiate(selectStatue.smokeEffect, selectStatue.transform.position, Quaternion.identity);
        effect.SetActive(true);

        SoundManager.instance.PlaySfx("Buy");

        statueList.Add(selectStatue);

        selectStatue.SaveData(SceneManager.GetActiveScene().name, statueSaveDic, selectStatue.transform.position);

        GameManager.instance.CalGold(selectStatue.goldType, -selectStatue.data.price);
        selectStatue.transform.SetParent(storageParent);


        ResetStatueManager();
    }
    void FinishMove()
    {
        DialogueManager.instance.DisableDialogue(term.termBuild_ChoicePos);
        DialogueManager.instance.EnableDialouge(term.termMove_ChoiceObj, true, false);

        GameObject effect = Instantiate(selectStatue.smokeEffect, selectStatue.transform.position, Quaternion.identity);
        effect.SetActive(true);

        SoundManager.instance.PlaySfx("Pop");

        selectStatue.SaveData(SceneManager.GetActiveScene().name, statueSaveDic, selectStatue.transform.position);

        selectStatue.transform.SetParent(storageParent);
        selectStatue.detectParent.gameObject.SetActive(false);

        selectStatue = null;
        isPreTrigger = true;
    }
    void FInishDestroy()
    {
        SoundManager.instance.PlaySfx("Pop");
        DialogueManager.instance.DisableDialogue(term.termDestroy_Select, " (" + destroyCount + "/" + statueList.Count + ")");

        for (int i = destroyList.Count -1; i >= 0; i--)
        {
            GameObject effect = Instantiate(destroyList[i].smokeEffect, destroyList[i].transform.position, Quaternion.identity);
            effect.SetActive(true);

            statueList.Remove(destroyList[i]);
            statueSaveDic.Remove(SceneManager.GetActiveScene().name + destroyList[i].data.ID + destroyList[i].data.pos);

            Destroy(destroyList[i].gameObject);
        }


        destroyList.Clear();
        isPreTrigger = true;
    }


    //Cancle
    public void CancleAct()
    {
        if(selectStatue != null)
        {
            switch (buildMode)
            {
                case BuildMode.BUILD: CancleBuild(); break;
                case BuildMode.MOVE: CancleFailMove(); break;
                case BuildMode.DESTROY: CancleDestroy(); break;
            }
        }
        MainCamera.instance.ChangeCamTarget();
        ResetStatueManager();
    }
    void CancleBuild() { Destroy(selectStatue.gameObject); }
    void CancleFailMove()
    {
        selectStatue.transform.position = defaultStatuePos;
        selectStatue.transform.SetParent(storageParent);
    }
    void CancleDestroy() { selectStatue.transform.SetParent(storageParent); }

}

