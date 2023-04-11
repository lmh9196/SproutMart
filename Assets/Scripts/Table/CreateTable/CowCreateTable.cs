using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowCreateTable : CreateTable
{

    [SerializeField] GameObject readySign;


    public Animator cowAnim;
    public AudioSource cowSFX;

    public override void Awake() { base.Awake(); }

    public override void Start()
    {
        base.Start();
        StartCoroutine(ActiveAnim());

        if (cowSFX != null) { StartCoroutine(StartSFX()); }
    }

    public override void Update()
    {
        base.Update();

        if (createWork.HaveCheck()) { readySign.SetActive(true); }
        else { readySign.SetActive(false); }
    }

    public int minRandAnimTime;
    public int maxRandAnimTime;
    IEnumerator ActiveAnim()
    {
        int rand = Random.Range(minRandAnimTime, maxRandAnimTime);
        yield return new WaitForSeconds(rand);

        if (data.IsUnlock) { cowAnim.SetTrigger("Active"); }

        StartCoroutine(ActiveAnim());
    }

    public int minRandSfxTime;
    public int maxRandSfxTime;
    IEnumerator StartSFX()
    {
        int rand = Random.Range(minRandSfxTime, maxRandSfxTime);
        yield return new WaitForSeconds(rand);

        if (data.IsUnlock) { cowSFX.Play(); }

        StartCoroutine(StartSFX());
    }
}
