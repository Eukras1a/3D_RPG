using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryMenu : MonoBehaviour
{
    [Header("Archive Panel")]
    public Text createBtn;
    public Button loadButton;
    public Button deleteButton;
    public SaveFileButton SaveButtonPrefab;
    public RectTransform dataListTransform;
    [Header("Setting Panel")]
    public Dropdown windowDropdown;
    public Toggle fullScreen;
    public Text fullScreenLabel;

    string currentSelectFile;
    bool isOpenArchivePanel;
    bool isOpenSetPanel;
    GameObject archivePanel;
    GameObject setPanel;
    #region 周期函数
    private void Awake()
    {
        archivePanel = transform.GetChild(0).gameObject;
        setPanel = transform.GetChild(1).gameObject;
        isOpenArchivePanel = false;
        isOpenSetPanel = false;
        archivePanel.SetActive(false);
        setPanel.SetActive(false);
    }
    private void OnEnable()
    {
        loadButton.onClick.AddListener(OnLoad);
        deleteButton.onClick.AddListener(OnDelete);
        fullScreen.onValueChanged.AddListener(OnFullScreenChanged);
        windowDropdown.onValueChanged.AddListener(OnWindowValueChanged);
    }
    private void Start()
    {
        LoadCustomWindow();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpenArchivePanel)
            {
                DisableArchivePanel();
            }
            if (isOpenSetPanel)
            {
                DisableSetPanel();
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
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
        fullScreen.onValueChanged.RemoveListener(OnFullScreenChanged);
        windowDropdown.onValueChanged.RemoveListener(OnWindowValueChanged);
    }
    #endregion
    #region 响应事件
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
        }
    }
    void OnDelete()
    {
        if (currentSelectFile != null)
        {
            SaveManager.Instance.DeleteGameData(currentSelectFile);
            currentSelectFile = null;
            ReadSavedFileData();
        }
    }
    #endregion
    public void EnableArchivePanel()
    {
        archivePanel.SetActive(true);
        isOpenArchivePanel = true;
        ReadSavedFileData();
    }
    public void EnableSetPanel()
    {
        setPanel.SetActive(true);
        isOpenSetPanel = true;
    }
    public void RigisterFile(string id)
    {
        currentSelectFile = id;
    }
    public void DisableArchivePanel()
    {
        archivePanel.SetActive(false);
        isOpenArchivePanel = false;
    }
    public void DisableSetPanel()
    {
        setPanel.SetActive(false);
        isOpenSetPanel = false;
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
}
