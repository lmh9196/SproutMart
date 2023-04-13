using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public NpcData npcData;

    public CustomerAppearance appearance = new();

    public Image hopeImage;

    [Space(20f)]
    [Header("ItemBox")]
    public BoxData boxData;
    public Transform itemBox;
    SpriteRenderer boxSpriteRenderer;
    public Transform itemBoxCover;
    SpriteRenderer coverSpriteRenderer;
    public ParticleSystem buffeffect;

    [Space(20f)]
    public GameObject packingBox;
   
   


    public AILerp aiLerp;
    public AIDestinationSetter aiDest;

    public bool isFind;
    public bool isActive;

    public int shoppingKindLimitCount;

    public int handsLimitCount;
    [HideInInspector] public Table counterTable;
    [HideInInspector] public Table exitTable;
    [HideInInspector] public List<Table> shoppingList = new();

    [HideInInspector] public int maxHandsCount;
    [HideInInspector] public int currentHandsCount;


    public CharacterAnimDir animDir = new();
    public CustomerShopping customerShopping = new();
    CapsuleCollider2D capsuleCollider;
    protected virtual void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxSpriteRenderer = itemBox.GetComponent<SpriteRenderer>();
        coverSpriteRenderer = itemBoxCover.GetComponent<SpriteRenderer>();
        animDir.anim = GetComponent<Animator>();
        aiLerp = GetComponent<AILerp>();
        aiDest = GetComponent<AIDestinationSetter>();
        counterTable = StageManager.instance.counterTable;
        exitTable = StageManager.instance.exitTable;
    }
    public virtual void Start()
    {
        appearance.Init(hopeImage);
    }
    protected virtual void Update()
    {
        aiDest.target = customerShopping.UpdateTarget(shoppingList, this);

        SetAnimDir();

        npcData.SetMoveSpeed();
        aiLerp.speed = npcData.moveSpeed;

        boxData.UpdateBoxSprite(animDir.lookDir, boxSpriteRenderer, coverSpriteRenderer);
        boxData.UpdateCropsPos(itemBox);
        packingBox.transform.position = itemBox.transform.position;

        GameManager.instance.checkList.BuffEvent(GameManager.instance.checkList.isCharSPeedBuff, buffeffect);
    }

    public void OnDisable() { DisableCustomer(); }

    public void ResetCollider() { StartCoroutine(EnableColl()); }
    IEnumerator EnableColl()
    {
        capsuleCollider.enabled = false;
        yield return null;
        capsuleCollider.enabled = true;
    }
    protected void ResetHandsCount() 
    {
        maxHandsCount = customerShopping.RegistShoppingCount(handsLimitCount).Item2;
        currentHandsCount = customerShopping.RegistShoppingCount(handsLimitCount).Item1;
    }
    public void DisableCustomer() 
    { 
        isActive = false;
        SetBox();
        shoppingList.Clear();
    }
    public void TargetChagne(List<Customer> salesList = null)
    {
        if (salesList != null) salesList.Remove(this);

        shoppingList.RemoveAt(0);

        ResetHandsCount();

        if (shoppingList[0] == exitTable) { appearance.ChangeThoughtBalloon(shoppingList[0].exitWork.SelectSign()); }
        else if (shoppingList[0] == counterTable) { appearance.ChangeThoughtBalloon(shoppingList[0].countWork.hopeSprite); }
        else { appearance.ChangeThoughtBalloon(shoppingList[0].salesWork.hopeSprite); }

        aiLerp.SearchPath();

        ResetCollider();
    }
    protected void SetBox()
    {
        if (packingBox != null) { for (int i = 0; i < packingBox.transform.childCount; i++) { Destroy(packingBox.transform.GetChild(0).gameObject); } }

        if (itemBox != null)
        {
            for (int i = 0; i < itemBox.childCount; i++) { Destroy(itemBox.GetChild(0).gameObject); }

            itemBox.gameObject.SetActive(true);
            itemBoxCover.gameObject.SetActive(true);
        }
    }
    protected void SetAnimDir()
    {
        animDir.SetDir(aiLerp.Dir, aiLerp.reachedEndOfPath);

        if(shoppingList.Count > 0)
        {
            if (shoppingList[0] == exitTable) { animDir.SetDir(Vector3.left, aiLerp.reachedEndOfPath); }
            else
            {
                for (int i = 0; i < shoppingList[0].customerList.Count; i++)
                {
                    if (shoppingList[0].customerList[0].Equals(this))
                    {
                        if (shoppingList[0] is CountTable || shoppingList[0] is ChildCounter)
                        {
                            animDir.SetDir(aiLerp.Dir, aiLerp.reachedEndOfPath, shoppingList[0].countWork.lookPoint); break;
                        }
                        else { animDir.SetDir(aiLerp.Dir, aiLerp.reachedEndOfPath, shoppingList[0].salesWork.lookPoint); break; }
                    }
                    else if (shoppingList[0].customerList[i].Equals(this))
                    {
                        animDir.SetDir(aiLerp.Dir, aiLerp.reachedEndOfPath, shoppingList[0].saleWayList[i-1]);
                        break;
                    }
                }
            }
        }
        else { Debug.Log("Error!"); }
       
        animDir.SetAnim();
    }
}
