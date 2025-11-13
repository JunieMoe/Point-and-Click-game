using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemData> items;
    public Inventory()
    {
        items = new List<ItemData>();
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    public List<ItemData> GetItems()
    {
        return items;
    }
}
