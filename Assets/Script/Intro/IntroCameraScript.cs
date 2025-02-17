using UnityEngine;

public class IntroCameraScript : MonoBehaviour
{
    public GameObject captain;
    private Vector3 offset;



    void Start()
    {
        offset = transform.position - captain.transform.position;
    }
    
    void Update()
    {
        transform.position = captain.transform.position + offset;
    }
}
