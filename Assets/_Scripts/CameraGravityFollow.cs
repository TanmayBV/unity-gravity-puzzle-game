using UnityEngine;

public class CameraGravityFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
    }
}
