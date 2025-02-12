using UnityEngine;

public class EnnemyBeamScript : MonoBehaviour
{
    public SpaceshipController spaceship;


    void OnCollisionEnter (Collision collision)
    {   
        SpaceshipController spaceshipController = collision.gameObject.GetComponent<SpaceshipController>();
        EnnemyScript es = collision.gameObject.GetComponent<EnnemyScript>();
        if (spaceshipController != null)
        {
            spaceshipController.getHit(10);
        }
        if (es == null){
            Destroy(gameObject);
        }
        
    }

}
