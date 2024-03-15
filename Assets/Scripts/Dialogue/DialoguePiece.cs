using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;
    public Sprite image;
    [TextArea]public string text;
    public TaskData_SO task;
    [HideInInspector]
    public bool canExpand;
    public List<DialogueOption> options = new List<DialogueOption>();
}
