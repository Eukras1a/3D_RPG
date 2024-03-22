using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    GameObject player;
    public GameObject dogPlayerPrefab;
    public GameObject malePlayerPrefab;
    public GameObject femalePlayerPrefab;
    public SceneFader sceneFaderPrefab;
    GameObject instantiatePlayerPrefab;
    bool fadeFinished;
    float deadWait;
    bool isdeadWait;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        instantiatePlayerPrefab = dogPlayerPrefab;
    }
    void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
        deadWait = 2f;
        isdeadWait = false;
    }
    void Update()
    {
        if (isdeadWait)
        {
            deadWait -= Time.deltaTime;
        }
    }
    void SelectPlayerMode(int id)
    {
        GameObject go = id switch
        {
            0 => malePlayerPrefab,
            1 => femalePlayerPrefab,
            2 => dogPlayerPrefab,
            _ => null
        };
        if (go != null)
        {
            instantiatePlayerPrefab = go;
        }
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.Same:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.Different:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        SaveManager.Instance.SavePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return StartCoroutine(fade.FadeOut(2));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(instantiatePlayerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            yield return StartCoroutine(fade.FadeIn(2));
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStates.gameObject;
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;
            yield return null;
        }
    }
    IEnumerator LoadScene_Teleport(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (scene != null)
        {
            yield return StartCoroutine(fade.FadeOut(2));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(instantiatePlayerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2));
            yield break;
        }
    }
    IEnumerator LoadScene_Document(string scene, Vector3 position, Quaternion rotation)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);

        if (scene != null)
        {
            yield return StartCoroutine(fade.FadeOut(2));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(instantiatePlayerPrefab, position, rotation);

            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2));
            yield break;
        }
    }

    IEnumerator LoadMenu()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2));
        yield return SceneManager.LoadSceneAsync("0_Menu");
        yield return StartCoroutine(fade.FadeIn(2));
        yield break;
    }
    public void LoadMenuScene()
    {
        StartCoroutine(LoadMenu());
    }
    public void LoadNewGame(int id)
    {
        SelectPlayerMode(id);
        SaveManager.Instance.RigisterPlayerID(id);
        StartCoroutine(LoadScene_Teleport("1_Forest"));
    }
    TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
            {
                return entrances[i];
            }
        }
        return null;
    }

    public void LoadGame(int id, Vector3 position, Quaternion rotation, string scene)
    {
        SelectPlayerMode(id);
        StartCoroutine(LoadScene_Document(scene, position, rotation));
    }

    public void EndNotify()
    {
        isdeadWait = true;
        if (fadeFinished)
        {
            if (deadWait < 0)
            {
                fadeFinished = false;
                StartCoroutine(LoadMenu());
            }
        }
    }
}
