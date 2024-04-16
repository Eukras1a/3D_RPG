using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour
{
    public Text saveName;
    public Text createTime;
    string id;
    public void SetUpDataInfo(string name, string time)
    {
        if (name != null)
        {
            id = name;
            saveName.text = name;
            createTime.text = time;
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
            FindObjectOfType<SecondaryMenu>().RigisterFile(id);
        }
    }
}
