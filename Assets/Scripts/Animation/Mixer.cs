using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : MonoBehaviour
{
    public float weight;

    Vector2 defaultPos;

    bool isPlaying;

    public Animator anim;
    private void Start()
    {
        defaultPos = transform.localPosition;
    }

    private void Update()
    {
        if (anim.GetBool("isMaking"))
        {
            if (!isPlaying) { StartCoroutine(MixerAct()); }
        }
        else { isPlaying = false; }
    }

    IEnumerator MixerAct()
    {
        isPlaying = true;

        while (isPlaying)
        {
            transform.localPosition = defaultPos + new Vector2(weight, 0);

            weight *= -1;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
