using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 10f;
    public float rotationSpeed = 3f;

    [Header("Collision")]
    public float collisionRadius = 0.3f;
    public LayerMask collisionLayer;

    private float mouseX;
    private float mouseY;
    private float currentDistance;

    void Start()
    {
        currentDistance = distance;
        mouseX = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        HandleRotation();
        HandleCamera();
    }

    //ROTATION
    void HandleRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed * 100f * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed * 100f * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -40f, 80f);
    }

    //CAMERA MOVEMENT
    void HandleCamera()
    {
        // Align with player gravity
        Quaternion gravityRotation = Quaternion.FromToRotation(Vector3.up, player.up);

        Quaternion rotation = gravityRotation * Quaternion.Euler(mouseY, mouseX, 0);

        Vector3 direction = rotation * Vector3.back;
        Vector3 targetPosition = player.position + player.up * height;

        float targetDistance = distance;

        // Collision
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, collisionRadius, direction, out hit, distance, collisionLayer))
        {
            targetDistance = hit.distance - 0.2f;
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * 10f);

        Vector3 finalPosition = targetPosition + direction * currentDistance;

        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(targetPosition, player.up);
    }
}
