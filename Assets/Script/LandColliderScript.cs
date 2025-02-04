using UnityEngine;

public class LandColliderScript : MonoBehaviour
{
    public SpaceshipController spaceship;
    public Vector3 landingAera = new Vector3(0,0.2f,0);
    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {   if (other.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.canLand = true;
            spaceshipController.landingPosition = landingAera;
        }
    }

    
    void OnTriggerExit (Collider other)
    {
        if (other.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.canLand = false;
        }
    }
}
