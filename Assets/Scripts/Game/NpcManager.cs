using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance = null;
    public StageData stageData;
    public NpcData npcData;

    public float setTimer;

    [Space(10f)]
    [Header("Upgrade")]
    [Space(10f)]

    [SerializeField] StageManager stageManager;
    Customer[] customerList;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        for (int i = 0; i < transform.childCount; i++) { transform.GetChild(i).gameObject.SetActive(false); }
        customerList = GetComponentsInChildren<Customer>();
    }

    private void Start() 
    { spawnTimer = setTimer; }
    private void Update()
    {
        UpdateNpcLevel();

        if (GameManager.instance.checkList.IsTutorialEnd && StageManager.instance.salesUnLockTableList.Count > 0 && spawnTimer <= 0) Spawn();
        else if(spawnTimer > 0) spawnTimer -= Time.deltaTime;
    }

    void UpdateNpcLevel()
    {
        stageData.NpcLevel = StageManager.instance.salesUnLockTableList.Count * 2;
    }

    float spawnTimer;
    public void Spawn(int count = 0)
    {
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (!transform.GetChild(i).gameObject.activeSelf)
                {
                    Customer customer = transform.GetChild(i).GetComponent<Customer>();

                    customer.gameObject.SetActive(true);
                    customer.transform.position = stageManager.spawner.SelectSpawner();
                    customer.aiLerp.SearchPath();
                }
            }
        }
        else
        {
            for (int i = 1; i <= stageData.NpcLevel; i++)
            {
                if(transform.childCount > i - 1)
                {
                    if (!transform.GetChild(i - 1).gameObject.activeSelf)
                    {
                        Customer customer = transform.GetChild(i - 1).GetComponent<Customer>();

                        customer.gameObject.SetActive(true);
                        customer.transform.position = stageManager.spawner.SelectSpawner();
                        customer.aiLerp.SearchPath();

                        spawnTimer = setTimer;
                        break;
                    }
                }
            }
        }
    }
}
