using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleEvent : MonoBehaviour
{
    public StageData stageData;
    [SerializeField] GameObject moleObj;

    [SerializeField] float moleSpawnTime;

    public float timer;

    bool isCal;

    private void Start() { timer = moleSpawnTime; }
    private void Update()
    {
        if (!GameManager.instance.checkList.IsTutorialEnd) { return; }
      
        if(!isCal && !moleObj.activeSelf) timer -= Time.deltaTime;

        if (timer <= 0f) Spawn();
    }
    
    Vector2[,] SetNode()
    {
        Vector2[,] spawnPosArray = new Vector2[11, 11];

        Vector2 playerPos = new Vector2(Mathf.RoundToInt(Player.instance.transform.position.x - 5), Mathf.RoundToInt(Player.instance.transform.position.y - 5));

        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++) { spawnPosArray[i, j] = playerPos + new Vector2(i, j); }
        }
        return spawnPosArray;
    }

    void Spawn()
    {
        timer = moleSpawnTime;
        StartCoroutine(SelectPos(SetNode()));
    }

    IEnumerator SelectPos(Vector2[,] spawnPosArray)
    {
        yield return null;

        isCal = true;

        int randX = Random.Range(0, 11);
        int randY = Random.Range(0, 11);

        RaycastHit2D[] hitPos = Physics2D.RaycastAll(spawnPosArray[randX, randY], transform.forward, 15f);

        if(hitPos.Length == 1)
        {
            if (hitPos[0].collider.gameObject.layer == 18)
            {
                moleObj.SetActive(true);
                moleObj.transform.position = spawnPosArray[randX, randY];
                isCal = false;
                yield break;
            }
        }
        StartCoroutine(SelectPos(SetNode()));
    }

    public int SetRewardPrice() { return stageData.molePrice[stageData.Level]; }
}


