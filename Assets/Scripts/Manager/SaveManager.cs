using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TaskManager;
public enum FileDataState
{
    None,
    Create,
    Override,
}
public class SaveManager : Singleton<SaveManager>
{
    const string filePath = "F:/SaveData/SAVEDATA.SD";
    GameFileData gameFileData = new GameFileData();
    string currentSaveFileName;
    List<Task> taskList;
    int playerID;
    public string SaveScene
    {
        get
        {
            return PlayerPrefs.GetString("saveScene");
        }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadFileGameData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("Saved.");
        }
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStates.characterData, "playerData");
        InventoryManager.Instance.SaveData();
        TaskManager.Instance.SaveTask();
        TaskManager.Instance.Save();
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStates.characterData, "playerData");
        InventoryManager.Instance.LoadData();
    }
    public void Save(UnityEngine.Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString("saveScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(UnityEngine.Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
    public void UpdateGameFileData()
    {
        var tempFileData = new FileData()
        {
            fileName = currentSaveFileName,
            createTime = DateTime.Now.ToString(),
            playerPrefabID = playerID,
            playerData = GameManager.Instance.playerStates.characterData,
        };
        tempFileData.SavePlayerTransformInfo(SceneManager.GetActiveScene().name, GameManager.Instance.playerStates.transform);
        gameFileData.currentGameFile = tempFileData.fileName;
        gameFileData.gameFiles.Add(tempFileData);
    }
    public void SaveGameData()
    {
        UpdateGameFileData();
        DeleteEmptyOrRepeat();
        InventoryManager.Instance.SaveData();
        TaskManager.Instance.Save();
        string jsonData = JsonUtility.ToJson(gameFileData, true);
        File.WriteAllText(filePath, jsonData);
    }
    void LoadFileGameData()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(data, gameFileData);
            DeleteEmptyOrRepeat();
        }
    }
    public void LoadGameData(string name)
    {
        DeleteEmptyOrRepeat();
        currentSaveFileName = name;
        InventoryManager.Instance.LoadData();
        //SceneController.Instance.LoadGame(CurrentFileData.playerPrefabID, CurrentFileData.playerLocationOnSceneLoad.position, CurrentFileData.playerLocationOnSceneLoad.rotation, CurrentFileData.lastScene);
    }
    public void DeleteData(string name)
    {
        if (currentSaveFileName == name)
        {
            currentSaveFileName = null;
        }
        for (int i = gameFileData.gameFiles.Count - 1; i >= 0; i--)
        {
            if (gameFileData.gameFiles[i].fileName == name)
            {
                gameFileData.gameFiles.Remove(gameFileData.gameFiles[i]);
                break;
            }
        }
        SaveGameData();
    }
    void DeleteEmptyOrRepeat()
    {
        for (int i = gameFileData.gameFiles.Count - 1; i >= 0; i--)
        {
            if (gameFileData.gameFiles[i].fileName == null)
            {
                gameFileData.gameFiles.Remove(gameFileData.gameFiles[i]);
                break;
            }
        }
        for (int i = 0; i < gameFileData.gameFiles.Count; i++)
        {
            for (int j = gameFileData.gameFiles.Count - 1; j > i; j--)
            {
                if (gameFileData.gameFiles[j].fileName == gameFileData.gameFiles[i].fileName)
                {
                    gameFileData.gameFiles.Remove(gameFileData.gameFiles[j]);
                }
            }
        }
    }
    bool RepeatFileData(string name)
    {
        foreach (var item in gameFileData.gameFiles)
        {
            if (item.fileName == name)
            {
                return false;
            }
        }
        return true;
    }

    #region Get/Set
    public void SaveFile(string name)
    {
        if (RepeatFileData(name))
        {
            if (name != null)
            {
                currentSaveFileName = name;
                SaveGameData();
            }
        }
    }
    public void RigisterPlayerID(int id)
    {
        playerID = id;
    }
    public void SaveTaskList(List<Task> list)
    {
        taskList = list;
    }
    public List<FileData> GetSavedFileData()
    {
        return gameFileData.gameFiles;
    }
    #endregion
    [System.Serializable]
    public class GameFileData
    {
        public string currentGameFile;
        public List<FileData> gameFiles = new List<FileData>();
    }
}