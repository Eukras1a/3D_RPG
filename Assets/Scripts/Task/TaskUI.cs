using UnityEngine;
using UnityEngine.UI;

public class TaskUI : Singleton<TaskUI>
{
    [Header("Elements")]
    public GameObject taskPanel;
    public Tooltip tooltip;
    bool isOpen = false;

    [Header("Task Name")]
    public RectTransform taskListTransform;
    public TaskNameButton taskNameButtonPrefab;

    [Header("Task Content")]
    public Text taskContent;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public TaskRequirement requirementPrefab;

    [Header("Reward")]
    public RectTransform rewardTransform;
    public ItemUI rewardPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isOpen = !isOpen;
            taskPanel.SetActive(isOpen);
            taskContent.text = string.Empty;
            SetUpTaskList();
            if (!isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
        }
    }
    public void SetUpTaskList()
    {
        foreach (Transform t in taskListTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in requireTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in rewardTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (var task in TaskManager.Instance.taskList)
        {
            var newTask = Instantiate(taskNameButtonPrefab, taskListTransform);
            newTask.SetupName(task.taskData);
            newTask.taskContentText = taskContent;
        }
    }
    public void SetupRequireList(TaskData_SO data)
    {
        foreach (Transform t in requireTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (var require in data.requireList)
        {
            var r = Instantiate(requirementPrefab, requireTransform);
            if (data.isFinished)
            {
                r.SetupRequirement(require, data.isFinished);
            }
            else
            {
                r.SetupRequirement(require);
            }
        }
    }
    public void SetupRewardList(TaskData_SO data)
    {
        foreach (Transform t in rewardTransform)
        {
            Destroy(t.gameObject);
        }
        foreach (var r in data.rewardList)
        {
            var item = Instantiate(rewardPrefab,rewardTransform);
            item.UpdateItemUI(r.itemData,r.amount);
        }
    }
    public void SetUI()
    {
        isOpen = false;
    }
}
