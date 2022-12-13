
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // target to focus on
    public Transform target;

    // speed of camera movement
    public float speed = 5f;

    // camera distance from target
    public float distance = 5f;

    // maximum angle for camera movement
    public float maxAngle = 180f;

    // the starting height of the camera
    public float height = 2f;

    // speed of camera zoom
    public float zoomSpeed = 0.5f;

    // speed of camera rotation
    public float rotateSpeed = 0.5f;

    // camera rotation
    private float yaw = 0f;
    private float pitch = 0f;

    // hide the cursor
    void Start()
    {
        // Releases the cursor
        Cursor.lockState = CursorLockMode.None;
        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Confines the cursor
        Cursor.lockState = CursorLockMode.Confined;
    }

    // update camera transform
    void Update()
    {
        // get mouse input
        yaw += Input.GetAxis("Mouse X") * rotateSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;

        // limit pitch angle
        pitch = Mathf.Clamp(pitch, -maxAngle, maxAngle);

        // rotate camera around target
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // zoom the camera
        distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // limit minimum distance
        distance = Mathf.Clamp(distance, 0.5f, 5f);

        // move camera to target
        Vector3 position = target.position - transform.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}