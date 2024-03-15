using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button currentButton;
    private DialoguePiece cueerntPiece;
    private string nextPieceID;
    private bool takeTask;
    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(OnOptionClick);
    }
    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        cueerntPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeTask = option.takeTask;
    }
    public void OnOptionClick()
    {
        if (cueerntPiece.task != null)
        {
            if (takeTask)
            {
                var newTask = new TaskManager.Task
                {
                    taskData = Instantiate(cueerntPiece.task)
                };
                if (TaskManager.Instance.HaveTask(newTask.taskData))
                {
                    if (TaskManager.Instance.GetTask(newTask.taskData).IsCompleted)
                    {
                        newTask.taskData.GiveRewards();
                        TaskManager.Instance.GetTask(newTask.taskData).IsFinished = true;
                    }
                }
                else
                {
                    TaskManager.Instance.taskList.Add(newTask);
                    TaskManager.Instance.GetTask(newTask.taskData).IsStarted = true;
                    foreach (var requireItem in newTask.taskData.RequireItemName())
                    {
                        InventoryManager.Instance.CheckTaskItem(requireItem);
                    }
                }
            }
        }
        if (nextPieceID == "")
        {
            DialogueManager.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            DialogueManager.Instance.UpdateMainDialogue(DialogueManager.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
