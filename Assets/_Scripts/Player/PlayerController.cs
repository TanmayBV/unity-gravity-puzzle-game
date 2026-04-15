using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator anim;

    [Header("Movement")]
    public float speed = 6f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Gravity")]
    public float gravityStrength = 20f;
    private Vector3 gravityDirection = Vector3.down;
    public bool isGravityOn = true;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.6f;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Ground Settings")]
    public float coyoteTime = 0.15f; // small delay before falling
    public float maxSlopeAngle = 60f;

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

    // ✅ GROUND CHECK
    void CheckGround()
    {
        Vector3 origin = transform.position + transform.up * 0.1f;

        RaycastHit hit;
        bool hitGround = Physics.SphereCast(
            origin,
            groundCheckRadius,
            gravityDirection,
            out hit,
            groundCheckDistance,
            groundLayer
        );

        wasGrounded = isGrounded;

        if (hitGround)
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
        }

        // 🔥 LANDING DETECTED
        if (!wasGrounded && isGrounded)
        {
            AlignToGroundInstant();
        }

        HandleGroundAnimation();
    }


    void HandleGroundAnimation()
    {
        bool isFalling = !isGrounded && rb.linearVelocity.magnitude > 0.5f;

        anim.SetBool("isFalling", isFalling);

        // Optional: landing trigger
        if (isGrounded && rb.linearVelocity.magnitude < 0.2f)
        {
            anim.SetTrigger("land");
        }
    }

    void AlignToGroundInstant()
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

        transform.rotation = targetRotation;
    }


    // 🎮 MOVEMENT
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
        anim.SetBool("isRunning", isGrounded);

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
            anim.SetTrigger("jump");
        }
    }

    // 🧲 CUSTOM GRAVITY
    void ApplyGravity()
    {
        if(isGravityOn)
        rb.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
    }

    // 🔄 ALIGN PLAYER
    void AlignWithGravity()
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    // 🌍 EXTERNAL CALL
    public void SetGravity(Vector3 newGravityDirection)
    {
        gravityDirection = newGravityDirection.normalized;

        rb.linearVelocity = Vector3.zero; // for Rigidbody
    }

    // DEBUG
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + gravityDirection * 2f);
    }
}
