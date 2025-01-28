using UnityEngine;

public class outerCameraScript : MonoBehaviour
{
    public float elevation;
    public Player player;
    public GameObject spaceship;
    private Vector3 v;
    public float x = -2.5f;
    public float y = 1f;
    public float z = 0f; 

    Vector3 offset;

    void Start()
    {
        Cursor.visible = false;
        offset = new Vector3(0f,0.55f,-1f);
    }
    
    void FixedUpdate(){
        if (player.isDriving){
            v = x * spaceship.transform.right + y * spaceship.transform.up + z * spaceship.transform.forward;
        }
        else{
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform);
        }
    }
    
    
    void LateUpdate(){
        if (player.isDriving){
            transform.position = v + spaceship.transform.position;  
            transform.LookAt(spaceship.transform.position + elevation * spaceship.transform.up,spaceship.transform.up);
        }
       
    }
}
