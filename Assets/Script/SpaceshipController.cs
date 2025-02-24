using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    System.Random random = new System.Random(); // For randomness

   // ===================== MOTION =====================
    [Header("Motion Settings")]
    public float thrustForce = 1e+7f; // Lower force for smoother acceleration
    public float rotationForce = 30; // Less sensitive rotation
    public float maxSpeed = 20f; // Limit spaceship speed
    public float rotationSpeed = 20f;
    public float boostForce = 4;
    
    // ==================== PARTICLE EFFECTS =====================
    [Header("Particles Objects")]
    public Light boosterLight1;
    public Light boosterLight2;
    public float drivingLightIntensity;
    public float staticLightIntensity;
    public float boostLightIntensity;
    public Color forwardColor;
    public Color backwardColor;
    public Color boostColor;

    // ===================== CONNECTED OBJECTS =====================
    [Header("Connected Objects")]
    private Rigidbody rb;  
    public Animator animator;
    private Animator playerAnimator;
    public Player player;
    public SpaceStationScript currentSpaceStation;
    public SpaceStationScript possibleSpaceStation;
    public SpaceStationScript targetSpaceStation;
    public SpaceStationsScript spaceStationsScript; 
    public WaypointMarker waypointMarker;
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
    public List<PNJScript> passengers;
    public HashSet<int> availableSeatsIndexes = new HashSet<int>(){ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public int nbOfPNJsInside = 0;
    public int nbOfPNJsRequired;
    public int nbOfpassengersToTransport = 50;
    public TextMeshProUGUI passengerInfos;
    public int transportedPassengers = 0;

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
    public List<EnnemyScript> classicEnnemyList;
    public float rangeDetection;
    public DialogueScript dialogueScript;

    // ===================== ENEMY SPAWN =====================
    [Header("Ennemy Spawn")]
    public GameObject ennemySpaceshipPrefab;
    public GameObject ennemyExplosionPrefab;
    public GameObject ennemyHealthBarPrefab;
    public Transform canvasTransform;
    public Camera mainCamera;
    public Vector3 usualSpawnOffset;
    public float spawnOffsetRange;

    // ===================== EVENT =====================
    [Header("Events")]
    public float distanceEvent;
    bool eventHappened;
    // public GameObject PNJMarker;
    // public WaypointMarker PNJMarkerScript;
    private PNJScript passengerToHelp;
    bool pnjEventActive = false;

    public void InitializePassengerList(int numberOfSeats)
    {
        passengers.Clear(); // Ensure it's empty first

        // Add null values for each seat
        for (int i = 0; i < numberOfSeats; i++)
        {
            passengers.Add(null);  // Null means no passenger initially
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for space movement
        rb.linearDamping = 0.5f; // Helps prevent excessive drifting
        rb.angularDamping = 0.5f; // Helps slow down rotation
        initialRotation = transform.rotation;

        //init health
        healthBar.setMaxHealth(maxHealth);
        healthGameObject.SetActive(false);
        health = maxHealth;
        
        InitializePassengerList(passengerSeats.Count);

        //init lights
        boosterLight1.intensity = 0f;
        boosterLight2.intensity = 0f;

        playerAnimator = player.GetComponent<Animator>();
        currentSpaceStation.gameObject.transform.GetChild(0).GetComponent<PNJManager>().SpawnPNJs();
        nbOfPNJsRequired = currentSpaceStation.gameObject.transform.GetChild(0).GetComponent<PNJManager>().numberOfPNJs;
    }

    


    void Update()
    {
        if (!PauseGameScript.gameIsPaused) {    
            bool isPlayingLaunchingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("Door closing");
            bool isPlayingLandingAnimation = animator.GetCurrentAnimatorStateInfo(0).IsName("Door opening");
            
            if (isPlayingLaunchingAnimation){
                rb.AddForce(new Vector3(0,1,0) * launchingSpeed, ForceMode.Force);
            } else if ((isPlayingLandingAnimation) || (isLanding)) {
                StaticBoosterEffects();
                isLanding = true;
                if (Vector3.Distance(transform.position, landingPosition) > positionTol) {
                    MoveTowardsTarget(rb, landingPosition, landingSpeed2);
                } else if (!(isPlayingLandingAnimation)){
                    transform.position = landingPosition;
                    transform.rotation = initialRotation;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    isLanding = false;
                    healthGameObject.SetActive(false);
                    boosterLight1.intensity = 0f;
                    boosterLight2.intensity = 0f;
                    GetComponent<Rigidbody>().isKinematic=true;
                    player.exitShip();
                    if (currentSpaceStation == targetSpaceStation){
                        LandedAtTargetStation();
                    }
                if (passengerToHelp != null) {
                    passengerToHelp.gameObject.GetComponent<AudioSource>().Stop();
                }
                pnjEventActive = false;
                // PNJMarker.SetActive(false);
                } 
            } else {
                if (player.isDriving) {
                    if (wantToLand) {
                        HandleLanding();
                    } else if (!(player.isFirstPerson && player.cameraUnlocked)) {
                        HandleMovement();
                        HandleRotation();
                        landing();
                        HandleFire();
                    }
                    Detection();
                    CheckLife();
                }
                
            }
        }
        passengerInfos.text = "Passagers transportés : " + transportedPassengers + "/" + nbOfpassengersToTransport;
        CheckWin();
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
    public void handleLaunching(){
        updateDestination();
        healthBar.setMaxHealth(maxHealth);
        health = maxHealth;
        currentSpaceStation.currentlydockedSpaceship = null;
        StaticBoosterEffects();
        eventHappened = false;
    }


    void HandleLanding() {
        Vector3 preLandingPosition = landingPosition + new Vector3(0,1.5f,0);
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
            wantToLand = false;
        }
        currentSpaceStation = possibleSpaceStation;
        currentSpaceStation.currentlydockedSpaceship = this;
    }
    void LandedAtTargetStation(){
        targetSpaceStation.gameObject.transform.GetChild(0).GetComponent<PNJManager>().SpawnPNJs();
        DismountPNJs();
        updateDestination();
        nbOfPNJsRequired = currentSpaceStation.gameObject.transform.GetChild(0).GetComponent<PNJManager>().numberOfPNJs;
        nbOfPNJsInside = 0;
    }

    void DismountPNJs(){
        for (int seatIndex = 0; seatIndex < 10; seatIndex++){
            if (!availableSeatsIndexes.Contains(seatIndex)){
                Seat seat = passengerSeats[seatIndex];
                PNJScript pnj = passengers[seatIndex];
                print(pnj);
                passengers[seatIndex] = null;

                seat.pnj = null;
                seat.pnjId = null;

                pnj.seat = null;
                pnj.GetComponent<Animator>().SetBool("isSitting", false);
                pnj.GetComponent<Animator>().Play("HumanoidIdle", 0, 0f);
                pnj.GetComponent<CharacterController>().enabled = true;
                pnj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                pnj.transform.position = this.transform.position + new Vector3(-0.13f, 0, -0.4f);
                pnj.isSitting = false;
                pnj.isAtTargetStation = true;
                pnj.pnjManager = currentSpaceStation.gameObject.transform.GetChild(0).GetComponent<PNJManager>();
            }
        }
        availableSeatsIndexes = new HashSet<int>(){ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
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
        float realForce = thrustForce;
        Vector3 forceDirection = Vector3.zero;
        bool isBoosting = Input.GetKey(KeyCode.LeftShift);
        // Adjusted for AZERTY keyboard (Z = W in Unity)
        if (Input.GetKey(KeyCode.W)) { // Forward (Z on AZERTY)
            if (!isBoosting) {
                forceDirection += transform.right; 
                BoosterEffects();
            } else {
                forceDirection += transform.right ; 
                HugeBoostEffects();
                realForce = realForce * boostForce;
            }
            
        } else {
            StaticBoosterEffects();
        }
        if (Input.GetKey(KeyCode.S)) {
            forceDirection -=transform.right;  // Backward
            BackwardBoosterEffects();
        }
        
        // Apply force gradually over time
        rb.AddForce(forceDirection.normalized * realForce * Time.deltaTime);

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
        if (classicEnnemyList.Count > 0){
            bool currentEnnemyDetection = false;
            foreach (EnnemyScript ennemyScript in classicEnnemyList) {
                if (Vector3.Distance(transform.position,ennemyScript.gameObject.transform.position) <= rangeDetection){
                    ennemyScript.isDetected = true;
                    currentEnnemyDetection = true;
                    if (!ennemyDetected){
                        ennemyDetectedPrompt();
                    }
                } else {
                    ennemyScript.isDetected = false;
                }
            }
            ennemyDetected = currentEnnemyDetection;
        }

        //Ennemy Spawn
        if (Input.GetKeyDown(KeyCode.M)) DoomedEnnemiesSpawn(6);

        //check Event
        if (Vector3.Distance(transform.position, targetSpaceStation.transform.position) < distanceEvent && !eventHappened) {
            eventHappened = true;
            RandomEvent();
        }
    }

    void RandomEvent(){
        float rand = Random.Range(1f,4f);
        if (rand < 2) {
            FrontEnnemiesSpawn(4);
        } else if (rand >= 2 && rand < 3){
            CaughtMiddleEnnemiesSpawn(6);
        } else if (rand >= 3 && rand <= 4){
            PNJEvent();
        }
    }

    void PNJEvent(){
        int choice;
        do {
           choice  = Random.Range(0, passengers.Count);
        } while (passengers[choice] == null);

        // PNJMarkerScript.offset = new Vector3(0,0.05f,0);
        // PNJMarkerScript.target = passengers[choice].gameObject.transform;
        // PNJMarker.SetActive(true);
        passengerToHelp = passengers[choice];
        dialogueScript.newDialogue(new string[] {"Un des passagers à besoin d'aide ! (appuyer sur C puis U pour la caméra interne)"});
        pnjEventActive = true;
        PNJCry();
    }

    void PNJCry(){
        //passengerToHelp.gameObject.GetComponent<AudioSource>().loop = true;
        passengerToHelp.gameObject.GetComponent<AudioSource>().Play();
    }

    void ennemyDetectedPrompt(){
        ennemyDetected = true;
        dialogueScript.newDialogue(new string[] {"Mince, des ennemis ! Je ferais mieux de fuir ou de riposter !"});
    }

    void CheckLife() {
        if (health <= 0) {
            SceneManager.LoadScene("GameOver");
        }
    }

    void CheckWin(){
        if (transportedPassengers >= nbOfpassengersToTransport){
            SceneManager.LoadScene("GameWon");
        }
    }

    public void ClassicEnnemyDestroyed(EnnemyScript ennemyScript){
        classicEnnemyList.Remove(ennemyScript);
    }



    public void usualEnnemiesSpawn(int numberOfEnnemies){
        for (int i = 0; i < numberOfEnnemies;i++){
            EnnemySpawn(usualSpawnOffset);
        }
    }

    public void FrontEnnemiesSpawn(int numberOfEnnemies){
        Vector3 frontOffset = transform.right * spawnOffsetRange;
        for (int i = 0; i < numberOfEnnemies;i++){
            EnnemySpawn(frontOffset);
        }
    }

    public void CaughtMiddleEnnemiesSpawn(int numberOfEnnemies){
        Vector3 rightOffset = transform.forward * spawnOffsetRange;
        for (int i = 0; i < numberOfEnnemies;i++){
            EnnemySpawn(Mathf.Pow(-1,i) * rightOffset);
        }
    }

    public void DoomedEnnemiesSpawn(int numberOfEnnemies){
        Vector3 rightOffset = transform.forward * spawnOffsetRange;
        Vector3 frontOffset = transform.right * spawnOffsetRange;
        Vector3 upOffset = transform.up * spawnOffsetRange;
        for (int i = 0; i < numberOfEnnemies;i++){
            switch (i % 6){
                case 0:
                    EnnemySpawn(rightOffset);
                    break;
                case 1:
                    EnnemySpawn(-rightOffset);
                    break;
                case 2:
                    EnnemySpawn(upOffset);
                    break;
                case 3:
                    EnnemySpawn(-upOffset);
                    break;
                case 4:
                    EnnemySpawn(-frontOffset);
                    break;
                case 5:
                    EnnemySpawn(frontOffset);
                    break;
            }
        }
    }

    void EnnemySpawn(Vector3 spawnOffset){
        GameObject ennemySpaceship = Instantiate(ennemySpaceshipPrefab, transform.position + spawnOffset, transform.rotation);
        GameObject ennemyExplosion = Instantiate(ennemyExplosionPrefab, transform.position + spawnOffset, transform.rotation);
        GameObject ennemyHealthBar = Instantiate(ennemyHealthBarPrefab, canvasTransform);

        EnnemyScript ennemyScript = ennemySpaceship.GetComponent<EnnemyScript>();
        EnnemyPrefabScript ennemyPrefabScript = ennemyExplosion.GetComponent<EnnemyPrefabScript>();
        EnnemyHealthBarMarker ennemyHealthBarMarker = ennemySpaceship.GetComponent<EnnemyHealthBarMarker>();
        GameObject explosionLight = ennemyExplosion.transform.GetChild(0).gameObject;
        GameObject explosionParticles = ennemyExplosion.transform.GetChild(1).gameObject;

        //add attributes of new spaceship
        ennemyScript.ennemyExplosion = ennemyExplosion;
        //ennemy health bar marker
        ennemyHealthBarMarker.healthGameObject = ennemyHealthBar;
        ennemyHealthBarMarker.img = ennemyHealthBar.GetComponent<Image>();
        ennemyHealthBarMarker.ennemy = ennemySpaceship;
        ennemyHealthBarMarker.target = ennemySpaceship.transform;
        ennemyHealthBarMarker.camera = mainCamera;

        ennemyScript.healthBar = ennemyHealthBar.GetComponent<HealthBarScript>();
        ennemyScript.healthGameObject = ennemyHealthBar;
        ennemyScript.ennemyHealthBarMarker = ennemyHealthBarMarker;

        ennemyScript.ennemyPrefabScript = ennemyPrefabScript;
        ennemyScript.spaceship = gameObject;
        ennemyScript.spaceshipController = this;
        ennemyScript.explosionParticles = explosionParticles.GetComponent<ParticleSystem>(); 
        ennemyScript.explosionLightScript = explosionLight.GetComponent<ExplosionLightScript>();

        ennemyPrefabScript.ennemySpaceship = ennemySpaceship;

        classicEnnemyList.Add(ennemyScript);
    }

    int GetAndRemoveRandomElement(HashSet<int> set)
    {
        List<int> tempList = new List<int>(set); // Convert HashSet to List
        int randomIndex = random.Next(tempList.Count); // Get random index
        int randomElement = tempList[randomIndex]; // Get the element
        set.Remove(randomElement); // Remove it from the HashSet
        return randomElement;
    }

    public void wantsToEmbark(PNJScript pnj){
        if (availableSeatsIndexes.Count != 0){
            int seatIndex = GetAndRemoveRandomElement(availableSeatsIndexes);
            passengerSeats[seatIndex].pnj = pnj;
            passengers[seatIndex] = pnj;

            Animator pnjAnimator = pnj.GetComponent<Animator>();
            pnjAnimator.SetBool("isSitting", true);
            pnjAnimator.SetFloat("speed", 0f);
            pnj.seat = passengerSeats[seatIndex];
            pnj.controller.enabled = false;
            pnj.navmesh.enabled = false;

            pnj.isSitting = true;
            nbOfPNJsInside++;
            if (nbOfPNJsInside == nbOfPNJsRequired) {
                canLaunch = true;
            }
        } else {
            canLaunch = true;
        }
    }

    void BackwardBoosterEffects(){
        float realLightIntensity = staticLightIntensity + Random.Range(-0.05f,0.05f);
        boosterLight1.intensity = realLightIntensity;
        boosterLight2.intensity = realLightIntensity;
        boosterLight1.color = backwardColor;
        boosterLight2.color = backwardColor;
    }

    void BoosterEffects(){
        float realLightIntensity = drivingLightIntensity + Random.Range(-0.2f,0.2f);
        boosterLight1.intensity = realLightIntensity;
        boosterLight2.intensity = realLightIntensity;
        boosterLight1.color = forwardColor;
        boosterLight2.color = forwardColor;
    }
    
    void StaticBoosterEffects(){
        float realLightIntensity = staticLightIntensity + Random.Range(-0.05f,0.05f);
        boosterLight1.intensity = realLightIntensity;
        boosterLight2.intensity = realLightIntensity;
        boosterLight1.color = forwardColor;
        boosterLight2.color = forwardColor;
    }

    void HugeBoostEffects() {
        float realLightIntensity = boostLightIntensity + Random.Range(-0.1f,0.1f);
        boosterLight1.intensity = realLightIntensity;
        boosterLight2.intensity = realLightIntensity;
        boosterLight1.color = boostColor;
        boosterLight2.color = boostColor;
    }

    void updateDestination(){
        int spaceStationIndex;
        do {
            spaceStationIndex = UnityEngine.Random.Range(0,spaceStationsScript.nbSpaceStations);
        } while(spaceStationsScript.spaceStations[spaceStationIndex]==this.currentSpaceStation);
        targetSpaceStation = spaceStationsScript.spaceStations[spaceStationIndex];
        waypointMarker.target = targetSpaceStation.transform;
    }
}
