using UnityEngine;

public class DriverSeat : MonoBehaviour
{
    public Transform spaceship; // Assign the spaceship in the Inspector
    public Vector3 axisCorrection = new Vector3(0, 0, 90); // Adjust rotation correction

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        if (spaceship == null)
        {
            Debug.LogError("Spaceship reference is missing in DriverSeat script!");
            return;
        }

        // Store initial position & rotation relative to spaceship
        initialLocalPosition = spaceship.InverseTransformPoint(transform.position);

        // Apply correction to the rotation if spaceship axes are misaligned
        Quaternion correction = Quaternion.Euler(axisCorrection);
        initialLocalRotation = Quaternion.Inverse(spaceship.rotation * correction) * transform.rotation;
    }

    void LateUpdate()
    {
        if (spaceship != null)
        {
            // Keep seat fixed inside spaceship
            transform.position = spaceship.TransformPoint(initialLocalPosition);

            // Apply corrected rotation
            Quaternion correction = Quaternion.Euler(axisCorrection);
            transform.rotation = spaceship.rotation * correction * initialLocalRotation;
        }
    }
}