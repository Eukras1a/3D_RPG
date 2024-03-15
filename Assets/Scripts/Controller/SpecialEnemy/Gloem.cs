using UnityEngine;
using UnityEngine.AI;

public class Gloem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 30f;

    public GameObject rockPrefab;
    public Transform handpos;
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            targetStates.TakeDamage(characterStates, targetStates);
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }

    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handpos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
