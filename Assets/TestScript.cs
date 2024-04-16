using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    string path = "F:/SaveData/TASK1.TASK";
    public TaskData_SO task;
    private void Start()
    {
        TaskData1 task1 = ScriptableObject.CreateInstance<TaskData1>();
        var jsonString = JsonConvert.SerializeObject(task1, Formatting.Indented);
        File.WriteAllText(path, jsonString);
    }

}
[System.Serializable]
public class TaskData1 : ScriptableObject
{
    [System.Serializable]
    public class TaskRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }
    public string taskID;
    public string taskName;
    [TextArea] public string taskDescription;
    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;
    public List<TaskRequire> requireList = new List<TaskRequire>();
    //public List<PackageItem> rewardList = new List<PackageItem>();
}
