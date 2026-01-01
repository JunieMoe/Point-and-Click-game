using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<string> inventoryItemNames = new List<string>();
    public List<SceneStateData> sceneStates = new List<SceneStateData>();

    public string currentScene;
    public string entranceNodeName;
}

[Serializable]
public class SceneStateData
{
    public string sceneName;
    public List<string> interactionKeys = new List<string>();
    public List<string> interactionValues = new List<string>();

    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();
        for (int i = 0; i < interactionKeys.Count; i++)
        {
            dict[interactionKeys[i]] = interactionValues[i];
        }
        return dict;
    }

    public void FromDictionary(Dictionary<string, string> dict)
    {
        interactionKeys.Clear();
        interactionValues.Clear();
        foreach (var kvp in dict)
        {
            interactionKeys.Add(kvp.Key);
            interactionValues.Add(kvp.Value);
        }
    }
}
