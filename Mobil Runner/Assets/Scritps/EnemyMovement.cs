using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemyAttentionRadius = 10f; //sil beni local yap,farkli dusmanlar icin kalabilir
    [SerializeField] float aimX, aimY, aimZ = 0;

    public bool isFollowing; // for runners
    public bool isStriking; // for runners

  //  public bool isFiring; //for shooters
    public bool startFiring;

    float targetDistance;
    Transform targetPlayer;
    NavMeshAgent navMeshAgent;
    SniperFire sniperFire;

   // Rigidbody enemyRb;
    private void Awake()
    {
        if (navMeshAgent == null && gameObject.tag == "Runner")
            navMeshAgent = GetComponent<NavMeshAgent>();                     //$$=Runners kullanmazsan kaldir
        if (targetPlayer == null)
            targetPlayer = FindObjectOfType<PlayerMovement>().transform;
        if (sniperFire == null)
            sniperFire = GetComponentInChildren<SniperFire>();
    }

    void Update()
    {
        Fire();
      //  HitPlayer();
    }
   void Fire()
    {
        targetDistance = Vector3.Distance(targetPlayer.position, transform.position);
        if (targetDistance < enemyAttentionRadius && !sniperFire.isShoot)   // buraya dikkat
        {
            transform.LookAt(targetPlayer,new Vector3(aimX,aimY,aimZ));
            
        //    transform.LookAt(targetPlayer);
            startFiring =true;
       //     isFiring = true;
        }
    }
}
