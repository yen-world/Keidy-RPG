using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int count;
    public string pickUpSound;

    private void OnTriggerStay2D(Collider2D other)
    {
        AudioManager.instance.Play(pickUpSound);
        Inventory.instance.GetAnItem(itemID, count);
        Destroy(this.gameObject);
    }
}
