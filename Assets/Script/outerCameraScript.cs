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

    public float distanceBehind = 1f; // How far behind the player the camera should be
    public float heightOffset = 0.55f; // How high the camera should be

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the mouse to the center
        Cursor.visible = false;
        offset = new Vector3(0f, heightOffset, -distanceBehind);
    }
    
    void FixedUpdate(){
        if (player.isDriving){
            v = x * spaceship.transform.right + y * spaceship.transform.up + z * spaceship.transform.forward;
        }
        else{
            Vector3 targetPosition = player.transform.position - player.transform.forward * distanceBehind + Vector3.up * heightOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f); // Smooth follow
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
