using UnityEngine;
using UnityEngine.UI;

public class TaskRequirement : MonoBehaviour
{
    private Text requireName;
    private Text progressNumber;

    private void Awake()
    {
        requireName = GetComponent<Text>();
        progressNumber = transform.GetChild(0).GetComponent<Text>();
    }
    public void SetupRequirement(TaskData_SO.TaskRequire r)
    {
        requireName.text = r.name;
        progressNumber.text = r.currentAmount.ToString() + "/" + r.requireAmount.ToString();
    }
    public void SetupRequirement(TaskData_SO.TaskRequire r,bool isFinished)
    {
        if (isFinished)
        {
            requireName.text = r.name;
            progressNumber.text = "Íê³É";
            requireName.color = Color.grey;
            progressNumber.color = Color.grey;
        }
    }
}
