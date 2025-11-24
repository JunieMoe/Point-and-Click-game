using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Inventory playerInventory;
    public string entranceNodeName;

    public SaveData saveData;

    public string saveFilePath => Application.persistentDataPath + "/save.json";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInventory = new Inventory();

        LoadGame();
    }

    public void SaveGame()
    {
        if (saveData == null)
            saveData = new SaveData();

        // Save current inventory
        saveData.inventoryItemNames.Clear();
        foreach (var item in playerInventory.GetItems())
            saveData.inventoryItemNames.Add(item.itemName);

        // Save picked-up item IDs (based on current scene)
        saveData.pickedUpItemIDs.Clear();
        Item[] allItems = FindObjectsOfType<Item>();
        foreach (Item item in allItems)
            saveData.pickedUpItemIDs.Add(item.gameObject.name); 

        saveData.currentScene = SceneManager.GetActiveScene().name;
        saveData.entranceNodeName = entranceNodeName;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }
    // Load save data from external file
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(json);

            entranceNodeName = saveData.entranceNodeName;

            SceneManager.LoadScene(saveData.currentScene);
        }
        else
        {
            saveData = new SaveData();
        }
    }
    // Retrieve inventory data from save file and load into player inventory by item name
    public void InitializeInventoryFromSave(List<ItemData> allItemData)
    {
        playerInventory = new Inventory();

        foreach (string itemName in saveData.inventoryItemNames)
        {
            ItemData item = allItemData.Find(i => i.itemName == itemName);
            if (item != null)
                playerInventory.AddItem(item);
        }
    }

    // ---- SCENE STATE MANAGEMENT ----

    // Get the dictionary representing the saved state of the scene by name
    private SceneStateData GetOrCreateSceneState(string sceneName)
    {
        var sceneState = saveData.sceneStates.Find(s => s.sceneName == sceneName);
        if (sceneState == null)
        {
            sceneState = new SceneStateData { sceneName = sceneName };
            saveData.sceneStates.Add(sceneState);
        }
        return sceneState;
    }

    // Get scene interaction dictionary (key->value pairs)
    public Dictionary<string, string> GetSceneInteractionDictionary(string sceneName)
    {
        var sceneState = GetOrCreateSceneState(sceneName);
        return sceneState.ToDictionary();
    }

    // Save updated dictionary back into SaveData for the scene
    public void SetSceneInteractionDictionary(string sceneName, Dictionary<string, string> dict)
    {
        var sceneState = GetOrCreateSceneState(sceneName);
        sceneState.FromDictionary(dict);
    }

    // Set one interaction's state by key and value for the current scene
    public void SetInteractionState(string interactionKey, string interactionValue)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var dict = GetSceneInteractionDictionary(currentScene);
        dict[interactionKey] = interactionValue;
        SetSceneInteractionDictionary(currentScene, dict);
    }

    // Get one interaction's state by key for the current scene, or default if not found
    public string GetInteractionState(string interactionKey, string defaultValue = null)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var dict = GetSceneInteractionDictionary(currentScene);
        if (dict.TryGetValue(interactionKey, out string value))
        {
            return value;
        }
        return defaultValue;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
