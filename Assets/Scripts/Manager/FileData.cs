using UnityEngine;

[System.Serializable]
public class FileData
{
    public string fileName;
    public string createTime;
    public string lastScene;
    public int playerPrefabID;
    public int taskCount;
    public PlayerSceneLocation playerLocationOnSceneLoad;
    public CharacterData_SO characterData;
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
