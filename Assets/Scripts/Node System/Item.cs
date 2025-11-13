using UnityEngine;
using System.IO;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public MoveNode itemNode;

    void Start()
    {
        if (GameManager.Instance == null) return;

        string myID = gameObject.name;
        if (File.Exists(GameManager.Instance.saveFilePath))
        {
            string json = File.ReadAllText(GameManager.Instance.saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data.pickedUpItemIDs.Contains(myID))
            {
                Destroy(gameObject);
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.saveData != null)
        {
            if (GameManager.Instance.saveData.pickedUpItemIDs.Contains(gameObject.name))
            {
                Destroy(gameObject);
            }
        }

        string state = GameManager.Instance.GetInteractionState("Item_" + gameObject.name, "available");
        if (state == "pickedUp")
        {
            // Hide or disable the item if already picked up
            Destroy(gameObject);
        }
    }
}
