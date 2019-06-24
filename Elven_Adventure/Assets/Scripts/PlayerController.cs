using UnityEngine;

// Is dit de plek waar je vaak je naam bovenaan zet? In dat geval schrijf ik hier graag mijn naam boven
// als trotse maker van al deze classes :)
//Jael van Rossum
//HKU Building Playful Worlds 2
//nr. 3032611

//Dit script is met behulp van Brackeys FPS tutorial geschreven https://www.youtube.com/watch?v=UK57qdq_lak&list=PLPV2KyIb3jR5PhGqsO7G4PsbEC_Al-kPZ&index=1

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{

    [Header("Move and Rotate")]
    [Tooltip("Movement Speed of the Player")]
    [SerializeField] private float movingSpeed = 5f;
    [Tooltip("Sensitivity of mouse on the Left-Right Axis")]
    [SerializeField] private float xSensitivity = 3f;
    [Tooltip("Sensitivity of mouse on the Up-Down Axis")]
    [SerializeField] private float ySensitivity = 3f;
    [Tooltip("Force of Upward Thrust applied when jumping")]
    [SerializeField] private float WingsForce = 1000f;
    [Tooltip("This is how long it takes for the wings to stop flapping")]
    [SerializeField] private float WingsPowerBurnSpeed = 1f;
    [Tooltip("This is how fast the power for the wings regenerate")]
    [SerializeField] private float WingsPowerRegenSpeed = 0.3f;
    [Tooltip("This is how much Wing power you have")]
    [SerializeField] private float WingsPowerAmount = 1f;

    [Header("Joint Options")]
    [Tooltip("Determines how strong the spring is")]
    [SerializeField] private float jointSpring = 20f;
    [Tooltip("Determines the Maximum force before you escape the spring")]
    [SerializeField] private float jointMaxForce = 40f;
    [Tooltip("This Layermask Decides wether or not object can be used as a floor for the spring")]
    [SerializeField] private LayerMask springEnvironmentMask = 0;

    public float GetWingPowerAmount()
    {
        return WingsPowerAmount;
    }


    private PlayerMovement playerMovement = null;

    private ConfigurableJoint joint;

    void Start()
    {
        //Get playermovementScript
        playerMovement = GetComponent<PlayerMovement>();

        //Get and Set the Configurable Joint
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    }

    void Update()
    {

        //Shoot out a raycast to set the height offset of the spring
        RaycastHit _hit;
        if (Physics.Raycast (transform.position, Vector3.down, out _hit, springEnvironmentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = Vector3.zero;
        }
        //Get movement input and set to a vector
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _yMove = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _yMove;

        // Movement vector to send
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * movingSpeed;

        // Send out movement
        playerMovement.Move(_velocity);

        //Get yRotation input and set to a vector
        float yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0, yRotation, 0) * xSensitivity;

        // Send out y Rotation
        playerMovement.Rotate(_rotation);

        //Get xRotation input and set it to a float for cameraRotation
        float xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = xRotation * ySensitivity;

        // Send out x Rotation
        playerMovement.RotateCamera(_cameraRotationX);

        Vector3 _wingsForce = Vector3.zero;
        //Get the jump Input and set it to a vector, disable the joint while Jumping
        if (Input.GetButton("Jump") && WingsPowerAmount > 0f)
        {
            WingsPowerAmount -= WingsPowerBurnSpeed * Time.deltaTime;

            if(WingsPowerAmount >= 0.01f)
            {
                _wingsForce = Vector3.up * WingsForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            WingsPowerAmount += WingsPowerRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        WingsPowerAmount = Mathf.Clamp(WingsPowerAmount, 0f, 1f); 
        //Send out Jump force
        playerMovement.ApplyWings(_wingsForce);
    }

    //Setting joint settings via struct of JointDrive Y
    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
