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
    string saveKey;
    public bool IsTrans
    {
        get; private set;
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
        deadWait = 2f;
        isdeadWait = false;
        saveKey = "AutoSave" + Random.Range(0, 1000).ToString();
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
        IsTrans = true;
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
    #region IEnumerator
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SaveManager.Instance.SaveGameData(saveKey);
            yield return StartCoroutine(fade.FadeOut(2));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(instantiatePlayerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadGameData(saveKey);
            SaveManager.Instance.DeleteGameData(saveKey);
            IsTrans = false;
            yield return StartCoroutine(fade.FadeIn(2));
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStates.gameObject;
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;
            IsTrans = false;
            yield return null;
        }
    }

    IEnumerator LoadScene(string scene, Vector3 position, Quaternion rotation)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene != null)
        {
            yield return StartCoroutine(fade.FadeOut(2));
            yield return SceneManager.LoadSceneAsync(scene);
            if (position == Vector3.zero)
            {
                position = GameManager.Instance.GetEntrance().position;
            }
            yield return player = Instantiate(instantiatePlayerPrefab, position, rotation);
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
    #endregion
    public void LoadMenuScene()
    {
        StartCoroutine(LoadMenu());
    }
    public void LoadNewGame(int id)
    {
        SelectPlayerMode(id);
        SaveManager.Instance.RigisterPlayerID(id);
        StartCoroutine(LoadScene("1_Forest", Vector3.zero, Quaternion.identity));
    }
    public void LoadGame(int id, Vector3 position, Quaternion rotation, string scene)
    {
        if (!IsTrans)
        {
            SelectPlayerMode(id);
            StartCoroutine(LoadScene(scene, position, rotation));
        }
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
