using UnityEngine;

public class LandColliderScript : MonoBehaviour
{
    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {
        other.GameObject
    }

    
    void OnTriggerExit (Collider other)
    {
        Debug.Log ("A collider has exited the DoorObject trigger");
    }
}
