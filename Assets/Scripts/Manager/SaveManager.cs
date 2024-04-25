using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum FileDataState
{
    None,
    Create,
    Override,
}
public class SaveManager : Singleton<SaveManager>
{
    GameFileData gameFileData = new GameFileData();
    string currentSaveFileName;
    int playerID;
    int taskCount;
    string filePath;
    string DataPath
    {
        get
        {
            return filePath + "/SAVEDATA.DATA";
        }
    }
    protected override void Awake()
    {
        base.Awake();
        filePath = Application.persistentDataPath + "/GameData";
        DontDestroyOnLoad(this);
        LoadFileData();
    }
    #region S/L
    public void Save(UnityEngine.Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    public void Load(UnityEngine.Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    #endregion
    #region Gamedata
    public void UpdateGameData()
    {
        if (GameManager.Instance.IsPlayerInitialized)
        {
            if (currentSaveFileName != null)
            {
                TaskManager.Instance.SaveTask(currentSaveFileName);
                InventoryManager.Instance.SaveData(currentSaveFileName);

                var tempFileData = new FileData()
                {
                    fileName = currentSaveFileName,
                    createTime = DateTime.Now.ToString(),
                    playerPrefabID = playerID,
                    taskCount = taskCount,
                    characterData = GameManager.Instance.playerStates.characterData,
                };
                tempFileData.SavePlayerTransformInfo(SceneManager.GetActiveScene().name, GameManager.Instance.playerStates.transform);
                gameFileData.currentGameFile = tempFileData.fileName;
                gameFileData.gameFiles.Add(tempFileData);
                SaveFileData();
            }
        }
    }
    public void SaveGameData(string name)
    {
        if (name != null)
        {
            if (RepeatFileData(name))
            {
                currentSaveFileName = name;
                UpdateGameData();
            }
        }
    }
    public void LoadGameData(string name)
    {
        DeleteEmptyOrRepeat();
        currentSaveFileName = name;
        var temp = GetFile(name);
        TaskManager.Instance.LoadTask(temp.taskCount, name);
        SceneController.Instance.LoadGame(temp.playerPrefabID, temp.playerLocationOnSceneLoad.position, temp.playerLocationOnSceneLoad.rotation, temp.lastScene);
    }
    FileData GetFile(string name)
    {
        return gameFileData.gameFiles.Find(gf => gf.fileName == name);
    }
    public void DeleteGameData(string name)
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
                Delete(name);
                break;
            }
        }
        SaveFileData();
    }
    #endregion
    #region 文件读写
    public void SaveFileData()
    {
        DeleteEmptyOrRepeat();
        var jsonString = JsonConvert.SerializeObject(gameFileData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);//检查路径是否存在，不存在则创建
        }
        File.WriteAllText(DataPath, jsonString);//写入文件
    }
    void LoadFileData()
    {
        if (File.Exists(DataPath))
        {
            var filecontents = File.ReadAllText(DataPath);//读取文件
            gameFileData = JsonConvert.DeserializeObject<GameFileData>(filecontents);//转录成对应格式数据
        }
    }
    #endregion
    #region 存档检测
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
    #endregion
    #region Get/Set
    public void GetInventoryData()
    {
        InventoryManager.Instance.LoadData(currentSaveFileName);
    }
    public void SetPlayerData()
    {
        if (!SceneController.Instance.IsTrans)
        {
            if (currentSaveFileName != null)
            {
                GameManager.Instance.SetPlayerData(GetFile(currentSaveFileName).characterData);
            }
        }
    }
    public void SetTaskCount(int count)
    {
        taskCount = count;
    }
    public void RigisterPlayerID(int id)
    {
        playerID = id;
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