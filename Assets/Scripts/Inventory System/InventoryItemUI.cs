using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemUI : MonoBehaviour, IDropHandler
{
    public ItemData itemData;
    public Movement player;
    public Image itemSprite;

    void Start()
    {
        player = FindObjectOfType<Movement>();
        itemSprite = gameObject.GetComponentInChildren<Image>();
        itemSprite.sprite = itemData.icon;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData.isIngredient && eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData.combineWith == itemData)
        {
            Destroy(gameObject);
            Destroy(eventData.pointerDrag);
            player.inventory.RemoveItem(itemData);
            player.inventory.RemoveItem(eventData.pointerDrag.GetComponent<InventoryItemUI>().itemData);
            player.inventory.AddItem(itemData.result);
            player.inventoryUI.RefreshInventory();
        }
    }
}
