using UnityEngine;
using UnityEngine.UI;

public class MainMenuArchive : MonoBehaviour, ILocalizationController
{
    public Button loadButton;
    public Button deleteButton;
    public SaveFileButton SaveButtonPrefab;
    public RectTransform dataListTransform;

    string currentSelectFile;
    bool isOpen;
    GameObject archivePanel;

    public Text createBtn;
    #region 周期函数
    private void Awake()
    {
        archivePanel = transform.GetChild(0).gameObject;
        isOpen = false;
        archivePanel.SetActive(false);
    }
    private void OnEnable()
    {
        loadButton.onClick.AddListener(OnLoad);
        deleteButton.onClick.AddListener(OnDelete);
        LocalizationManager.Instance.AddLocalizationController(this);
    }
    private void Start()
    {
        ChangeLanguage();
    }
    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableArchivePanel();
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
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
    }
    #endregion
    #region 响应事件
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
        isOpen = true;
        ReadSavedFileData();
    }
    public void RigisterFile(string id)
    {
        currentSelectFile = id;
    }
    public void DisableArchivePanel()
    {
        archivePanel.SetActive(false);
        isOpen = false;
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
        createBtn.text = LocalizationManager.Instance.GetLocalization("create");
        loadButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("load");
        deleteButton.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("delete");
    }
}
