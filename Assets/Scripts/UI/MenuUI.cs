using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    GameObject mainMenu;
    GameObject archiveMenu;
    GameObject confirmMenu;
    public SaveFileButton SaveButtonPrefab;
    [Header("Main Menu")]
    public Button archiveButton;
    public Button settingButton;
    public Button exitButton;
    [Header("Archive Menu")]
    public Button createButton;
    public Button loadButton;
    public Button deleteButton;
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
        Archive,
        Confirm,
    }
    EscapeMenuState menu = EscapeMenuState.None;
    #region 周期函数
    private void Awake()
    {
        mainMenu = transform.GetChild(0).gameObject;
        archiveMenu = transform.GetChild(1).gameObject;
        confirmMenu = transform.GetChild(2).gameObject;
        ChangeMenuStates(EscapeMenuState.None);
    }
    private void OnEnable()
    {
        confirm.onClick.AddListener(OnConfirm);
        cancel.onClick.AddListener(OnCancel);
        archiveButton.onClick.AddListener(OnLoadArchiveMenu);
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
        archiveButton.onClick.RemoveListener(OnLoadArchiveMenu);
        createButton.onClick.RemoveListener(OnCreate);
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
        exitButton.onClick.RemoveListener(OnExitGame);
    }
    #endregion
    #region 响应事件
    void OnLoad()
    {
        if (currentSelectFile != null)
        {
            SaveManager.Instance.LoadGameData(currentSelectFile);
            currentSelectFile = null;
            ChangeMenuStates(EscapeMenuState.None);
        }
    }
    void OnDelete()
    {
        if (currentSelectFile != null)
        {
            SaveManager.Instance.DeleteGameData(currentSelectFile);
            ReadSavedFileData();
        }
    }
    void OnCreate()
    {
        ChangeMenuStates(EscapeMenuState.Confirm);
    }
    void OnConfirm()
    {
        SaveManager.Instance.SaveGameData(fileName.text);
        OnLoadArchiveMenu();
    }
    void OnCancel()
    {
        ChangeMenuStates(EscapeMenuState.Archive);
    }
    void OnLoadArchiveMenu()
    {
        ChangeMenuStates(EscapeMenuState.Archive);
        ReadSavedFileData();
        fileName.text = null;
    }
    void OnExitGame()
    {
        GameManager.Instance.IsStopGame = false;
        Time.timeScale = 1;
        SceneController.Instance.LoadMenuScene();
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
        archiveMenu.SetActive(false);
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
            case EscapeMenuState.Archive:
                archiveMenu.SetActive(true);
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
