using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button newGameBtn;
    public Button continueBtn;
    public Button quitBtn;
    public Button backBtn;
    PlayableDirector director;

    [Header("Player Model")]
    public GameObject malePlayerPrefab;
    public GameObject femalePlayerPrefab;
    public GameObject dogPlayerPrefab;
    GameObject currentPlayer;
    int playerID;
    void Awake()
    {
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
        newGameBtn.onClick.AddListener(ChangePanel);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
        backBtn.onClick.AddListener(BackToStart);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    private void Update()
    {
        if (currentPlayer != null)
        {

        }
    }
    public void ShowSelectPlayer(string name)
    {
        currentPlayer = name switch
        {
            "MALE" => malePlayerPrefab,
            "FEMALE" => femalePlayerPrefab,
            "DOG" => dogPlayerPrefab,
            _ => null
        };
    }
    public void ChangePlayer(int id)
    {
        playerID = id;
        director.Play();
    }
    void BackToStart()
    {
        currentPlayer = null;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
    void ChangePanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    void NewGame(PlayableDirector pd)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadNewGameScene(playerID);
    }
    void ContinueGame()
    {
        SceneController.Instance.ContinueGame();
    }
    void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
