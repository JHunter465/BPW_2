using UnityEngine;
using Cinemachine;

// Is dit de plek waar je vaak je naam bovenaan zet? In dat geval schrijf ik hier graag mijn naam boven
// als trotse maker van al deze classes :)
//Jael van Rossum
//HKU Building Playful Worlds 2
//nr. 3032611

//Dit script is met behulp van Brackeys FPS tutorial geschreven
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam = null;
    [SerializeField] private float yViewingRange = 85f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 flyForce = Vector3.zero;

    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    private Rigidbody rb = null;

    //set Rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a movement vector 
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Gets a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //Gets a rotational float for our camera
    public void RotateCamera(float _cameraRotation)
    {
        cameraRotationX = _cameraRotation;
    }

    //Gets a force vector for our Wings
    public void ApplyWings(Vector3 _wingsForce)
    {
        flyForce = _wingsForce;
    }

    //We're gonna apply our movement and rotation every physics tick
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Does the actual movement of the player rigidbody
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if(flyForce != Vector3.zero)
        {
            rb.AddForce(flyForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    //Does the actual rotation of the player and camera
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
        if(cam != null)
        {
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -yViewingRange, yViewingRange);
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }
    }
}
