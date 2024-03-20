using System.Collections.Generic;
using UnityEngine;
using static TaskManager;

[System.Serializable]
public class FileData
{
    public string fileName;
    public string createTime;
    public string lastScene;
    public int playerPrefabID;
    public PlayerSceneLocation playerLocationOnSceneLoad;
    public CharacterData_SO playerData;
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
