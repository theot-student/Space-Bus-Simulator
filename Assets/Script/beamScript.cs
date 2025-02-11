using UnityEngine;

public class beamScript : MonoBehaviour
{
    public SpaceshipController ennemy;

    void OnCollisionEnter (Collision collision)
    {   
        ennemyScript es = collision.gameObject.GetComponent<ennemyScript>();
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
