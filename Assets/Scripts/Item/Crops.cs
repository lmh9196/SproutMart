using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



public class Crops : MonoBehaviour
{
    public CropsData cropsData;

    public GameObject destroyEffect;
   

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
        if (parentRenderer != null) { spriteRenderer.sortingOrder = parentRenderer.sortingOrder - 1; }
    }


    void CropsInit(Transform parent, CropsData cropsData)
    {
        transform.SetParent(parent);
        this.cropsData = cropsData;
        anim.SetTrigger("ChangeParent");
    }
    public void CropsTableInit(Transform parent, CropsData cropsData)
    {
        CropsInit(parent, cropsData);
        spriteRenderer.sprite = cropsData.frontSprite;
        spriteRenderer.sortingOrder = -10;
        parentRenderer = null;
    }
    public void CropsCharInit(Transform parent, CropsData cropsData)
    {
        CropsInit(parent, cropsData);
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }
    
}
