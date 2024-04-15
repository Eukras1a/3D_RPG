using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(DialogueData_SO))]
public class DialogueCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open in Editor"))
        {
            DialogueEditor.InitWindow((DialogueData_SO)target);
        }
        base.OnInspectorGUI();
    }
}
public class DialogueEditor : EditorWindow
{
    DialogueData_SO currentData;
    ReorderableList piecesList = null;
    Vector2 scrollPostion = Vector2.zero;
    Dictionary<string, ReorderableList> optionList = new Dictionary<string, ReorderableList>();
    [MenuItem("Editor/Dialogue Editor")]
    public static void Init()
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");
        editorWindow.autoRepaintOnSceneChange = true;
    }
    public static void InitWindow(DialogueData_SO data)
    {
        DialogueEditor editorWindow = GetWindow<DialogueEditor>("Dialogue Editor");
        editorWindow.currentData = data;
    }
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        DialogueData_SO data = EditorUtility.InstanceIDToObject(instanceID) as DialogueData_SO;
        if (data != null)
        {
            DialogueEditor.InitWindow(data);
            return true;
        }
        return false;
    }
    private void OnSelectionChange()
    {
        var newData = Selection.activeObject as DialogueData_SO;
        if (newData != null)
        {
            currentData = newData;
            SetUpReorderableList();
        }
        else
        {
            currentData = null;
            piecesList = null;
        }
        Repaint();
    }
    void OnGUI()
    {
        if (currentData != null)
        {
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if (piecesList == null)
            {
                SetUpReorderableList();
            }
            piecesList.DoLayoutList();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            if (GUILayout.Button("Create New Dialogue"))
            {
                string dataPath = "Assets/Game Data/Dialogue Data/";
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }
                DialogueData_SO newData = ScriptableObject.CreateInstance<DialogueData_SO>();
                AssetDatabase.CreateAsset(newData, dataPath + "/" + "New Dialogue.asset");
                currentData = newData;
            }
            EditorGUILayout.LabelField("NO DATA SELECTED!", EditorStyles.boldLabel);
        }
    }
    private void OnDisable()
    {
        optionList.Clear();
    }
    void SetUpReorderableList()
    {
        piecesList = new ReorderableList(currentData.dialoguePieces, typeof(DialoguePiece), true, true, true, true);
        piecesList.drawHeaderCallback += OnDrawPieceHeader;
        piecesList.drawElementCallback += OnDrawElement;
        piecesList.elementHeightCallback += OnDrawHeight;
    }
    private float OnDrawHeight(int index)
    {
        var height = EditorGUIUtility.singleLineHeight;
        var isExpand = currentData.dialoguePieces[index].canExpand;
        if (isExpand)
        {
            height += EditorGUIUtility.singleLineHeight * 9;
            if (currentData.dialoguePieces[index].options.Count > 1)
            {
                height += EditorGUIUtility.singleLineHeight * currentData.dialoguePieces[index].options.Count;
            }
        }
        return height;
    }
    private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorUtility.SetDirty(currentData);
        GUIStyle style = new GUIStyle("TextField")
        {
            wordWrap = true
        };
        if (index < currentData.dialoguePieces.Count)
        {
            var currentPiece = currentData.dialoguePieces[index];
            var tempRect = rect;
            tempRect.height = EditorGUIUtility.singleLineHeight;
            currentPiece.canExpand = EditorGUI.Foldout(tempRect, currentPiece.canExpand, currentPiece.ID);
            if (currentPiece.canExpand)
            {
                tempRect.width = 20;
                tempRect.y += tempRect.height;
                EditorGUI.LabelField(tempRect, "ID");
                tempRect.x += tempRect.width;
                tempRect.width = 80;
                currentPiece.ID = EditorGUI.TextField(tempRect, currentPiece.ID);

                tempRect.x += tempRect.width + 20;
                tempRect.width = 120;
                EditorGUI.LabelField(tempRect, "Task");
                currentPiece.task = (TaskData_SO)EditorGUI.ObjectField(tempRect, currentPiece.task, typeof(TaskData_SO), false);

                tempRect.y += EditorGUIUtility.singleLineHeight + 5;
                tempRect.x = rect.x;
                tempRect.height = 60;
                tempRect.width = tempRect.height;
                currentPiece.image = (Sprite)EditorGUI.ObjectField(tempRect, currentPiece.image, typeof(Sprite), false);

                tempRect.x += tempRect.width + 5;
                tempRect.width = rect.width - tempRect.x;
                currentPiece.text = EditorGUI.TextField(tempRect, currentPiece.text, style);

                tempRect.y += tempRect.height + 5;
                tempRect.x = rect.x;
                tempRect.width = rect.width;

                string optionKey = currentPiece.ID + currentPiece.text;
                if (optionKey != string.Empty)
                {
                    if (!optionList.ContainsKey(optionKey))
                    {
                        var option = new ReorderableList(currentPiece.options, typeof(DialogueOption), true, true, true, true);
                        option.drawHeaderCallback = OnDrawOptionHeader;
                        option.drawElementCallback = (optionRect, optionIndex, optionActive, optionFocused) =>
                        {
                            OnDrawOptionElement(currentPiece, optionRect, optionIndex, optionActive, optionFocused);
                        };
                        optionList[optionKey] = option;
                    }
                    optionList[optionKey].DoList(tempRect);
                }
            }
        }
    }
    private void OnDrawOptionHeader(Rect rect)
    {
        GUI.Label(rect, "Text");
        rect.x += (rect.width * 0.5f) + 10;
        GUI.Label(rect, "ID");
        rect.x += rect.width * 0.3f;
        GUI.Label(rect, "Apply");
    }
    private void OnDrawOptionElement(DialoguePiece currentPiece, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var currentOption = currentPiece.options[optionIndex];
        var tempRect = optionRect;

        tempRect.width = optionRect.width * 0.5f;
        currentOption.text = EditorGUI.TextField(tempRect, currentOption.text);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect, currentOption.targetID);

        tempRect.x += tempRect.width + 5;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeTask = EditorGUI.Toggle(tempRect, currentOption.takeTask);
    }
    private void OnDrawPieceHeader(Rect rect)
    {
        GUI.Label(rect, "Dailogue Pieces");
    }
}
