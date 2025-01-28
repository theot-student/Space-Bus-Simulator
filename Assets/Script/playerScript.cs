using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isDriving = false;
    public GameObject innerCamera;
    public GameObject spaceship;
    private CharacterController controller;
    private Animator animator; // Animator component
    private Vector3 velocity;
    private float speedVelocity;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float mouseSensitivity = 2f;

    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get Animator component

        if (controller == null)
        {
            Debug.LogError("CharacterController missing from Player!");
            return;
        }

        controller.enabled = true;
    }

    void Update()
    {
        if (!isDriving)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    void HandleMovement()
    {
        if (controller == null || !controller.enabled) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = isRunning ? moveSpeed * sprintMultiplier : moveSpeed;
        if(isRunning){
            animator.SetBool("isRunning", true); 
        }
        else{
            animator.SetBool("isRunning", false); 
        }
        float currentSpeed = Mathf.SmoothDamp(controller.velocity.magnitude, targetSpeed, ref speedVelocity, smoothTime);

        // Apply movement
        controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

        float movementMagnitude = new Vector2(moveX, moveZ).magnitude;
        animator.SetFloat("speed", movementMagnitude * currentSpeed); // Set speed for animations
    }

    void HandleRotation()
    {
        if (innerCamera == null)
        {
            Debug.LogWarning("Inner Camera not assigned!");
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        innerCamera.transform.localRotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, 0f);
    }
}
