using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Crops : MonoBehaviour
{
    public CropsData cropsData;

    public GameObject destroyEffect;
    public enum ParentsType
    {
        TABLE, CHAR
    }
    public ParentsType parentsType;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public SpriteRenderer parentRenderer;

    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource audioSoruce;

    [HideInInspector] public SortingGroup sort;


    private void Awake()
    {
        sort = GetComponent<SortingGroup>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSoruce = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (transform.parent == null) Destroy(gameObject);
    }

    private void Update()
    {
        if (parentsType == ParentsType.CHAR) { spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1; }
    }

    public void InitCrops(CropsData cropsData)
    {
        this.cropsData = cropsData;

        spriteRenderer.sprite = cropsData.frontSprite;

        spriteRenderer.sortingOrder = -10;

        anim.SetTrigger("ChangeParent");
    }

    public void ChangeParent(CropsData cropsData, Transform parent, ParentsType type)
    {
        transform.SetParent(parent);
        parentsType = type;
        this.cropsData = cropsData;

        switch (type)
        {
            case ParentsType.CHAR:
                parentRenderer = transform.parent.GetComponent<SpriteRenderer>();

                //spriteRenderer.sortingOrder = 0;
                /* if (Mathf.Abs(crops.spriteRenderer.transform.parent.GetComponent<ItemBox>().animDir.lookDir.x)
                     > Mathf.Abs(crops.spriteRenderer.transform.parent.GetComponent<ItemBox>().animDir.lookDir.y))
                     crops.spriteRenderer.sprite = sideSprite;
                 else
                     crops.spriteRenderer.sprite = frontSprite;*/
                break;

            case ParentsType.TABLE:
                spriteRenderer.sprite = cropsData.frontSprite;
                spriteRenderer.sortingOrder = -10;
                break;
        }
        anim.SetTrigger("ChangeParent");
    }


}
