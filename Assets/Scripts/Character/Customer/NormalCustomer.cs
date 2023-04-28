using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCustomer : Customer
{
    protected override void Awake() { base.Awake(); }

    public override void Start() { base.Start(); }
    protected override void Update()
    {
        if (!isActive && StageManager.instance.salesUnLockTableList.Count > 0) EnableCustomer();

        base.Update();
    }

    void EnableCustomer()
    {
        appearance.type = appearance.RegistAppearance(CustomerAppearance.Type.CUSTOMER, animDir.anim, npcData);
        customerShopping.RegistShoppingList(shoppingList, shoppingKindLimitCount, counterTable, exitTable);

        ResetHandsCount();

        SetBox();

        if(shoppingList.Count > 0)
        {
            if (shoppingList[0] == exitTable) { appearance.ChangeThoughtBalloon(shoppingList[0].exitWork.SelectSign()); }
            else if (shoppingList[0] == counterTable) { appearance.ChangeThoughtBalloon(shoppingList[0].countWork.hopeSprite); }
            else { appearance.ChangeThoughtBalloon(shoppingList[0].salesWork.hopeSprite); }
        }

        isActive = true;

        ResetCollider();
    }
}
