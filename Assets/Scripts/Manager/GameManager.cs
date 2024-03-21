using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public CharacterStates playerStates;
    [HideInInspector] public bool IsStopGame;

    CinemachineFreeLook followCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.LoadScene_Teleport("Menu");
    }*/
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RegisterPlayer(CharacterStates player)
    {
        playerStates = player;
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
}
