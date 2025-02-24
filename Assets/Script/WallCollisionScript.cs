using UnityEngine;

public class WallCollisionScript : MonoBehaviour
{
    public SpaceshipController spaceship;
    
    void Start(){
        spaceship = FindObjectOfType<SpaceshipController>();
    }
    
    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == spaceship.name){
            spaceship.getHit(10);
        }
    }
}
