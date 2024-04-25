using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject attackTarget;
    private CharacterStates characterStates;

    private float stopDistance;
    private float lastAttackTime;//攻击间隔
    bool isDead;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        stopDistance = agent.stoppingDistance;
    }
    void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RegisterPlayer(characterStates);
    }

    void Start()
    {
        SaveManager.Instance.SetPlayerData();
    }

    void OnDisable()
    {
        if (!MouseManager.IsInitialized)
            return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    void Update()
    {
        if (!SceneController.Instance.IsTrans)
        {
            isDead = characterStates.CurrentHealth == 0;
        }
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude / 15);
        animator.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopCoroutine(MoveToAttackTarget());
        if (isDead)
            return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }
    private void EventAttack(GameObject target)
    {
        if (isDead)
            return;
        if (target != null)
        {
            attackTarget = target;
            characterStates.isCritical = Random.value < characterStates.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStates.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        //TODO:攻击范围或许要加上怪物的碰撞判定半径(已加待修改
        while (Vector3.Distance(transform.position, attackTarget.transform.position) > characterStates.attackData.attackRange +
            (attackTarget.GetComponent<NavMeshAgent>() ? attackTarget.GetComponent<NavMeshAgent>().radius : 0))
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        //Attack
        agent.isStopped = true;
        if (lastAttackTime < 0)
        {
            animator.SetBool("Critical", characterStates.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = characterStates.attackData.coolDown;
        }
    }
    //Animition Event
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.None)
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }

    }
}
