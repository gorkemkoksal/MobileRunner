using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float forceDivideParameter = 75f;
    [SerializeField] float jumpSlideDeadZone = 2f;
    [SerializeField] float jumpDistance = 100f;
    [SerializeField] float crashImpact = 1f;

    float border = 1.2f;

    public bool isRunning;
    public bool IsRunning { get { return isRunning; } private set { } }
    public bool isJumping;
    public bool isSliding;

    public bool isCrashed;

    bool isntCrashed = true;
    bool isGrounded;

    Rigidbody playerRb;
    CapsuleCollider playerCCollider;
    Vector3 verticalForce;

    Touch touch;
    Vector3 touchStart;
    Vector3 touchEnd;
    void Awake()
    {
        if (playerRb == null)
            playerRb = GetComponent<Rigidbody>();
        if (playerCCollider == null)
            playerCCollider = GetComponent<CapsuleCollider>();
    }
    private void FixedUpdate()
    {
        RunningForward(isRunning);
        GroundCheck();
        Debug.Log(isntCrashed + " " + isRunning);
    }
    void Update()
    {
        TouchSetter(isntCrashed);
        
    }
    void GroundCheck()
    {
        float extraHeight = 0.01f;
        isGrounded = Physics.Raycast(playerCCollider.bounds.center, Vector3.down, playerCCollider.bounds.extents.y + extraHeight);
    }

    public bool firstTouch = true;
    bool prevState = false;
    void TouchSetter(bool isntCrashed)
    {
        if (!isntCrashed) return;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if (touch.phase == TouchPhase.Began && !firstTouch)
        {
            if (!prevState)
            {
                prevState = true;
                touchStart = touch.position;
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            prevState = false;
            touchEnd = touch.position;

            if (firstTouch)
            {
                touchStart = touchEnd;
                firstTouch = false;
            }
        }

        if (firstTouch) return;

        touchEnd = touch.position;



        //  touchStart = touch.position;

        VerticalMovement();
        JumpingOrSliding();
        touchEnd = touchStart;
        touchStart = touch.position;

    }
    void JumpingOrSliding()
    {
        var jumpSlideChecker = touchEnd.y - touchStart.y;

        //if(jumpSlideChecker!=0)
        //Debug.Log(jumpSlideChecker);

        if (jumpSlideChecker > jumpSlideDeadZone && isGrounded)
        {
            isJumping = true;
            playerRb.AddForce(Vector3.up * jumpDistance);
            isGrounded = false;
        }
        else if (-jumpSlideChecker > jumpSlideDeadZone && isGrounded)
        {
            isSliding = true;
            playerCCollider.height = 0.7f;
            playerCCollider.center = new Vector3(0, 0.35f, 0);
            //playerCCollider.height *= 0.5f;
            //playerCCollider.center.Set(0, playerCCollider.center.y * 0.5f, 0);
            //Debug.Log("kac defa calistin kim bilir");
            StartCoroutine(SlideWaiterForCollider());
        }
        jumpSlideChecker = 0;
    }
    IEnumerator SlideWaiterForCollider()
    {
        yield return new WaitForSeconds(1.15f);
        playerCCollider.height = 1.4f;
        playerCCollider.center = Vector3.up * 0.7f;
    }
    void RunningForward(bool isRunning)
    {
        if (!isntCrashed)
        {
            playerRb.AddForce(Vector3.back * crashImpact); //FallBack
        }

        if (!isRunning || !isntCrashed) return; //buraya DIKKAT
        var direction = Vector3.forward;
        playerRb.MovePosition(transform.position + (playerSpeed * Time.deltaTime * direction));
    }
    void VerticalMovement()
    {
        VerticalBorders(transform.position.x);
        var verticalForceStrength = (touchEnd.x - touchStart.x) / forceDivideParameter;
        verticalForce = new Vector3(verticalForceStrength, 0, 0);

        playerRb.AddForce(verticalForce, ForceMode.Impulse);
    }
    void VerticalBorders(float x)
    {
        if (x > border)
        {
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, playerRb.velocity.z);
            transform.position = new Vector3(border, transform.position.y, transform.position.z);
        }
        else if (x < -border)
        {
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, playerRb.velocity.z);
            transform.position = new Vector3(-border, transform.position.y, transform.position.z);
        }
        else return;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == 6) // 6 is Ground
        //{
        //    isGrounded = true;
        //}
        if (other.gameObject.layer == 7)
        {
            isCrashed = true;
            isntCrashed = false;
        }
    }
}
