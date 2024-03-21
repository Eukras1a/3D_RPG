using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuArchive : MonoBehaviour
{
    public Button loadButton;
    public Button deleteButton;
    GameObject archiveMenu;
    public SaveFileButton SaveButtonPrefab;
    public RectTransform dataListTransform;
    string currentSelectFile;

    bool isOpen;
    GameObject archivePanel;
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
    }
    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(OnLoad);
        deleteButton.onClick.RemoveListener(OnDelete);
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
            SaveManager.Instance.DeleteData(currentSelectFile);
            ReadSavedFileData();
        }
    }
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
}
