using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button newGameBtn;
    public Button loadBtn;
    public Button quitBtn;
    public Button backBtn;

    public MainMenuArchive archive;
    PlayableDirector director;

    [Header("Player Model")]
    public GameObject malePlayerModel;
    public GameObject femalePlayerModel;
    public GameObject dogPlayerModel;
    GameObject currentPlayerModel;

    GameObject startMenu;
    GameObject selectMenu;
    int playerID;
    void Awake()
    {
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
        newGameBtn.onClick.AddListener(OnChangePanel);
        loadBtn.onClick.AddListener(OnLoadGame);
        quitBtn.onClick.AddListener(OnQuitGame);
        backBtn.onClick.AddListener(OnBack);
        startMenu = transform.GetChild(0).gameObject;
        selectMenu = transform.GetChild(1).gameObject;
        startMenu.SetActive(true);
        selectMenu.SetActive(false);
        HideAllModel();
    }
    private void Update()
    {
        if (currentPlayerModel != null)
        {
            currentPlayerModel.SetActive(true);
            currentPlayerModel.transform.Rotate(Vector3.up, 150 * Time.deltaTime);
        }
    }
    void HideAllModel()
    {
        malePlayerModel.SetActive(false);
        femalePlayerModel.SetActive(false);
        dogPlayerModel.SetActive(false);
    }
    public void ShowSelectPlayer(string name)
    {
        currentPlayerModel = name switch
        {
            "MALE" => malePlayerModel,
            "FEMALE" => femalePlayerModel,
            "DOG" => dogPlayerModel,
            _ => null
        };
        HideAllModel();
    }
    public void ChangePlayer(int id)
    {
        playerID = id;
        director.Play();
    }
    #region ÏìÓ¦ÊÂ¼þ
    void NewGame(PlayableDirector pd)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadNewGame(playerID);
    }
    void OnBack()
    {
        currentPlayerModel = null;
        startMenu.SetActive(true);
        selectMenu.SetActive(false);
    }
    void OnChangePanel()
    {
        startMenu.SetActive(false);
        selectMenu.SetActive(true);
    }
    void OnLoadGame()
    {
        archive.EnableArchivePanel();
    }
    void OnQuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion
}
