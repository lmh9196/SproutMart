using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/npcData", order = 6)]
public class NpcData : ScriptableObject
{
    public float defalutMoveSpeed;
    public float buffSpeed;

    public UnityEngine.U2D.Animation.SpriteLibraryAsset[] npcTypeArry;

    public Sprite counterSprite;
    public Sprite[] exitSpritesArry;

    [HideInInspector] public float moveSpeed;

    public void SetMoveSpeed() { moveSpeed = defalutMoveSpeed + buffSpeed; }
}
