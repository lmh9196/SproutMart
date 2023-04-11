using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    float timer;

    int animID = Animator.StringToHash("IRand");

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 10f)
        {
            StartCoroutine(AnimPlay());
        }
    }

    IEnumerator AnimPlay()
    {
        int rand = Random.Range(1, 11);
        anim.SetInteger(animID, rand);

        timer = 0;

        yield return null;
    }


    //Profile Animator Event
    public void EndAnim(int Id)
    {
        anim.SetInteger(animID, 0);
    }

}
