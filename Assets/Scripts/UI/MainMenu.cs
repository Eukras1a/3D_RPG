using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    PlayableDirector director;
    void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;

        newGameBtn.onClick.AddListener(PlayTimeline);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
    }
    void PlayTimeline()
    {
        director.Play();

    }
    void NewGame(PlayableDirector pd)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadNewGameScene();
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
