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
    const string filePath = "F:/SaveData/SAVEDATA.SD";
    GameFileData gameFileData = new GameFileData();
    string currentSaveFileName;
    int playerID;
    CharacterData_SO data;
    bool isTrans;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadFileData();
        data = ScriptableObject.CreateInstance<CharacterData_SO>();
    }

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
    public void UpdateGameData()
    {
        if (GameManager.Instance.IsPlayerInitialized)
        {
            if (currentSaveFileName != null)
            {
                var player = GameManager.Instance.playerStates.characterData;
                var tempPlayerData = new SavedPlayerInfo()
                {
                    characterName = player.characterName,
                    maxHealth = player.maxHealth,
                    currentHealth = player.currentHealth,
                    baseDefence = player.baseDefence,
                    currentDefence = player.currentDefence,
                    killPoint = player.killPoint,
                    currentLevel = player.currentLevel,
                    maxLevel = player.maxLevel,
                    currentExp = player.currentExp,
                    baseExp = player.baseExp,
                    levelBuff = player.levelBuff,
                };
                var tempFileData = new FileData()
                {
                    fileName = currentSaveFileName,
                    createTime = DateTime.Now.ToString(),
                    playerPrefabID = playerID,
                    savedPlayerInfo = tempPlayerData,
                };
                tempFileData.SavePlayerTransformInfo(SceneManager.GetActiveScene().name, GameManager.Instance.playerStates.transform);
                gameFileData.currentGameFile = tempFileData.fileName;
                gameFileData.gameFiles.Add(tempFileData);
                SaveFileData();
                SaveM();
            }
        }
    }
    void SaveM()
    {
        InventoryManager.Instance.SaveData();
        TaskManager.Instance.SaveTask();
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
        //TODO:±³°ü´æµµ¸ÄÔì
        //InventoryManager.Instance.LoadData();
        currentSaveFileName = name;
        TaskManager.Instance.LoadTask();
        var temp = gameFileData.gameFiles.Find(gf => gf.fileName == name);
        SceneController.Instance.LoadGame(temp.playerPrefabID, temp.playerLocationOnSceneLoad.position, temp.playerLocationOnSceneLoad.rotation, temp.lastScene);

        data.characterName = temp.savedPlayerInfo.characterName;
        data.maxHealth = temp.savedPlayerInfo.maxHealth;
        data.currentHealth = temp.savedPlayerInfo.currentHealth;
        data.baseDefence = temp.savedPlayerInfo.baseDefence;
        data.currentDefence = temp.savedPlayerInfo.currentDefence;
        data.killPoint = temp.savedPlayerInfo.killPoint;
        data.currentLevel = temp.savedPlayerInfo.currentLevel;
        data.maxLevel = temp.savedPlayerInfo.maxLevel;
        data.currentExp = temp.savedPlayerInfo.currentExp;
        data.baseExp = temp.savedPlayerInfo.baseExp;
        data.levelBuff = temp.savedPlayerInfo.levelBuff;
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
                break;
            }
        }
        SaveFileData();
    }
    #region ÎÄ¼þ¶ÁÐ´
    public void SaveFileData()
    {
        DeleteEmptyOrRepeat();
        string jsonData = JsonUtility.ToJson(gameFileData, true);
        File.WriteAllText(filePath, jsonData);
    }
    void LoadFileData()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(data, gameFileData);
            DeleteEmptyOrRepeat();
        }
    }
    #endregion
    #region ´æµµ¼ì²â
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
    public void SetPlayerData()
    {
        if (SceneController.Instance.IsTrans)
        {
            GameManager.Instance.SetPlayerData(data);
        }
    }
    public void SetTransitionT()
    {
        isTrans = true;
    }
    public void SetTransitionF()
    {
        isTrans = false;
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