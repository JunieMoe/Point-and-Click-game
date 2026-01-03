using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public MoveNode itemNode;

    void Start()
    {
        string state = GameManager.Instance.GetInteractionState("Item_" + gameObject.name, "available");
        if (state == "pickedUp")
        {
            // get rid of the item if already picked up
            Destroy(gameObject);
        }
    }
}
