using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour, ILocalizationController
{
    GameObject mainMenu;
    GameObject archiveMenu;
    GameObject settingMenu;
    GameObject confirmMenu;
    public SaveFileButton SaveButtonPrefab;
    [Header("Main Panel")]
    public Button archiveButton;
    public Button settingButton;
    public Button exitButton;
    [Header("Archive Panel")]
    public Button createButton;
    public Button loadButton;
    public Button deleteButton;
    [Header("Setting Panel")]
    public Dropdown languageDropdown;
    public Dropdown windowDropdown;
    public Toggle fullScreen;
    public Text fullScreenLabel;
    [Header("Confirm Panel")]
    public InputField fileName;
    public Button confirmButton;
    public Button cancelButton;
    public RectTransform dataListTransform;

    string currentSelectFile;
    enum EscapeMenuState
    {
        None,
        Main,
        Archive,
        Setting,
        Confirm,
    }
    EscapeMenuState menu = EscapeMenuState.None;
    #region 周期函数
    private void Awake()
    {
        mainMenu = transform.GetChild(0).gameObject;
        archiveMenu = transform.GetChild(1).gameObject;
        confirmMenu = transform.GetChild(2).gameObject;
        settingMenu = transform.GetChild(3).gameObject;
        ChangeMenuStates(EscapeMenuState.None);
    }
    private void Start()
    {
        LoadLanguage();
        LoadCustomWindow();
        ChangeLanguage();
    }
    private void OnEnable()
    {
        LocalizationManager.Instance.AddLocalizationController(this);       
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        archiveButton.onClick.AddListener(OnOpenArchiveMenu);
        settingButton.onClick.AddListener(OnOpenSettingMenu);
        fullScreen.onValueChanged.AddListener(OnFullScreenChanged);
        windowDropdown.onValueChanged.AddListener(OnWindowValueChanged);
        languageDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
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
        LocalizationManager.Instance.RemoveLocalizationController(this);
        confirmButton.onClick.RemoveListener(OnConfirm);
        cancelButton.onClick.RemoveListener(OnCancel);
        archiveButton.onClick.RemoveListener(OnOpenArchiveMenu);
        settingButton.onClick.RemoveListener(OnOpenSettingMenu);
        fullScreen.onValueChanged.RemoveListener(OnFullScreenChanged);
        windowDropdown.onValueChanged.RemoveListener(OnWindowValueChanged);
        languageDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        createButton.onClick.RemoveListener(OnCreate);
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
        exitButton.onClick.RemoveListener(OnExitGame);
    }
    #endregion
    #region 响应事件
    void OnOpenSettingMenu()
    {
        ChangeMenuStates(EscapeMenuState.Setting);
    }
    void OnDropdownValueChanged(int index)
    {
        LocalizationManager.Instance.SetLanguageState(LocalizationManager.Instance.GetLanguageState(languageDropdown.options[index].text));
    }
    void OnFullScreenChanged(bool isOn)
    {
        GameManager.Instance.SetCustomWindow(windowDropdown.options[windowDropdown.value].text, isOn);
    }
    void OnWindowValueChanged(int index)
    {
        GameManager.Instance.SetCustomWindow(windowDropdown.options[index].text, fullScreen.isOn);
    }
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
        OnOpenArchiveMenu();
    }
    void OnCancel()
    {
        ChangeMenuStates(EscapeMenuState.Archive);
    }
    void OnOpenArchiveMenu()
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
    void LoadLanguage()
    {
        languageDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (var item in LocalizationManager.Instance.GetLanguageOptions())
        {
            options.Add(new Dropdown.OptionData(item.Key));
        }
        languageDropdown.options = options;
    }
    void LoadCustomWindow()
    {
        windowDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (var item in GameManager.Instance.GetWindow())
        {
            options.Add(new Dropdown.OptionData(item));
        }
        windowDropdown.options = options;
    }
    void ChangeMenuStates(EscapeMenuState state)
    {
        menu = state;
        mainMenu.SetActive(false);
        archiveMenu.SetActive(false);
        settingMenu.SetActive(false);
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
            case EscapeMenuState.Setting:
                settingMenu.SetActive(true);
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
    public void ChangeLanguage()
    {
        fullScreenLabel.text = LocalizationManager.Instance.GetLocalization("full_screen");
        archiveButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("archive");
        settingButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("set");
        exitButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("exit_game");
        createButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("create");
        loadButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("load");
        deleteButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("delete");
        confirmButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("confirm");
        cancelButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("cancel");
        fileName.placeholder.GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("get_filename");
    }
}
