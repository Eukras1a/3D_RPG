using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuUI : MonoBehaviour
{
    GameObject mainMenu;
    GameObject saveMenu;
    GameObject confirmMenu;
    public SaveFileButton SaveButtonPrefab;
    [Header("Main Menu")]
    public Button saveButton;
    public Button createButton;
    public Button loadButton;
    public Button deleteButton;
    public Button exitButton;
    [Header("Confirm Menu")]
    public InputField fileName;
    public Button confirm;
    public Button cancel;
    public RectTransform dataListTransform;

    string currentSelectFile;

    enum EscapeMenuState
    {
        None,
        Main,
        Save,
        Confirm,
    }
    EscapeMenuState menu = EscapeMenuState.None;
    #region 周期函数
    private void Awake()
    {
        mainMenu = transform.GetChild(0).gameObject;
        saveMenu = transform.GetChild(1).gameObject;
        confirmMenu = transform.GetChild(2).gameObject;
        ChangeMenuStates(EscapeMenuState.None);
    }
    private void OnEnable()
    {
        confirm.onClick.AddListener(OnConfirm);
        cancel.onClick.AddListener(OnCancel);
        saveButton.onClick.AddListener(OnLoadSaveMenu);
        createButton.onClick.AddListener(OnCreate);
        loadButton.onClick.AddListener(OnLoad);
        deleteButton.onClick.AddListener(OnDelete);
        exitButton.onClick.AddListener(OnExitGame);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu == EscapeMenuState.Main)
            {
                GameManager.Instance.IsStopGame = false;
                Time.timeScale = 1;
                ChangeMenuStates(EscapeMenuState.None);
            }
            else
            {
                GameManager.Instance.IsStopGame = true;
                Time.timeScale = 0;
                ChangeMenuStates(EscapeMenuState.Main);
            }
        }
        if (currentSelectFile != null)
        {
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;
        }
    }
    private void OnDisable()
    {
        confirm.onClick.RemoveListener(OnConfirm);
        cancel.onClick.RemoveListener(OnCancel);
        saveButton.onClick.RemoveListener(OnLoadSaveMenu);
        createButton.onClick.RemoveListener(OnCreate);
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
        exitButton.onClick.RemoveListener(OnExitGame);
    }
    #endregion
    #region 响应事件
    void OnLoad()
    {
        ChangeMenuStates(EscapeMenuState.None);
        SaveManager.Instance.LoadGameData(currentSelectFile);
    }
    void OnDelete()
    {
        if (currentSelectFile != null)
        {
            SaveManager.Instance.DeleteData(currentSelectFile);
            ReadSavedFileData();
        }
    }
    void OnCreate()
    {
        ChangeMenuStates(EscapeMenuState.Confirm);
    }
    void OnConfirm()
    {
        SaveManager.Instance.SaveFile(fileName.text);
        OnLoadSaveMenu();
    }
    void OnCancel()
    {
        ChangeMenuStates(EscapeMenuState.Save);
    }
    void OnLoadSaveMenu()
    {
        ChangeMenuStates(EscapeMenuState.Save);
        ReadSavedFileData();
        fileName.text = null;
    }
    void OnExitGame()
    {
        GameManager.Instance.IsStopGame = false;
        Time.timeScale = 1;
        SceneController.Instance.LoadMainScene();
    }
    #endregion
    public void RigisterFile(string id)
    {
        currentSelectFile = id;
    }
    void ChangeMenuStates(EscapeMenuState state)
    {
        menu = state;
        mainMenu.SetActive(false);
        saveMenu.SetActive(false);
        confirmMenu.SetActive(false);
        currentSelectFile = null;
        switch (state)
        {
            case EscapeMenuState.None:
                GameManager.Instance.IsStopGame = false;
                Time.timeScale = 1;
                break;
            case EscapeMenuState.Main:
                mainMenu.SetActive(true);
                break;
            case EscapeMenuState.Save:
                saveMenu.SetActive(true);
                break;
            case EscapeMenuState.Confirm:
                confirmMenu.SetActive(true);
                break;
        }
    }
    void ReadSavedFileData()
    {
        foreach (Transform t in dataListTransform)
        {
            Destroy(t.gameObject);
        }
        if (SaveManager.Instance.GetSavedFileData().Count > 0)
        {
            foreach (var item in SaveManager.Instance.GetSavedFileData())
            {
                var newData = Instantiate(SaveButtonPrefab, dataListTransform);
                newData.SetUpDataInfo(item.fileName, item.createTime);
            }
        }

    }
}
