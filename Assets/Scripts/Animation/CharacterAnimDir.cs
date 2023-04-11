using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimDir
{
    public bool isMove;
    public Vector3 lookDir;

    public Animator anim;

    public bool isMenualCtr;


    public Vector3 CalDir(Vector3 _inputDir)
    {
        if (Mathf.Abs(_inputDir.x) > Mathf.Abs(_inputDir.y))
        {
            if (_inputDir.x > 0)
                return  Vector3.right;
            else if (_inputDir.x < 0)
                return Vector3.left;
        }
        else if (Mathf.Abs(_inputDir.x) < Mathf.Abs(_inputDir.y))
        {
            if (_inputDir.y > 0)
                return Vector3.up;
            else if (_inputDir.y < 0)
                return Vector3.down;
        }

        return lookDir;
    }

    public void SetDir(Vector3 inputDir, bool stop, Transform target = null)
    {
        if (isMenualCtr) { return; }

        if (!stop) isMove = true;
        else isMove = false;

        lookDir = CalDir(inputDir);

        if (target != null)
            if (!isMove)
            {
                lookDir = (target.position - anim.transform.position).normalized;

                lookDir = CalDir((target.position - anim.transform.position).normalized);
            }
    }

    public void SetAnim()
    {
        if (isMenualCtr) { return; }


        if (isMove)
            anim.SetBool("isMove", true);
        else
            anim.SetBool("isMove", false);

        anim.SetFloat("fHorizontal", lookDir.x);
        anim.SetFloat("fVertical", lookDir.y);
    }

    public void CheckHaveItem(bool isHave) { anim.SetBool("isHave", isHave); }
    public void CheckHat(bool isEquip) { anim.SetBool("isHat", isEquip); }
 
    public void HatState(HatData hatData, SpriteRenderer hatSpriteRenderer)
    {
        if(hatData != null)
        {
            if (lookDir.y == 0)
            {
                hatSpriteRenderer.sprite = hatData.sideSprite;

                if (lookDir.x > 0)
                    hatSpriteRenderer.flipX = false;
                else
                    hatSpriteRenderer.flipX = true;
            }
            if (lookDir.x == 0)
            {
                if (lookDir.y > 0)
                    hatSpriteRenderer.sprite = hatData.backSprite;
                else
                    hatSpriteRenderer.sprite = hatData.frontSprite;
            }

            if(lookDir == Vector3.zero) hatSpriteRenderer.sprite = hatData.frontSprite;
        }
        else
            hatSpriteRenderer.sprite = null;
    }
}
