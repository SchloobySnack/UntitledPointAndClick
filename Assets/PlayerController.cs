using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardInput;
    public float strafeInput;
    public Transform camera;
    public float gravity = 9.81f;
    public float verticalVelocity;
    public float jumpForce = 10f;

    private CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        //get input values
        forwardInput = Input.GetAxis("Vertical");
        strafeInput = Input.GetAxis("Horizontal");
    }


    void FixedUpdate()
    {
        //move the player
        transform.position += transform.forward * forwardInput * Time.deltaTime;
        transform.position += transform.right * strafeInput * Time.deltaTime;

        Vector3 cameraForward = camera.forward;
        cameraForward.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}