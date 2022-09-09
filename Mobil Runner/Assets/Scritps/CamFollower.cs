using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    [SerializeField] float followDistance = 2f;
    [SerializeField] GameObject target;

    PlayerMovement playerMovement;
    GameManager gameManager;
    private void Start()
    {
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (playerMovement.isCrashed || gameManager.isLevelEnded) return;
        transform.position=new Vector3(transform.position.x, transform.position.y,target.transform.position.z - followDistance);
    }
}
