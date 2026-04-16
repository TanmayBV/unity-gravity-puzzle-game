using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    public Rigidbody rb;
    private Animator anim;

    [Header("Movement")]
    public float speed = 6f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Gravity")]
    public float gravityStrength = 20f;
    private Vector3 gravityDirection = Vector3.down;
    public bool isGravityOn;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.6f;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Ground Settings")]
    public float coyoteTime = 0.15f; 
    public float maxSlopeAngle = 60f;

    [SerializeField]
    private bool isGrounded;
    private bool wasGrounded;

    private Vector3 groundNormal;

    private float lastGroundedTime;


    private bool canControl = true;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        isGravityOn = true;
    }

    void FixedUpdate()
    {
        if (!canControl) return;

        CheckGround();
        HandleMovement();
        ApplyGravity();
    }

    void Update()
    {
        AlignWithGravity();
    }

    public void EnableControl(bool value)
    {
        canControl = value;
    }

    // GROUND CHECK
    void CheckGround()
    {
        Vector3 origin = transform.position + transform.up * 0.4f; 

        RaycastHit hit;

        bool hitGround = Physics.SphereCast(
            origin,
            groundCheckRadius,
            -transform.up,
            out hit,
            groundCheckDistance,
            groundLayer
        );

        wasGrounded = isGrounded;

        if (hitGround)
        {
            float slopeAngle = Vector3.Angle(hit.normal, transform.up);

            if (slopeAngle <= maxSlopeAngle)
            {
                isGrounded = true;
                groundNormal = hit.normal;
                lastGroundedTime = Time.time;
            }
        }
        else
        {
            //prevents flicker
            if (Time.time - lastGroundedTime > coyoteTime)
            {
                isGrounded = false;
            }
        }

        //LAND
        if (!wasGrounded && isGrounded)
        {
            AlignToGravityStraight();
        }

        float verticalSpeed = Vector3.Dot(rb.linearVelocity, gravityDirection);

        bool isFallingNow = !isGrounded && verticalSpeed > 0.1f;

        anim.SetBool("isFalling", isFallingNow);
    }
    // MOVEMENT
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, gravityDirection).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, gravityDirection).normalized;

        Vector3 move = camForward * v + camRight * h;

        Vector3 targetVelocity = move * speed;

        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = targetVelocity - Vector3.ProjectOnPlane(velocity, gravityDirection);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        bool isMoving = move.magnitude > 0.1f;
        anim.SetBool("isRunning", isMoving && isGrounded);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, transform.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            //anim.SetTrigger("jump");
        }
    }

    // CUSTOM GRAVITY
    void ApplyGravity()
    {
        if(isGravityOn)
            rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    }

    //ALIGN PLAYER
    void AlignWithGravity()
    {
        if (!canControl) return; 

        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    public void AlignToGravityStraight()
    {
        Vector3 up = -gravityDirection;

        // Project forward onto plane to remove tilt
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;

        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.Cross(transform.right, up);

        Quaternion targetRotation = Quaternion.LookRotation(forward, up);

        transform.rotation = targetRotation;
    }


    //CUSTOM GRAVITY 
    public void SetGravity(Vector3 newGravityDirection)
    {
        gravityDirection = newGravityDirection.normalized;

    }

    // DEBUG
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + transform.up * .1f , groundCheckRadius);
    }

    //INTERACTION
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Points"))
        {
            other.gameObject.GetComponent<IInteractable>().Interact();
        }
    }
}
