using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 5000f; // Lower force for smoother acceleration
    public float rotationForce = 1f; // Less sensitive rotation
    public float maxSpeed = 50f; // Limit spaceship speed
    public float rotationSpeed = 0.1f;
    private Rigidbody rb;  
    public Animator animator;
    public Player player;
    public bool canLaunch = true;
    public bool canLand = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for space movement
        rb.linearDamping = 0.5f; // Helps prevent excessive drifting
        rb.angularDamping = 0.5f; // Helps slow down rotation
    }

    void Update()
    {
        bool isPlayingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("launching");
        // Active ou désactive le mode kinematic
        rb.isKinematic = isPlayingAnimation;
        HandleMovement();
        HandleRotation();
    }



    void HandleMovement()
    {
        Vector3 forceDirection = Vector3.zero;
        // Adjusted for AZERTY keyboard (Z = W in Unity)
        if (Input.GetKey(KeyCode.W)) forceDirection += transform.right; // Forward (Z on AZERTY)
        if (Input.GetKey(KeyCode.S)) forceDirection -=transform.right;  // Backward
        
        // Apply force gradually over time
        rb.AddForce(forceDirection.normalized * thrustForce * Time.deltaTime);

        // Limit maximum speed to prevent excessive acceleration
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

    }

    void HandleRotation()
    {
        Vector3 torque = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) torque += rotationSpeed * transform.right;
        if (Input.GetKey(KeyCode.D)) torque -= rotationSpeed * transform.right;

        float mouseX = Input.GetAxis("Mouse X") * rotationForce;
        float mouseY = Input.GetAxis("Mouse Y") * rotationForce;

        // Smooth, less sensitive rotation
        torque += mouseX * transform.up + mouseY * transform.forward;

        // Apply torque for rotation
        rb.AddTorque(torque, ForceMode.Force);
    }
}
