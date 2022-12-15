using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float sprintSpeed = 9f;
    public float forwardInput;
    public float strafeInput;
    public bool isSprinting;
    public Transform camera;

    void Update()
    {
        //get input values
        forwardInput = Input.GetAxis("Vertical");
        //make it so the player sprints when shift is pressed
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        strafeInput = Input.GetAxis("Horizontal");
    }


    void FixedUpdate()
    {
        //move the player
        //increase forward speed when sprinting
        if(isSprinting)
        {
            transform.position += transform.forward * forwardInput * sprintSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * forwardInput * speed * Time.deltaTime;
        }
        transform.position += transform.right * strafeInput * Time.deltaTime;

        Vector3 cameraForward = camera.forward;
        cameraForward.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraForward);
    }
}