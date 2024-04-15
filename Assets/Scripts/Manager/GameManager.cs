using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public CharacterStates playerStates;
    [HideInInspector] public bool IsStopGame;

    CinemachineFreeLook followCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    List<string> customWindow = new List<string>() {
        "1920x1080","1600x900","1280x768","640 x480"
    };
    public bool IsPlayerInitialized
    {
        get; private set;
    }
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.LoadScene_Teleport("Menu");
    }*/
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        IsPlayerInitialized = false;
    }
    public void SetPlayerData(CharacterData_SO data)
    {
        playerStates.characterData = data;
    }
    public void RegisterPlayer(CharacterStates player)
    {
        playerStates = player;
        IsPlayerInitialized = true;
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        followCamera.Follow = playerStates.transform.GetChild(0).transform;
        followCamera.LookAt = playerStates.transform.GetChild(0).transform;
    }
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }
    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
    public Transform GetEntrance()
    {
        foreach (var entrance in FindObjectsOfType<TransitionDestination>())
        {
            if (entrance.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                return entrance.transform;
            }
        }
        return null;
    }
    public List<string> GetWindow()
    {
        return customWindow;
    }
    public void SetCustomWindow(string resolution, bool fullScreen)
    {
        string[] resTemp = resolution.Split('x');
        int width = int.Parse(resTemp[0]);
        int height = int.Parse(resTemp[1]);
        Debug.Log($"{width}+{height}+{fullScreen}");
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
    }
}
