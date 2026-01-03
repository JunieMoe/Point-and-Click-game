using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform slotContainer;
    private Transform slotTemplate;
    private InventoryItemUI inventoryItemUI;

    private void Awake()
    {
        slotContainer = transform.Find("ItemSlotContainer");
        slotTemplate = slotContainer.Find("ItemSlot");

    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        //ensure items dont duplicate
        foreach (Transform child in slotContainer)
        {
            if (child == slotTemplate) continue;
            Destroy(child.gameObject);
        }


        int y = 0;
        float slotSize = 30f;
        foreach (ItemData item in inventory.GetItems())
        {
            RectTransform slotRectTransform = Instantiate(slotTemplate, slotContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.GetComponent<InventoryItemUI>().itemData = item;
            slotRectTransform.gameObject.SetActive(true);
            slotRectTransform.anchoredPosition = new Vector2(0, y*slotSize);
            y-=5;
        }
    }
}
