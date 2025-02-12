using UnityEngine;

public class OuterCameraScript : MonoBehaviour
{
    public float elevation;
    public Player player;
    public GameObject spaceship;
    private Vector3 v;
    public float x = -2.5f;
    public float y = 1f;
    public float z = 0f; 

    // Third-person (non-driving) camera settings:
    [Header("Third-Person Camera Settings")]
    public float distanceBehind = 3f;       // How far behind the player the camera should be
    public float heightOffset = 0.2f;        // How high above the player the camera should be
    public float mouseSensitivity = 3.0f;     // How responsive the camera orbit is to mouse movement

    // Internal variables to keep track of the orbit’s angles:
    private float yaw = 0f;
    private float pitch = 10f;              // You can set a default pitch here

    void Start()
    {
        // Lock and hide the cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        // Only update the mouse-controlled orbit if the player is not driving.
        if (!player.isDriving)
        {
            // Update the yaw (horizontal rotation) and pitch (vertical rotation) using mouse input.
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // Clamp the pitch to avoid the camera flipping too far up or down.
            pitch = Mathf.Clamp(pitch, -10f, 45f);
        }
    }

    void LateUpdate()
    {
        if (player.isDriving)
        {
            // When driving, position the camera relative to the spaceship.
            v = x * spaceship.transform.right + y * spaceship.transform.up + z * spaceship.transform.forward;
            transform.position = spaceship.transform.position + v;
            transform.LookAt(spaceship.transform.position + elevation * spaceship.transform.up, spaceship.transform.up);
        }
        else
        {
            // When not driving, calculate an orbiting offset using the yaw and pitch.
            // Create a rotation from the current pitch and yaw.
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            
            // The base offset behind the player is along the negative Z-axis.
            Vector3 orbitOffset = rotation * new Vector3(0, 0, -distanceBehind);
            
            // Determine the target camera position relative to the player's position.
            Vector3 targetPosition = player.transform.position + orbitOffset + Vector3.up * heightOffset;
            
            // Smoothly move the camera to the target position.
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
            
            // Ensure the camera is looking at the player (adding a small vertical offset to aim higher, if desired).
            transform.LookAt(player.transform.position + Vector3.up * heightOffset);
        }
    }
}
