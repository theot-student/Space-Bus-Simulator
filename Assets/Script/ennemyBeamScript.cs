using UnityEngine;

public class ennemyBeamScript : MonoBehaviour
{
    public SpaceshipController spaceship;


    void OnCollisionEnter (Collision collision)
    {   
        SpaceshipController spaceshipController = collision.gameObject.GetComponent<SpaceshipController>();
        ennemyScript es = collision.gameObject.GetComponent<ennemyScript>();
        if (spaceshipController != null)
        {
            spaceshipController.getHit(10);
        }
        if (es == null){
            Destroy(gameObject);
        }
        
    }

}
