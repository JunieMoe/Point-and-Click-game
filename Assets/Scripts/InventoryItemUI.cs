using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IDropHandler
{
    public ItemData itemData;
    public Movement player;

    void Start()
    {
        player = FindObjectOfType<Movement>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData.isIngredient && eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData.combineWith == itemData)
        {
            Debug.Log("combined "+ eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData+"with "+itemData);
            Debug.Log("created " + eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData.result);
            Destroy(gameObject);
            Destroy(eventData.pointerDrag);
            player.inventory.RemoveItem(itemData);
            player.inventory.RemoveItem(eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData);
            player.inventory.AddItem(itemData.result);
            player.inventoryUI.RefreshInventory();
        }
    }
}
