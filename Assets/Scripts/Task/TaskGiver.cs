using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class TaskGiver : MonoBehaviour
{
    public DialogueData_SO startDialogueData;
    public DialogueData_SO progressDialogueData;
    public DialogueData_SO completeDialogueData;
    public DialogueData_SO finishDialogueData;

    DialogueController controller;
    TaskData_SO currentTaskData;

    public bool IsStarted
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTaskData))
            {
                return TaskManager.Instance.GetTask(currentTaskData).IsStarted;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsCompleted
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTaskData))
            {
                return TaskManager.Instance.GetTask(currentTaskData).IsCompleted;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsFinished
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTaskData))
            {
                return TaskManager.Instance.GetTask(currentTaskData).IsFinished;
            }
            else
            {
                return false;
            }
        }
    }
    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    private void Start()
    {
        controller.currentData = startDialogueData;
        currentTaskData = controller.currentData.GetTaskData();
    }
    private void Update()
    {
        if (IsStarted)
        {
            if (IsCompleted)
            {
                controller.currentData = completeDialogueData;
            }
            else
            {
                controller.currentData = progressDialogueData;
            }
        }
        if (IsFinished)
        {
            controller.currentData = finishDialogueData;
        }
    }
}
