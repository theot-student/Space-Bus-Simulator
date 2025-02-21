using UnityEngine;

public class PNJScript : MonoBehaviour
{
    public SpaceshipController spaceship;
    public PNJManager pnjManager;
    public bool isSitting = false;
    public bool isAtTargetStation=false;
    public UnityEngine.AI.NavMeshAgent agent;
    public Vector3 target;
    private Vector3 spaceShipOffset = new Vector3(-0.13f, 0, -0.4f);
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSitting){
            target = this.transform.position;
        }
        else{
            if (isAtTargetStation){
                target = pnjManager.transform.position;
            }
            else{
                target = spaceship.transform.position + spaceShipOffset;
            }
        }
        agent.SetDestination(target);
        float speed = agent.velocity.magnitude; // Use magnitude of velocity
        animator.SetFloat("speed", speed);
    }
}
