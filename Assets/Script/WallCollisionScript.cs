using UnityEngine;

public class WallCollisionScript : MonoBehaviour
{
    public SpaceshipController spaceship;

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == spaceship.name){
            spaceship.getHit(10);
        }
    }
}
