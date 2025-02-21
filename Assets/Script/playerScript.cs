using UnityEngine;
using TMPro;
public class Player : MonoBehaviour
{
    public bool isDriving = false;
    public GameObject innerCamera;
    public SpaceshipController spaceship;
    private Animator spaceshipAnimator;

    private CharacterController controller;
    private Animator animator; // Animator component
    private float speed;
    public TextMeshProUGUI textOnScreen;
    private bool withinReachToShip = false;
    public bool isFirstPerson = false;

    public Transform driverSeat; // Assign the driver's seat Transform in the Inspector

    public GameObject healthBar;
    public bool cameraUnlocked = false;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float mouseSensitivity = 2f;

    private float rotationX = 0f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get Animator component
        spaceshipAnimator = spaceship.GetComponent<Animator>();

        if (controller == null)
        {
            Debug.LogError("CharacterController missing from Player!");
            return;
        }

        
        controller.enabled = true;
        textOnScreen.text = ""; 
    }

void Update()
{
    if (!PauseGameScript.gameIsPaused) {
        if (!isDriving)
        {
            animator.enabled = !isFirstPerson;
            HandleMovement();
            HandleRotation();
            if (withinReachToShip && Input.GetKey(KeyCode.E))
            {
                isDriving = true;
                spaceshipAnimator.SetBool("isDriven", true);
                HideMessage();
                healthBar.SetActive(true);
                animator.SetFloat("speed",0f);
                animator.SetBool("isSitting", true);
            }
        }
        else
        {
            // Move player to the driver's seat inside the spaceship

            transform.rotation = driverSeat.transform.rotation; // Align player with spaceship
            transform.position = driverSeat.transform.position;

            // Disable movement components
            controller.enabled = false;

            if (Input.GetKey(KeyCode.L)){
                cameraUnlocked = !cameraUnlocked;
            }       
        }
    }
}

    void HandleMovement()
    {
        if (controller == null || !controller.enabled) return;

        float moveZ = Input.GetAxis("Vertical");
        bool isWalking = moveZ > 0f; 
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        speed = isWalking ? moveSpeed : 0;

        if (isRunning){
            speed = speed * sprintMultiplier;
            if (!isFirstPerson){
                animator.SetBool("isRunning", true);
            }
            
        }
        else{
            if (!isFirstPerson){
                animator.SetBool("isRunning", false);
            }
        }

        // Apply movement
        controller.Move(transform.forward * speed * Time.deltaTime);
        if (!isFirstPerson){
            animator.SetFloat("speed", speed); // Set speed for animations
        }
    }

    void HandleRotation()
    {
        if (innerCamera == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (isFirstPerson){
            transform.Rotate(this.transform.up * mouseX);
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            innerCamera.transform.localRotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, 0f);
        }
        else{
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -30f, 45f); // Adjust these limits as desired.
            innerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            if (!isFirstPerson){
                float turn = Input.GetAxis("Horizontal");
                animator.SetFloat("rotation", turn);
            }
            

        }
    }
    void HideMessage()
    {
        textOnScreen.text = "";
    }
    public void ShowMessage(string message, float duration = 2f)
    {
        textOnScreen.text = message;
        Invoke(nameof(HideMessage), duration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDriving && other.transform.gameObject.name == spaceship.name)
        {
            ShowMessage("Appuyez sur 'E' pour rentrer dans votre SpaceBus");
            withinReachToShip = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (!isDriving && other.transform.gameObject.name == spaceship.name)
        {
            HideMessage();
            withinReachToShip = false;
        }
    }

    public void exitShip(Vector3 shipPosition){
        animator.enabled = true;
        controller.enabled = true;

        transform.position = shipPosition + new Vector3(0,0,6);
    }
}
