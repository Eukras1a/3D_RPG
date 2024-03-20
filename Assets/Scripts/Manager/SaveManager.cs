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
    FileData currentFileData;
    string currentSaveFileName;
    List<Task> taskList;
    GameFileData gameFileData = new GameFileData();

    int tempIndex;
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
        LoadExistGameData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("Saved.");
        }
    }
    public void ExitGame()
    {
        SceneController.Instance.LoadMainScene();
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
        currentFileData = new FileData()
        {
            fileName = currentSaveFileName,
            createTime = DateTime.Now.ToString(),
            taskList = taskList,
            playerData = GameManager.Instance.playerStates.characterData,
        };
        currentFileData.SavePlayerTransformInfo(SceneManager.GetActiveScene().name, GameManager.Instance.playerStates.transform);
        gameFileData.currentGameFile = currentSaveFileName;
        gameFileData.gameFiles.Add(currentFileData);
    }
    public void SaveGameData()
    {
        UpdateGameFileData();
        string jsonData = JsonUtility.ToJson(gameFileData, true);
        File.WriteAllText(filePath, jsonData);
    }
    void LoadExistGameData()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            gameFileData = JsonUtility.FromJson<GameFileData>(data);
        }
    }
    public void DeleteData(string name)
    {
        for (int i = 0; i < gameFileData.gameFiles.Count; i++)
        {
            if (gameFileData.gameFiles[i].fileName == name)
            {
                gameFileData.gameFiles.RemoveAt(i);
            }
        }
    }

    #region Get/Set
    public void SaveFile(string name)
    {
        currentSaveFileName = name;
        SaveGameData();
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