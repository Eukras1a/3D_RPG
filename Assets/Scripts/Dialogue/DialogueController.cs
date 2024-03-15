using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    bool canTalk;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }
    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            if (currentData.dialoguePieces[0] != null)
            {
                StartDialogue();
            }
        }
    }
    void StartDialogue()
    {
        DialogueManager.Instance.UpdateDialogueData(currentData);
        DialogueManager.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}
