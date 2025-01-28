using UnityEngine;

public class innerCameraScript : MonoBehaviour
{

    public GameObject player;
    public GameObject spaceship;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + transform.up;
    }
}
