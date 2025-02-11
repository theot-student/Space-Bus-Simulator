using UnityEngine;

public class beamScript : MonoBehaviour
{
    public SpaceshipController ennemy;

    void OnCollisionEnter (Collision collision)
    {   
        if (collision.transform.gameObject.name == ennemy.name){
            //baisser vie ennemi
        } 
        Destroy(this);
    }

}
