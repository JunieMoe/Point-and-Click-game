using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isIngredient;
    public ItemData combineWith;
    public ItemData result;
    public ItemData usedItem;
    public bool consumable;
}

