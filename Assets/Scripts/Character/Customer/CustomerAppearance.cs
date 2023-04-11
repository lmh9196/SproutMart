using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using Unity.VisualScripting;

public class CustomerAppearance
{
    public enum Type
    {
        CUSTOMER, DRUNKEN, NULL
    }
    public Type type;

    Image hopeImage;

    public void Init(Image _hopeImage) { hopeImage = _hopeImage; }

    public Type RegistAppearance(Type type, Animator anim, NpcData npcData)
    {
        switch (type)
        {
            case Type.CUSTOMER:
                int rand = Random.Range(0, npcData.npcTypeArry.Length);
                anim.GetComponent<SpriteLibrary>().spriteLibraryAsset = npcData.npcTypeArry[rand];
                return Type.CUSTOMER;
            case Type.DRUNKEN:
                return Type.DRUNKEN;
            default:
                return Type.NULL;
        }
    }

    public void ChangeThoughtBalloon(Sprite targetSprite) { hopeImage.sprite = targetSprite; }
}
