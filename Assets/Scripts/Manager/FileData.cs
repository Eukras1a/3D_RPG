using UnityEngine;

[System.Serializable]
public class FileData
{
    public string fileName;
    public string createTime;
    public string lastScene;
    public int playerPrefabID;
    public PlayerSceneLocation playerLocationOnSceneLoad;
    public SavedPlayerInfo savedPlayerInfo;
    public void SavePlayerTransformInfo(string sceneName, Transform t)
    {
        lastScene = sceneName;
        playerLocationOnSceneLoad = new PlayerSceneLocation()
        {
            position = t.position,
            rotation = t.rotation,
        };
    }
}
[System.Serializable]
public class PlayerSceneLocation
{
    public Vector3 position;
    public Quaternion rotation;
}
[System.Serializable]
public class SavedPlayerInfo
{
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    public int killPoint;

    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
}
