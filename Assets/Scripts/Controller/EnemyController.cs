using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStates))]

public class EnemyController : MonoBehaviour, IEndGameObserver
{
    protected GameObject attackTarget;
    private Animator anim;
    protected CharacterStates characterStates;
    private NavMeshAgent agent;
    private Collider col;
    private EnemyStates enemyStates;

    [Header("Settings")]
    public float sightRadius;
    public bool isGuard;
    public float patrolRange;
    public float lookAtTime;

    private float speed;
    private float lastAttackTime;
    private float remainLookAtTime;
    private Quaternion guardRotation;

    private Vector3 wayPoint;
    private Vector3 guardPos;

    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        characterStates = GetComponent<CharacterStates>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
        speed = agent.speed;
        guardRotation = transform.rotation;
    }
    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //TODO:加载场景时修改
        GameManager.Instance.AddObserver(this);
    }

    /*    void OnEnable() 
        { 
            GameManager.Instance.AddObserver(this);
        }*/
    void OnDisable()
    {
        if (!GameManager.IsInitialized)
            return;
        GameManager.Instance.RemoveObserver(this);
        if (GetComponent<LootSpawner>() && isDead)
        {
            GetComponent<LootSpawner>().Spawnloot();
        }
        if (TaskManager.IsInitialized && isDead)
        {
            TaskManager.Instance.UpdateTaskProgress(characterStates.characterData.characterName,1);
        }
    }
    void Update()
    {
        if (characterStates.CurrentHealth == 0)
        {
            isDead = true;
        }
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }
    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStates.isCritical);
        anim.SetBool("Death", isDead);
    }
    void SwitchStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (FoundPlayer())
        {
            remainLookAtTime = lookAtTime;
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.SqrMagnitude(transform.position - guardPos) <= agent.stoppingDistance)//考虑SqrMagnitude与Distance的性能开销
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL://巡逻
                isChase = false;
                agent.speed = speed * 0.5f;
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE://追击
                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (FoundPlayer())
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                else
                {
                    isFollow = false;

                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }

                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                        remainLookAtTime = lookAtTime;
                    }
                }
                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStates.attackData.coolDown;
                        characterStates.isCritical = Random.value < characterStates.attackData.criticalChance;
                        Attack();
                    }
                }
                break;
            case EnemyStates.DEAD:
                col.enabled = false;
                agent.radius = 0;
                Destroy(gameObject, 2f);
                break;
        }
    }
    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (TargetInAttackRange())
        {
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            anim.SetTrigger("Skill");
        }
    }
    bool FoundPlayer()
    {
        var collider = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in collider)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(randomX + guardPos.x, transform.position.y, randomZ + guardPos.z);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.attackData.skillRange;
        }
        else
            return false;
    }
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStates.attackData.attackRange;
        }
        else
            return false;
    }
    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }
    public void EndNotify()
    {
        playerDead = true;
        anim.SetBool("Win", true);
        isWalk = false;
        isChase = false;
        isFollow = false;
        attackTarget = null;
    }
}
