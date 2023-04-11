using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueDetectSquare : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;

    public Sprite green;
    public Sprite red;

    private void Awake() { spriteRenderer = GetComponent<SpriteRenderer>(); }

    private void OnTriggerStay2D(Collider2D collision)
    {
        spriteRenderer.sprite = red;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.sprite = green;
    }

}
