using UnityEngine;

/// <summary>
/// Script used for the player main movement using CharacterController component
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Transform playerCameraTransform;
    
    [Header("Player settings")]
    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f; //Player rotation smoothness
    private float turnSmoothVelocity;

    [Header("Free fall settings")] 
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance = 0.4f;
    private Vector3 actualFallVelocity;
    private bool isGrounded;

    void Awake()
    {
        //Cash variables
        controller = GetComponent<CharacterController>();
        playerCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        CheckGround();
        ApplyGravity();
        
        //Get the input axis to have a direction(vector3) where the player is heading
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0f, v).normalized;
        
        //If there is any movement
        if (direction.magnitude >= 0.1f)
        {
            float moveDirDegrees = GetMoveDirectionInDegrees(direction);
            
            RotateTowardsMoveDirection(moveDirDegrees);
            MovePlayer(moveDirDegrees);
        }
    }

    /// <summary>
    /// Move the player based on the player speed and the camera angle
    /// </summary>
    /// <param name="targetAngle">Angle in degrees that defines the player move direction</param>
    private void MovePlayer(float targetAngle)
    {
        Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        controller.Move(moveDir * playerSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// Rotate the player smoothly towards the move direction
    /// </summary>
    /// <param name="targetAngle">Angle in degrees in which the player will rotate to</param>
    private void RotateTowardsMoveDirection(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle,0f);
    }
    
    /// <summary>
    /// Returns a direction in degrees based on the player position
    /// </summary>
    /// <param name="direction">Direction to convert in degrees</param>
    private float GetMoveDirectionInDegrees(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCameraTransform.eulerAngles.y;
    }
    
    /// <summary>
    /// Apply free fall gravity forces
    /// </summary>
    private void ApplyGravity()
    {
        //If is in the ground reset fall velocity
        if (isGrounded && actualFallVelocity.y < 0f)
        {
            actualFallVelocity.y = -1f;
        }
        
        //free fall  ( (1/2) * g * t^2 )
        actualFallVelocity.y += gravity * Time.deltaTime;
        controller.Move(actualFallVelocity * Time.deltaTime);
    }
    
    /// <summary>
    /// Check if the player is touching the ground
    /// </summary>
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }
}
