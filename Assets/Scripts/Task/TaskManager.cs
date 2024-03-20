using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [System.Serializable]
    public class Task
    {
        public TaskData_SO taskData;
        public bool IsStarted
        {
            get
            {
                return taskData.isStarted;
            }
            set
            {
                taskData.isStarted = value;
            }
        }
        public bool IsFinished
        {
            get
            {
                return taskData.isFinished;
            }
            set
            {
                taskData.isFinished = value;
            }
        }
        public bool IsCompleted
        {
            get
            {
                return taskData.isCompleted;
            }
            set
            {
                taskData.isCompleted = value;
            }
        }
    }
    public List<Task> taskList = new List<Task>();

    private void Start()
    {
        LoadTask();
    }

    public bool HaveTask(TaskData_SO data)
    {
        if (data != null)
        {
            return taskList.Any(x => x.taskData.taskID == data.taskID);
        }
        else
            return false;
    }
    public Task GetTask(TaskData_SO data)
    {
        return taskList.Find(q => q.taskData.taskID == data.taskID);
    }
    public void UpdateTaskProgress(string name, int amount)
    {
        foreach (var task in taskList)
        {
            if (task.IsFinished)
            {
                continue;
            }
            var matchedTask = task.taskData.requireList.Find(r => r.name == name);
            if (matchedTask != null)
            {
                matchedTask.currentAmount += amount;
            }
            task.taskData.CheckTaskProgress();
        }
    }
    public void SaveTask()
    {
        PlayerPrefs.SetInt("TaskCount", taskList.Count);
        for (int i = 0; i < taskList.Count; i++)
        {
            SaveManager.Instance.Save(taskList[i].taskData, "task" + i);
        }
    }
    public void Save()
    {
        SaveManager.Instance.SaveTaskList(taskList);
    }
    public void LoadTask()
    {
        var taskCount = PlayerPrefs.GetInt("TaskCount");
        for (int i = 0; i < taskCount; i++)
        {
            var newTask = ScriptableObject.CreateInstance<TaskData_SO>();
            SaveManager.Instance.Load(newTask, "task" + i);
            taskList.Add(new Task { taskData = newTask });
        }
    }
}
