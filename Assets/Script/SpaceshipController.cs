using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    public float thrustForce = 50000f; // Lower force for smoother acceleration
    public float rotationForce = 0.5f; // Less sensitive rotation
    public float maxSpeed = 20f; // Limit spaceship speed
    public float rotationSpeed = 0.03f;
    private Rigidbody rb;  
    public Animator animator;
    public Player player;
    public bool canLaunch = false;
    public bool canLand = false;
    private bool wantToLand = false;
    Quaternion initialRotation = Quaternion.Euler(0, 0, 0);
    public Vector3 landingPosition = new Vector3(0,0,0);
    public float landingRotationSpeed = 2f;
    public float landingSpeed = 2f;
    public float rotationTol = 2f;
    public float positionTol = 0.3f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for space movement
        rb.linearDamping = 0.5f; // Helps prevent excessive drifting
        rb.angularDamping = 0.5f; // Helps slow down rotation
        initialRotation = transform.rotation;
    }

    void Update()
    {
        bool isPlayingLaunchingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("Door closing");

        // Active ou désactive le mode kinematic
        if (isPlayingLaunchingAnimation){
            Vector3 targetLaunching = transform.position + new Vector3(0,2,0);
            MoveTowardsTarget(rb, targetLaunching, landingSpeed);
        } else if (player.isDriving) {
                if (wantToLand) {
                    HandleLanding();
                } else {
                    HandleMovement();
                    HandleRotation();
                    landing();
                }
        }
        
        
    }

    void HandleLanding() {
        if ((Quaternion.Angle(transform.rotation,initialRotation) > rotationTol) || (Vector3.Distance(transform.position,landingPosition) > positionTol)){
            // remettre le vaisseau droit
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * landingRotationSpeed);
            MoveTowardsTarget(rb, landingPosition, landingSpeed);
        }    
        else {
            transform.rotation = initialRotation;
            transform.position = landingPosition;
            //lancer l'animation d'atterissage
            //A FAIRE

            wantToLand = false;
        }
    

        
    }

    void MoveTowardsTarget(Rigidbody rb, Vector3 targetPosition, float force)
    {
        Vector3 direction = (targetPosition - rb.position).normalized; // Direction vers la cible
        rb.AddForce(direction * force, ForceMode.Force); // Applique une force continue
    }

    void landing() {
        if (Input.GetKey(KeyCode.L) && canLand) {
            wantToLand = true;
            //on enlève les forces
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
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
