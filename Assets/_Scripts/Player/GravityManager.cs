using UnityEngine;
using System.Collections;

public class GravityManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    public Transform hologram;

    [Header("Settings")]
    public float previewHeight = 1.5f;
    public float moveSpeed = 5f;

    private Vector3 selectedDirection;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    void Update()
    {
        HandleInput(); 
    }

    //HANDLE HOLLOW 
    void HandleInput()
    {
        Vector3 up = player.transform.up;

        //CAMERA RELATIVE DIRECTIONS
        Vector3 camForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(Camera.main.transform.right, up).normalized;

        // SNAP TO 4 DIRECTIONS
        camForward = GetSnappedDirection(camForward);
        camRight = GetSnappedDirection(camRight);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            selectedDirection = camForward;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            selectedDirection = -camForward;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            selectedDirection = -camRight;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            selectedDirection = camRight;

        if (selectedDirection != Vector3.zero)
        {
            ShowPreview();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ApplyGravitySmooth());
        }
    }

    Vector3 GetSnappedDirection(Vector3 dir)
    {
        dir.Normalize();

        float x = Mathf.Round(dir.x);
        float y = Mathf.Round(dir.y);
        float z = Mathf.Round(dir.z);

        return new Vector3(x, y, z).normalized;
    }



    // HOLOGRAM PREVIEW
    void ShowPreview()
    {
        hologram.gameObject.SetActive(true);

        targetPosition =  player.transform.position + player.transform.up * previewHeight;

        Vector3 newUp = -selectedDirection;

        Vector3 forward = Vector3.ProjectOnPlane(player.transform.forward, newUp).normalized;

        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.Cross(player.transform.right, newUp);

        targetRotation = Quaternion.LookRotation(forward, newUp);


        hologram.position = targetPosition;
        hologram.rotation = targetRotation;
    }

    // SMOOTH TRANSITION
    IEnumerator ApplyGravitySmooth()
    {
        if (selectedDirection == Vector3.zero)
            yield break;

        player.EnableControl(false);
        player.isGravityOn = false;

        player.rb.linearVelocity = Vector3.zero;
        player.rb.isKinematic = true;

        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;

        Vector3 liftPos = startPos + player.transform.up * 1.0f;
        Quaternion targetRot = hologram.rotation;
        Vector3 targetPos = hologram.position;

        yield return new WaitForSeconds(.3f);

        float t;

        //LIFT
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            player.transform.position = Vector3.Lerp(startPos, liftPos, smoothT);

            yield return null;
        }

        //ROTATE
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            player.transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);

            yield return null;
        }

        // MOVE
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            player.transform.position = Vector3.Lerp(liftPos, targetPos, smoothT);

            yield return null;
        }

        //APPLY GRAVITY
        player.SetGravity(selectedDirection);
        player.AlignToGravityStraight();

        player.rb.isKinematic = false;
        player.isGravityOn = true;
        player.EnableControl(true);

        hologram.gameObject.SetActive(false);
        selectedDirection = Vector3.zero;
        
    }



}
