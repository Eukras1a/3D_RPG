using UnityEngine;
using UnityEngine.UI;

public class TaskNameButton : MonoBehaviour
{
    public Text taskNameText;
    public TaskData_SO currentData;
    public Text taskContentText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SetupContentText);
    }
    void SetupContentText()
    {
        taskContentText.text = currentData.taskDescription;
        TaskUI.Instance.SetupRequireList(currentData);
        TaskUI.Instance.SetupRewardList(currentData);
    }
    public void SetupName(TaskData_SO data)
    {
        currentData = data;
        if (data.isCompleted)
        {
            taskNameText.text = data.taskName + "£¨ÒÑÍê³É£©";
        }
        else
        {
            taskNameText.text = data.taskName;
        }
    }
}
