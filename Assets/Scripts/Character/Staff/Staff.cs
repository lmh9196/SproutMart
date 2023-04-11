using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staff : MonoBehaviour
{
    public CharData charData;
    public HatData hatData;
    public SpriteRenderer hatSpriteRenderer;
    [HideInInspector]public Transform waitingPos;

    public CharacterAnimDir animDir = new();

     public List<TableArea> setList = new List<TableArea>();
     public List<TableArea> getList = new List<TableArea>();

    public virtual void Awake()
    {
        animDir.anim = GetComponent<Animator>();
    }

    public virtual void SetAnimation()
    {
        animDir.SetAnim();
        animDir.HatState(hatData, hatSpriteRenderer);
    }
    public virtual void SetCharState() {}

    public void SetStartPos() { transform.position = waitingPos.position; }

}
