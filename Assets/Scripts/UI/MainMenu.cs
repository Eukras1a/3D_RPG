using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SecondaryMenu secondaryMenu;
    [Header("Main Menu")]
    public Button newGameBtn;
    public Button loadBtn;
    public Button setBtn;
    public Button exitBtn;
    [Header("Secondary Menu")]
    public Text male;
    public Text female;
    public Text dog;
    public Button backBtn;

    [Header("Player Model")]
    public GameObject malePlayerModel;
    public GameObject femalePlayerModel;
    public GameObject dogPlayerModel;

    PlayableDirector director;
    GameObject currentPlayerModel;
    GameObject startMenu;
    GameObject selectMenu;
    int playerID;
    bool isSecondaryMenu;
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
    private void OnEnable()
    {
        director.stopped += NewGame;
        newGameBtn.onClick.AddListener(OnStartNewGame);
        loadBtn.onClick.AddListener(OnLoadGame);
        setBtn.onClick.AddListener(OnEnableSet);
        exitBtn.onClick.AddListener(OnQuitGame);
        backBtn.onClick.AddListener(OnBack);
    }
    private void Update()
    {
        if (currentPlayerModel != null)
        {
            currentPlayerModel.SetActive(true);
            currentPlayerModel.transform.Rotate(Vector3.up, 150 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSecondaryMenu)
            {
                OnBack();
            }
        }
    }
    private void OnDisable()
    {
        director.stopped -= NewGame;
        newGameBtn.onClick.RemoveListener(OnStartNewGame);
        loadBtn.onClick.RemoveListener(OnLoadGame);
        setBtn.onClick.RemoveListener(OnEnableSet);
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
    void OnEnableSet()
    {
        secondaryMenu.EnableSetPanel();
    }
    void OnBack()
    {
        currentPlayerModel = null;
        HideAllModel();
        startMenu.SetActive(true);
        selectMenu.SetActive(false);
        isSecondaryMenu = false;
    }
    void OnStartNewGame()
    {
        startMenu.SetActive(false);
        selectMenu.SetActive(true);
        isSecondaryMenu = true;
    }
    void OnLoadGame()
    {
        secondaryMenu.EnableArchivePanel();
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
