using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LogicGateSaveData
{
    public LogicGates_Type gateType;
    public bool isSolved;
}

[Serializable] 
public class PlacedItemData
{
    public string itemName; 
    public float posX;      
    public float posY;  
    public float posZ;      
}

[Serializable]
public class GameData
{
    public List<SceneItemsMap> allScenesData = new List<SceneItemsMap>();
    public List<LogicGateSaveData> savedLogicGates = new List<LogicGateSaveData>();
}

[Serializable]
public class SceneItemsMap
{
    public string sceneName;
    public List<PlacedItemData> items = new List<PlacedItemData>();
}

public class SaveManager : MonoBehaviour
{
    private static GameData currentGameData = new GameData();
    private static string saveFilePath;
    
    private void Awake()
    {
        
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_save.json");
        LoadGame(); 
    }

    // Save the state of logic gates
    public static void SaveLogicGates(Dictionary<LogicGates_Type, bool> solvedComponents)
    {
        currentGameData.savedLogicGates.Clear();

        foreach (KeyValuePair<LogicGates_Type, bool> kvp in solvedComponents)
        {
            currentGameData.savedLogicGates.Add(new LogicGateSaveData 
            { 
                gateType = kvp.Key, 
                isSolved = kvp.Value 
            });
        }

        SaveCurrentDataToDisk();
    }

    // Load the state of logic gates
    public static Dictionary<LogicGates_Type, bool> GetSavedLogicGates()
    {
        LoadGame(); 
        Dictionary<LogicGates_Type, bool> loadedDictionary = new Dictionary<LogicGates_Type, bool>();

        foreach (LogicGateSaveData data in currentGameData.savedLogicGates)
        {
            loadedDictionary[data.gateType] = data.isSolved;
        }

        return loadedDictionary;
    }
    
    // Save a single placed item in the current scene
    public static void SaveSinglePlacedItem(string itemName, Vector3 position)
    {
        LoadGame(); 

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneItemsMap targetSceneMap = currentGameData.allScenesData.Find(s => s.sceneName == currentScene);

        if (targetSceneMap == null)
        {
            targetSceneMap = new SceneItemsMap { sceneName = currentScene };
            currentGameData.allScenesData.Add(targetSceneMap);
        }

        targetSceneMap.items.Add(new PlacedItemData
        {
            itemName = itemName,
            posX = position.x,
            posY = position.y,
            posZ = position.z
        });

        SaveCurrentDataToDisk();
        Debug.Log($"Saved item in scene: {currentScene}");
    }

    // Retrieve all placed items for a specific scene
    public static List<PlacedItemData> GetItemsBySceneName(string sceneName)
    {
        LoadGame(); 

        SceneItemsMap targetSceneMap = currentGameData.allScenesData.Find(s => s.sceneName == sceneName);
        Debug.Log($"Retrieved items for scene: {sceneName}, count: {(targetSceneMap != null ? targetSceneMap.items.Count : 0)}");
        
        return targetSceneMap != null ? targetSceneMap.items : new List<PlacedItemData>();
    }

    // Load the game data from disk
    private static void SaveCurrentDataToDisk()
    {
        string json = JsonUtility.ToJson(currentGameData, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "game_save.json"), json);
    }
    
    private static void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "game_save.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentGameData = JsonUtility.FromJson<GameData>(json);
            if (currentGameData == null)
            {
                currentGameData = new GameData();
            }
        }
        else
        {
            currentGameData = new GameData();
        }
    }

    
    public static void ClearSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "game_save.json");

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        currentGameData = new GameData();
        Debug.Log("Save data cleared.");
    }
}