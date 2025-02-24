using UnityEngine;

public class PNJScript : MonoBehaviour
{
    public int id;
    private static int PNJCounter = 0;
    public SpaceshipController spaceship;
    public PNJManager pnjManager;
    private float distanceToEmbark = 0.8f;
    private float distanceToDebark = 1.5f;
    public bool isSitting = false;
    public bool isAtTargetStation=false;
    public UnityEngine.AI.NavMeshAgent agent;
    public Vector3 target;
    private Vector3 spaceShipOffset = new Vector3(-0.13f, 0, -0.4f);
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Seat seat;
    public CharacterController controller;
    public UnityEngine.AI.NavMeshAgent navmesh;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();

        target = this.transform.position;
        PNJCounter += 1;
        id = PNJCounter;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSitting){
            // Move pnj to the driver's seat inside the spaceship
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation; // Align player with spaceship
        }
        else{
            if (isAtTargetStation){
                target = pnjManager.transform.position;
                if (Vector3.Distance(target, this.transform.position)<=distanceToDebark){
                    Destroy(gameObject, 0.1f);
                }
            }
            else{
                target = spaceship.transform.position + spaceShipOffset;
                if (Vector3.Distance(target, this.transform.position)<=distanceToEmbark){
                    target = this.transform.position;
                    spaceship.wantsToEmbark(this);
                }
            }
        }
        if (navmesh.enabled){
            agent.SetDestination(target);
        }
        float speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }
}
