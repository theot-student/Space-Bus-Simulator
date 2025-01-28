using UnityEngine;

public class InnerCameraScript : MonoBehaviour
{
    public GameObject player;
    public Transform head; // Assign this in the Inspector
    public float heightOffset = 0.09f; // Adjust this value to move the camera up

    void Update()
    {
        if (head != null)
        {
            // Move the camera slightly above the head
            transform.position = head.position + Vector3.up * heightOffset;
        }
        else
        {
            Debug.LogWarning("Head transform is not assigned!");
        }
    }
}