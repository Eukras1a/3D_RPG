using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour, ILocalizationController
{
    public Text saveName;
    public Text createTime;
    string nameStr;
    string timeStr;
    string id;
    public void SetUpDataInfo(string name, string time)
    {
        ChangeLanguage();
        if (name != null)
        {
            id = name;
            saveName.text = nameStr + "£º" + name;
            createTime.text = timeStr + "£º" + time;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnRigisterFileData);
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(OnRigisterFileData);
    }
    private void OnRigisterFileData()
    {
        if (SceneManager.GetActiveScene().name != "0_Menu")
        {
            FindObjectOfType<MenuUI>().RigisterFile(id);
        }
        else
        {
            FindObjectOfType<MainMenuArchive>().RigisterFile(id);
        }
    }

    public void ChangeLanguage()
    {
        nameStr = LocalizationManager.Instance.GetLocalization("save_name");
        timeStr = LocalizationManager.Instance.GetLocalization("save_time");
    }
}
