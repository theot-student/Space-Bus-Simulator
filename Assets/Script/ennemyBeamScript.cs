using UnityEngine;

public class ennemyBeamScript : MonoBehaviour
{
    public SpaceshipController spaceship;

    void OnCollisionEnter (Collision collision)
    {   
        if (collision.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = collision.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.getHit(10);
        } 
        Destroy(this);
    }

}
