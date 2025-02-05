using UnityEngine;

public class InnerCameraScript : MonoBehaviour
{
    public Player player;
    public Transform head; // Assign this in the Inspector
    public float heightOffset = 0.09f; // Adjust this value to move the camera up

    void Update()
    {
        if (head != null)
        {
            // Move the camera slightly above the head
            transform.position = head.position + head.up * heightOffset;
            if (player.isDriving){
                transform.rotation = head.rotation;
            }
        }
        else
        {
            Debug.LogWarning("Head transform is not assigned!");
        }
    }
}