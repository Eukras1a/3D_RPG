using UnityEngine;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour
{
    public Text saveName;
    public Text createTime;
    string id;
    public void SetUpDataInfo(string name, string time)
    {
        id = name;
        saveName.text = "NAME:" + name;
        createTime.text = "TIME:" + time;
    }
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnLoadGame);
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(OnLoadGame);
    }
    private void OnLoadGame()
    {
        //TODO:Ìæ»»ÒÑÓÐ´æµµ
        FindObjectOfType<EscapeMenuUI>().RigistFile(id);
    }
}
