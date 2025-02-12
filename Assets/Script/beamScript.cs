using UnityEngine;

public class BeamScript : MonoBehaviour
{
    public SpaceshipController ennemy;

    void OnCollisionEnter (Collision collision)
    {   
        EnnemyScript es = collision.gameObject.GetComponent<EnnemyScript>();
        SpaceshipController spaceshipController = collision.gameObject.GetComponent<SpaceshipController>();
        if (es != null)
        {
            es.getHit(10);
        }
        if (spaceshipController == null) {
            Destroy(gameObject);
        }
        
    }

}
