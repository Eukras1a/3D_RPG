using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, ILocalizationController
{
    public Button newGameBtn;
    public Button loadBtn;
    public Button exitBtn;
    public Button backBtn;
    public Text m;
    public Text f;
    public Text d;

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
    #region 周期函数
    void Awake()
    {
        director = FindObjectOfType<PlayableDirector>();

        startMenu = transform.GetChild(0).gameObject;
        selectMenu = transform.GetChild(1).gameObject;
        startMenu.SetActive(true);
        selectMenu.SetActive(false);
        HideAllModel();
    }
    private void Start()
    {
        ChangeLanguage();
    }
    private void OnEnable()
    {
        director.stopped += NewGame;
        newGameBtn.onClick.AddListener(OnChangePanel);
        loadBtn.onClick.AddListener(OnLoadGame);
        exitBtn.onClick.AddListener(OnQuitGame);
        backBtn.onClick.AddListener(OnBack);
        LocalizationManager.Instance.AddLocalizationController(this);
    }
    private void Update()
    {
        if (currentPlayerModel != null)
        {
            currentPlayerModel.SetActive(true);
            currentPlayerModel.transform.Rotate(Vector3.up, 150 * Time.deltaTime);
        }
    }
    private void OnDisable()
    {
        LocalizationManager.Instance.RemoveLocalizationController(this);
        director.stopped -= NewGame;
        newGameBtn.onClick.RemoveListener(OnChangePanel);
        loadBtn.onClick.RemoveListener(OnLoadGame);
        exitBtn.onClick.RemoveListener(OnQuitGame);
        backBtn.onClick.RemoveListener(OnBack);
    }
    #endregion
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
    #region 响应事件
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
    public void ChangeLanguage()
    {
        m.text = LocalizationManager.Instance.GetLocalization("male");
        f.text = LocalizationManager.Instance.GetLocalization("female");
        d.text = LocalizationManager.Instance.GetLocalization("secret");
        backBtn.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("back");
        newGameBtn.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("start_game");
        loadBtn.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("load_archive");
        exitBtn.transform.GetChild(0).GetComponent<Text>().text = LocalizationManager.Instance.GetLocalization("exit_game");
    }
}
