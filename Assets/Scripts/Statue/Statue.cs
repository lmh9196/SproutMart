using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StatueSaveData
{
    public string ID;
    public Vector3 pos;

    public int price;

    public StatueSaveData(string iD, Vector3 pos)
    {
        ID = iD;
        this.pos = pos;
    }
}
public class Statue : MonoBehaviour
{
    public StatueSaveData data;

    public GameManager.GoldType goldType;

    [HideInInspector] public StatueManager statueManager;

    //public GameObject prefabs;
    public GameObject destroyImage;
    public GameObject smokeEffect;
    public StatueDetectSquare detectSquare;
    public List<StatueDetectSquare> detectSquareList = new List<StatueDetectSquare>();

    public Transform detectParent;

    public SortingGroup order;

    public Vector3 defalutPos;

    [Header("Child")]
    public Collider2D childColl;
    BoxCollider2D parentColl;
    public SpriteRenderer childRenderer;

    public bool isSelect;

    private void Awake()
    {
        statueManager = GameObject.FindObjectOfType<StatueManager>();
        order = GetComponent<SortingGroup>();
        parentColl = GetComponent<BoxCollider2D>();

        CreateDetectArea();
    }

    private void Update()
    {
        if(detectParent.gameObject.activeSelf)
        {
            int redCount = 0;

            for (int i = 0; i < detectSquareList.Count; i++)
            {
                if (detectSquareList[i].spriteRenderer.sprite == detectSquareList[i].red) { redCount++; }

                if (i == detectSquareList.Count - 1)
                {
                    if (redCount > 0) { statueManager.isCollTrigger = true; }
                    else { statueManager.isCollTrigger = false; }
                }
            }
        }

        if (transform.parent != null)
        {
            if (transform.parent == statueManager.selectParent)
            {
                childColl.enabled = false;
                order.sortingOrder = 30;
            }
            else
            {
                childColl.enabled = true;
                order.sortingOrder = 0;
            }
        }
    }


    public void DestroySelect(bool on)
    {
        if (on) { childRenderer.color = new Color32(255, 100, 100, 255); }
        else { childRenderer.color = new Color32(255, 255, 255, 255); }
    }

    public void InitStatue(Vector3 targetPos)
    {
        gameObject.name = data.ID;
        transform.position = targetPos;
    }

    public void LoadStatue(Vector3 dataPos)
    {
        InitStatue(dataPos);
        detectParent.gameObject.SetActive(false);
    }

    public void SaveData(string sceneName, Dictionary<string, StatueSaveData> statueDataDic, Vector3 currentPos)
    {
        if (statueDataDic.ContainsKey(sceneName + data.ID + data.pos))
        {
            statueDataDic.Remove(sceneName + data.ID + data.pos);
        }

        data.pos = currentPos;

        statueDataDic.Add(sceneName + data.ID + data.pos, new StatueSaveData(data.ID, data.pos));
    }


    public void CreateDetectArea()
    {
        Vector3Int leftTop = new Vector3Int(Mathf.RoundToInt(parentColl.bounds.center.x - parentColl.bounds.extents.x),
            Mathf.RoundToInt(parentColl.bounds.center.y + parentColl.bounds.extents.y), 0);

        Vector3Int RightBottom = new Vector3Int(Mathf.RoundToInt(parentColl.bounds.center.x + parentColl.bounds.extents.x),
           Mathf.RoundToInt(parentColl.bounds.center.y - parentColl.bounds.extents.y), 0);


        for (int i = leftTop.y; i >= RightBottom.y; i--)
        {
            for (int j = leftTop.x; j <= RightBottom.x; j++)
            {
                StatueDetectSquare detectS = Instantiate(detectSquare.gameObject, detectParent).GetComponent<StatueDetectSquare>();

                detectS.transform.position = new Vector3Int(j, i, 0);

                detectSquareList.Add(detectS);
            }
        }
    }
}
