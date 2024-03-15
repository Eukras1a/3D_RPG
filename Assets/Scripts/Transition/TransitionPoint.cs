using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        Same,
        Different
    }
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canTrans)
        {
            //TODO:´«ËÍ
            Debug.Log("Transition!");
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            canTrans = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
