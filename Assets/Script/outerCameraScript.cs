using UnityEngine;

public class outerCameraScript : MonoBehaviour
{
    public float elevation;
    public GameObject player;
    private Vector3 v;
    float x, y, z;


    void Start()
    {
        Cursor.visible = false;
        Vector3 offset = transform.position - player.transform.position;
        x = offset.x;
        y = offset.y;
        z = offset.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        v = x * player.transform.right + y * player.transform.up + z * player.transform.forward;
    }
    
    
    void LateUpdate(){
        transform.position = v + player.transform.position;  
        transform.LookAt(player.transform.position + elevation * player.transform.up,player.transform.up);
    }
}
