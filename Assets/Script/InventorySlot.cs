using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    // 아이템의 이미지
    public Image icon;
    // 아이템의 이름
    public TMP_Text itemName_Text;
    // 아이템의 갯수
    public TMP_Text itemCount_Text;
    // 아이템 슬롯의 패널
    public GameObject selected_Item;

    public void AddItem(Item _item)
    {
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        if (Item.ItemType.Use == _item.itemType)
        {
            if (_item.itemCount > 0)
                itemCount_Text.text = "X " + _item.itemCount.ToString();
            else
                itemCount_Text.text = "";
        }
    }

    public void RemoveItem()
    {
        itemCount_Text.text = "";
        itemName_Text.text = "";
        icon.sprite = null;
    }
}
