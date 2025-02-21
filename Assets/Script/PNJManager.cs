using UnityEngine;
using System.Collections.Generic;

public class PNJManager : MonoBehaviour
{
    [Header("PNJ Settings")]
    public GameObject pnjPrefab; // Prefab of the PNJ
    public Transform spawnPoint; // Where PNJs will spawn
    public int numberOfPNJs = 5; // Number of PNJs to spawn

    [Header("Customization Options")]
    public Material[] skinMaterials; // Different skins/textures
    public Vector2 heightRange = new Vector2(0.08f, 0.12f); // Min and max height
    public Vector2 corpulenceRange = new Vector2(0.08f, 0.13f); // Min and max body width

    void Start()
    {
        // SpawnP/NJs();
    }

    void SpawnPNJs()
    {
        for (int i = 0; i < numberOfPNJs; i++)
        {
            // Spawn at random position near spawnPoint
            GameObject newPNJ = Instantiate(pnjPrefab, spawnPoint.position, Quaternion.identity);
            CustomizePNJ(newPNJ);
        }
    }

    void CustomizePNJ(GameObject pnj)
    {
        // Get the PNJ character model
        Transform model = pnj.transform.GetChild(0); // Assuming the model is the first child

        // Randomize height and corpulence
        float randomHeight = Random.Range(heightRange.x, heightRange.y);
        float randomWidth = Random.Range(corpulenceRange.x, corpulenceRange.y);
        model.localScale = new Vector3(randomWidth, randomHeight, randomWidth);

        // Randomize skin texture
        Renderer renderer = model.GetComponent<Renderer>();
        if (renderer != null && skinMaterials.Length > 0)
        {
            renderer.material = skinMaterials[Random.Range(0, skinMaterials.Length)];
        }

        // Apply random animation (optional)
        Animator animator = pnj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat("Speed", Random.Range(0.5f, 1.5f)); // Example: different movement speeds
        }
    }
}
