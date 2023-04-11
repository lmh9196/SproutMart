using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DetectArea : MonoBehaviour
{
    [HideInInspector] public Table parentTable;


    BoxCollider2D boxCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Customer customer))
        {
            if(customer.shoppingList.Count > 0)
            {
                if (customer.shoppingList[0] == parentTable)
                {
                    if (parentTable.customerList.Count == 0) { AddCustomer(customer); }
                    else if (!parentTable.customerList.Contains(customer))
                    {
                        AddCustomer(customer);
                    }
                }
            }
        }
    }

   
    void AddCustomer(Customer customer)
    {
        parentTable.customerList.Add(customer);

        if (parentTable is CountTable)
        {
            CountTable parentCount = parentTable.GetComponent<CountTable>();

            if (parentCount.childCounter.gameObject.activeSelf) { parentCount.countWork.CompareCustomer(customer, parentCount.childCounter); }
        }
    }

    public void Init(Table _parentTable) 
    { 
        parentTable = _parentTable;

        transform.localScale = new Vector3(1, 1, 1);
        transform.position = parentTable.transform.position;

        boxCollider = GetComponent<BoxCollider2D>();

        BoxCollider2D parentColl = parentTable.GetComponent<BoxCollider2D>();

        boxCollider.offset = parentColl.offset;

        boxCollider.size = new Vector2(parentColl.size.x + 3.2f, parentColl.size.y + 3f);
    }
}
