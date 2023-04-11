using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/BoxData", order = 9)]
public class BoxData :ScriptableObject
{
    public Sprite horizontal;
    public Sprite vertical;
    public Sprite horizontalCover;
    public Sprite verticalCover;

    public void UpdateBoxSprite(Vector3 dir, SpriteRenderer boxSpriteRenderer, SpriteRenderer coverSpriteRenderer)
    {
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            boxSpriteRenderer.sprite = horizontal;
            coverSpriteRenderer.sprite = horizontalCover;
        }
        else
        {
            boxSpriteRenderer.sprite = vertical;
            coverSpriteRenderer.sprite = verticalCover;
        }
    }
    public void UpdateFullSign(GameObject fullSign, Transform itemBox, int maxHandsCount)
    {
        if (itemBox.childCount < maxHandsCount) { fullSign.SetActive(false); }
        else
        {
            fullSign.SetActive(true);
            fullSign.transform.position = itemBox.GetChild(itemBox.childCount - 1).transform.position + new Vector3(0, 0.7f, 0);
        }
    }

    public void UpdateCropsPos(Transform itemBox)
    {
        for (int i = 0; i < itemBox.childCount; i++)
        {
            if (i == 0) { itemBox.GetChild(i).localPosition = new Vector3(0, 0.05f, 0); }
            else { itemBox.GetChild(i).localPosition = (itemBox.GetChild(i - 1).localPosition + new Vector3(0, 0.32f, -0.4f)); }
        }
    }


    public CharacterAnimDir CheckParent(Transform BoxForm)
    {
        switch (BoxForm.parent.gameObject.layer)
        {
            case 3:
                return BoxForm.parent.GetComponent<Player>().animDir;
            case 7:
                return BoxForm.parent.GetComponent<Customer>().animDir;
            case 9:
                return BoxForm.parent.GetComponent<Staff>().animDir;
            default:
                Debug.Log("¿À·ù!");
                return null;
        }
    }
}
