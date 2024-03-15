using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string saveScene = null;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.LoadMainScene();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            Debug.Log("Saved." + saveScene);
        }
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStates.characterData, "playerData");
        InventoryManager.Instance.SaveData();
        TaskManager.Instance.SaveTask();
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStates.characterData, "playerData");
        InventoryManager.Instance.LoadData();
    }
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString("saveScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
