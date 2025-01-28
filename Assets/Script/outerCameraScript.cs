using UnityEngine;

public class outerCameraScript : MonoBehaviour
{
    public float elevation;
    public GameObject player;
    private Vector3 v;
    public float x = -2.5f;
    public float y = 1f;
    public float z = 0f;


    void Start()
    {
        Cursor.visible = false;
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
