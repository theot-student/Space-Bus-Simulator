using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 5000f; // Lower force for smoother acceleration
    public float rotationForce = 5000f; // Less sensitive rotation
    public float maxSpeed = 50f; // Limit spaceship speed
    public float dragFactor = 0.999f; // Slow down movement naturally
    public float angularDragFactor = 0.999f; // Slow down rotation naturally

    private Rigidbody rb;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for space movement
        rb.linearDamping = 0.5f; // Helps prevent excessive drifting
        rb.angularDamping = 0.5f; // Helps slow down rotation
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 forceDirection = Vector3.zero;

        // Adjusted for AZERTY keyboard (Z = W in Unity)
        if (Input.GetKey(KeyCode.W)) forceDirection += transform.right; // Forward (Z on AZERTY)
        if (Input.GetKey(KeyCode.S)) forceDirection -=transform.right;  // Backward
        if (Input.GetKey(KeyCode.A)) forceDirection += transform.forward; // Left
        if (Input.GetKey(KeyCode.D)) forceDirection -= transform.forward; // Right

        // Apply force gradually over time
        rb.AddForce(forceDirection.normalized * thrustForce * Time.deltaTime, ForceMode.Force);

        // Limit maximum speed to prevent excessive acceleration
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Apply slight drag for smooth stopping
        rb.linearVelocity *= dragFactor;
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationForce;
        float mouseY = Input.GetAxis("Mouse Y") * rotationForce;

        // Smooth, less sensitive rotation
        Vector3 torque = new Vector3(-mouseY, mouseX, 0) * Time.deltaTime;

        // Apply torque for rotation
        rb.AddTorque(torque, ForceMode.Force);

        // Apply angular drag for smooth stopping
        rb.angularVelocity *= angularDragFactor;
    }
}
