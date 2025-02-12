using UnityEngine;

public class EnnemyScript : MonoBehaviour
{
    public GameObject spaceship;
    public SpaceshipController spaceshipController;
    private Rigidbody rb;  

    public bool isDetected;
    public float rangeDetection;
    public float fireRangeDetection;
    private bool isWalkingAround = true;
    private bool isChasing = false;
    private bool isFiring = false;

    //walking around
    public Vector3 start;
    public Vector3 end;
    private bool gotoEnd = true;

    //motion
    public float force = 1000f;
    public float maxSpeed = 20f;

    //Fire
    public GameObject ennemiBeamPrefab;
    public float destroyFireTime = 5f;
    public float fireForce = 1000f;
    public float fireRate = 0.2f;
    private float nextFireTime;

    //health
    public HealthBarScript healthBar;
    public GameObject healthGameObject;
    public int maxHealth = 100;
    public int health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //init health
        healthBar.setMaxHealth(maxHealth);
        healthGameObject.SetActive(false);
        health = maxHealth;
    }


    void Update()
    {   
        if (!PauseGameScript.gameIsPaused) {
            if (isWalkingAround) {
                WalkingAround();
                Detection();
            }   
            if (isChasing){
                ChaseSpaceship();
                Detection();
            }
            if (isFiring)
            {
                Fire();
                Detection();
            }
        }
    }

    void Detection(){
        if (Vector3.Distance(transform.position,spaceship.transform.position) <= fireRangeDetection) {
            isWalkingAround = false;
            isChasing = false;
            isFiring = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        } else if (Vector3.Distance(transform.position,spaceship.transform.position) <= rangeDetection){
            isWalkingAround = false;
            isChasing = true;
            isFiring = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else {
            isWalkingAround = true;
            isChasing = false;
            isFiring = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void MoveForward(Vector3 position){
        Vector3 dir = (position - transform.position).normalized;
        transform.LookAt(position);
        rb.AddForce(dir * force);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    void WalkingAround(){
        if (gotoEnd) {
            MoveForward(end);
        } else {
            MoveForward(start);
        }
        if (Vector3.Distance(transform.position, start) < 0.2) {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gotoEnd = true;
        }
        if (Vector3.Distance(transform.position, end) < 0.2){
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            gotoEnd = false;
        }
    }

    void ChaseSpaceship(){
        MoveForward(spaceship.transform.position);
    }

    void Fire(){
        transform.LookAt(spaceship.transform.position);

        if (nextFireTime < Time.time){
            nextFireTime = fireRate + Time.time;

            Vector3 dirBullet = transform.forward;
 
            GameObject ennemyBeam = Instantiate(ennemiBeamPrefab, transform.position , transform.rotation * ennemiBeamPrefab.transform.rotation);
            ennemyBeam.GetComponent<EnnemyBeamScript>().spaceship = spaceshipController;
            ennemyBeam.GetComponent<Rigidbody>().AddForce(dirBullet * fireForce);
            Destroy(ennemyBeam, destroyFireTime);
        }

    }

    public void getHit(int healthLost){
        health = health - healthLost;
        healthBar.setHealth(health);
        
    }
}
