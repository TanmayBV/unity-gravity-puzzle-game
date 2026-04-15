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

    void HandleInput()
    {
        Transform t = player.transform;

        // 🔥 Flatten directions relative to surface
        Vector3 forward = Vector3.ProjectOnPlane(t.forward, player.transform.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(t.right, player.transform.up).normalized;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            selectedDirection = forward;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            selectedDirection = -forward;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            selectedDirection = -right;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            selectedDirection = right;

        if (selectedDirection != Vector3.zero)
        {
            ShowPreview();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ApplyGravitySmooth());
        }
    }


    // 🔥 HOLOGRAM PREVIEW
    void ShowPreview()
    {
        hologram.gameObject.SetActive(true);

        // 👉 slightly above player
        targetPosition =  player.transform.position + player.transform.up * previewHeight;

        // 👉 calculate final rotation
        targetRotation =
            Quaternion.FromToRotation(player.transform.up, -selectedDirection) *
            player.transform.rotation;

        hologram.position = targetPosition;
        hologram.rotation = targetRotation;
    }

    // 🔥 SMOOTH TRANSITION
    IEnumerator ApplyGravitySmooth()
    {
        player.isGravityOn = false;
        if (selectedDirection == Vector3.zero)
            yield break;

        player.EnableControl(false);

        float t = 0f;
        float speed = 5f;

        Vector3 startPos = player.transform.position;
        Quaternion startRot = player.transform.rotation;

        // 🔥 TAKE TARGET FROM HOLOGRAM
        Vector3 targetPos = hologram.position;
        Quaternion targetRot = hologram.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            player.transform.position = Vector3.Lerp(startPos, targetPos, t);
            player.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // 🔥 Apply gravity AFTER reaching hologram
        player.SetGravity(selectedDirection);

        player.EnableControl(true);

        hologram.gameObject.SetActive(false);
        selectedDirection = Vector3.zero;

        player.isGravityOn = true;
    }

}
