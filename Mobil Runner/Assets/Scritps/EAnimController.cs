using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EAnimController : MonoBehaviour
{
    Animator enemyAnimator;
    EnemyMovement enemyMovement;
    NavMeshAgent navMeshAgent;

    int isFireHash;
    
    void Awake()
    {
        if (enemyAnimator == null)
            enemyAnimator = GetComponent<Animator>();
        isFireHash = Animator.StringToHash("isFire");
        if (enemyMovement == null)
            enemyMovement = GetComponent<EnemyMovement>();
        if (navMeshAgent == null && gameObject.tag == "Runner")
            navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {
     //   IsFire(enemyMovement.isFiring);

    //    IsFollowing(enemyMovement.isFollowing);
    //    IsHitting(navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance); $$=Zombie
    }
    public void IsFire(bool isFiring)
    {

        if (!isFiring) return;
        enemyAnimator.SetTrigger(isFireHash);
    }
    //void IsFollowing(bool isRunning)
    //{
    //    if (isRunning)
    //    {
    //       // enemyMovement.isFollowing = false;
    //        enemyAnimator.SetBool("isFollowing", true);            $$=Zombie
    //    }
    //}
    //void IsHitting(bool isHitting)
    //{
    //    if (isHitting)
    //    {
    //        enemyAnimator.SetTrigger("isStriking");
    //    }
    //}
}
