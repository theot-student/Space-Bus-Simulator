using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
   // ===================== MOTION =====================
    [Header("Motion Settings")]
    public float thrustForce = 1e+7f; // Lower force for smoother acceleration
    public float rotationForce = 120f; // Less sensitive rotation
    public float maxSpeed = 20f; // Limit spaceship speed
    public float rotationSpeed = 20f;

    // ===================== CONNECTED OBJECTS =====================
    [Header("Connected Objects")]
    private Rigidbody rb;  
    public Animator animator;
    private Animator playerAnimator;
    public Player player;

    // ===================== LAUNCHING & LANDING =====================
    [Header("Launching & Landing")]
    public bool canLaunch = false;
    
    [Space] // Adds a small gap in the Inspector
    public bool canLand = false;
    private bool wantToLand = false;
    public Quaternion initialRotation = Quaternion.Euler(0, 0, 0);
    public Vector3 landingPosition = new Vector3(0,0,0);
    public float landingRotationSpeed = 2f;
    public float landingSpeed = 100000f;
    public float landingSpeed2 = 1000f;
    public float launchingSpeed = 100f;
    public float rotationTol = 1f;
    public float positionTol = 0.2f;
    private bool isLanding = false;

    // ===================== PASSENGERS =====================
    [Header("Passenger Management")]
    public List<Seat> passengerSeats;
    public List<PNJ> passengers;

    // ===================== WEAPONS & FIRE =====================
    [Header("Weapon System")]
    public GameObject beamPrefab;
    public GameObject leftWeapon;
    public GameObject rightWeapon;
    public float fireRate = 0.1f;
    private float nextFireTime;
    public float destroyFireTime = 5f;
    public float fireForce = 100f;

    // ===================== HEALTH =====================
    [Header("Health System")]
    public HealthBarScript healthBar;
    public GameObject healthGameObject;
    public int maxHealth = 100;
    public int health;

    // ===================== ENEMY DETECTION =====================
    [Header("Detection & AI")]
    private bool ennemyDetected = false;
    public EnnemyScript[] classicEnnemyList;
    public float rangeDetection;
    public DialogueScript dialogueScript;

    void Update()
    {
    if (!PauseGameScript.gameIsPaused) {    
        bool isPlayingLaunchingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("Door closing");
        bool isPlayingLandingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("Door opening");
        
        if (isPlayingLaunchingAnimation){
            healthBar.setMaxHealth(maxHealth);
            health = maxHealth;
            rb.AddForce(new Vector3(0,1,0) * launchingSpeed, ForceMode.Force);
        } else if ((isPlayingLandingAnimation) || (isLanding)) {
            isLanding = true;
            if (Vector3.Distance(transform.position, landingPosition) > positionTol) {
                MoveTowardsTarget(rb, landingPosition, landingSpeed2);
            } else if (!(isPlayingLandingAnimation)){
                transform.position = landingPosition;
                transform.rotation = initialRotation;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                player.isDriving = false;
                player.exitShip(transform.position);
                isLanding = false;
                healthGameObject.SetActive(false);
            } 
        } else {
            if (player.isDriving) {
                if (wantToLand) {
                    HandleLanding();
                } else {
                    HandleMovement();
                    HandleRotation();
                    landing();
                    HandleFire();
                }
                Detection();
            }
            
        }
    }
    }

    void HandleFire(){
        if (Input.GetMouseButtonDown(0)) {
            if (nextFireTime < Time.time){
                nextFireTime = fireRate + Time.time;

                Vector3 dirBullet = leftWeapon.transform.forward;
 
                GameObject beamLeft = Instantiate(beamPrefab, leftWeapon.transform.position , leftWeapon.transform.rotation * beamPrefab.transform.rotation);
                GameObject beamRight = Instantiate(beamPrefab, rightWeapon.transform.position , rightWeapon.transform.rotation * beamPrefab.transform.rotation);

                beamLeft.GetComponent<Rigidbody>().AddForce(dirBullet * fireForce);
                Destroy(beamLeft, destroyFireTime);
                beamRight.GetComponent<Rigidbody>().AddForce(dirBullet * fireForce);
                Destroy(beamRight, destroyFireTime);
            }
        }
    }


    void HandleLanding() {
        Vector3 preLandingPosition = landingPosition + new Vector3(0,1f,0);
        if ((Quaternion.Angle(transform.rotation,initialRotation) > rotationTol) || (Vector3.Distance(transform.position,preLandingPosition) > positionTol)){
            // remettre le vaisseau droit
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * landingRotationSpeed);
            MoveTowardsTarget(rb, preLandingPosition, Time.deltaTime * landingSpeed);
        }    
        else {
            transform.rotation = initialRotation;
            transform.position = preLandingPosition;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //lancer l'animation d'atterissage
            animator.SetBool("isDriven", false);
            playerAnimator.SetBool("isSitting", false);
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

    public void getHit(int healthLost){
        health = health - healthLost;
        healthBar.setHealth(health);
    }

     void Detection(){
        foreach (EnnemyScript ennemyScript in classicEnnemyList) {
            if (Vector3.Distance(transform.position,ennemyScript.gameObject.transform.position) <= rangeDetection){
                ennemyScript.isDetected = true;
                if (!ennemyDetected){
                    ennemyDetectedPrompt();
                }
            } else {
                ennemyScript.isDetected = false;
                ennemyDetected = false;
            }
        }

    }

    void ennemyDetectedPrompt(){
        ennemyDetected = true;
        dialogueScript.newDialogue(new string[] {"Mince, des ennemis ! Je ferais mieux de fuir ou de riposter !"});
    }
}
