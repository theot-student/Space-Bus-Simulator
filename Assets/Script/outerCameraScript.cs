using UnityEngine;

public class outerCameraScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    
    void Start()
    {
        offset = transform.postion - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        Quaternion rotation = player.transform.rotation;
        Vector3 v = rotation * offset;
        transform.position = v + player.transform.position;  
    }
    
    
    void LateUpdate(){
        

    }
}
