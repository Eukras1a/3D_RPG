using UnityEngine;
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
            saveName.text = "NAME:" + name;
            createTime.text = "TIME:" + time;
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
        FindObjectOfType<EscapeMenuUI>().RigisterFile(id);
    }
}
