using UnityEngine;

public class LandColliderScript : MonoBehaviour
{
    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {
        SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
        spaceshipController.canLand = true;
    }

    
    void OnTriggerExit (Collider other)
    {
        SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
        spaceshipController.canLand = false;
    }
}
