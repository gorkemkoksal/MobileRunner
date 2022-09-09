using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnimController : MonoBehaviour
{
    Animator playerAnimator;
    PlayerMovement playerMovement;
    GameManager gameManager;
    Touch touch;

    int isStartedHash;
    int isJumpedHash;
    int isLevelEndedHash;

    bool isEnded;
    bool isCrashDead;
 //   int isSlidedHash;

    void Awake()
    {      
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

        if (playerAnimator == null)
            playerAnimator = GetComponent<Animator>();

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        isStartedHash = Animator.StringToHash("isStarted");
        isJumpedHash = Animator.StringToHash("isJumped");
        isLevelEndedHash = Animator.StringToHash("levelEnded");
       // isSlidedHash = Animator.StringToHash("isSlided"); $$=SLIDE
    }
    void Update()
    {
        IsLevelEnded();
        IsLevelStarted();
        IsJump(playerMovement.isJumping); //bu tarz bir layout nasil gelistirilebilir
        IsSlide(playerMovement.isSliding);
        IsCrashed(playerMovement.isCrashed); //$$=FallBack
    }
    void IsCrashed(bool crashInfo)
    {
        if (crashInfo && !isCrashDead)
        {
            playerAnimator.SetTrigger("isCrashed");   //crash
                                                      //    playerMovement.isCrashed = false;
            isCrashDead = true;
        }
    }
    void IsLevelEnded()
    {
        isEnded = playerAnimator.GetBool(isLevelEndedHash);
        if (isEnded) return;

        if (gameManager.isLevelEnded)
        {
            playerAnimator.SetBool(isStartedHash, false);
            playerAnimator.SetBool(isLevelEndedHash, true);
        }

        isEnded = playerAnimator.GetBool(isLevelEndedHash);

        //if(gameManager.isLevelEnded && gameManager.isStartedAgain)
        //{
        //    playerAnimator.SetBool(isStartedHash, true);  /////////bekle
        //    playerAnimator.SetBool(isLevelEndedHash, false);
        //}
        //else if (gameManager.isLevelEnded)
        //{
        //    playerAnimator.SetBool(isStartedHash, false);
        //    playerAnimator.SetBool(isLevelEndedHash, true);
        //}
    }
    void IsJump(bool jumpInfo)
    {
        if (jumpInfo)
        {
            //playerAnimator.SetTrigger(isJumpedHash);
            //playerMovement.isJumping = false;

            playerAnimator.Play("Running Jump");
            playerMovement.isJumping = false;
        }
    }
    void IsSlide(bool slideInfo) 
    {
        if (slideInfo)
        {
            playerAnimator.Play("Running Slide");
            playerMovement.isSliding = false;
        }
    }
    public void IsLevelStarted()
    {
        bool isStarted = playerAnimator.GetBool(isStartedHash);
        if (isStarted || isEnded) return;

        if(Input.touchCount>0)
            touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended)
        {
            playerAnimator.SetBool(isStartedHash, true);
            playerMovement.isRunning = true;
        }
    }
}
