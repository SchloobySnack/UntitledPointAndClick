using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    // The target to follow
    public Transform target;

    // The distance from the target
    public float distance = 5.0f;

    // The height of the camera
    public float height = 3.0f;

    // The damping value for smooth following
    public float damping = 5.0f;

    // The default rotation of the camera
    public Vector3 rotation = new Vector3(45, 0, 0);

    void Update()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position - (rotation * distance);

        // Calculate the height
        desiredPosition.y += height;

        // Set the position of the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // Look at the target
        transform.LookAt(target, Vector3.up);
    }
}
