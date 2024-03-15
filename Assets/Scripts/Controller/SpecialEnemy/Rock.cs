using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        None
    }
    public RockStates rockStates;
    private Vector3 direction;
    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;
    public int damage;
    public GameObject target;
    public GameObject breakEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        FlyToTarget();
        rockStates = RockStates.HitPlayer;
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1)
            rockStates = RockStates.None;
    }
    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;

        direction = (target.transform.position - transform.position + (Vector3.up * 1.5f)).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (col.gameObject.CompareTag("Player"))
                {
                    col.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    col.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force * 0.9f;
                    col.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    col.gameObject.GetComponent<CharacterStates>().TakeDamage(damage, col.gameObject.GetComponent<CharacterStates>());

                    rockStates = RockStates.None;
                }
                break;
            case RockStates.HitEnemy:
                if (col.gameObject.GetComponent<Gloem>())
                {
                    col.gameObject.GetComponent<CharacterStates>().TakeDamage(damage, col.gameObject.GetComponent<CharacterStates>());
                    Instantiate(breakEffect,transform.position,Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}
