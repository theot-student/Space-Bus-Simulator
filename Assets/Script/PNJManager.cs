using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PNJManager : MonoBehaviour
{
    [Header("PNJ Settings")]
    public PNJScript pnjPrefab; // Prefab of the PNJ
    public Transform spawnPoint; // Where PNJs will spawn
    public int numberOfPNJs = 5; // Number of PNJs to spawn

    [Header("Customization Options")]
    public Material[] skinMaterials; // Different skins/textures

    void Start()
    {
        SpawnPNJs();
    }

   void SpawnPNJs()
    {
        for (int i = 0; i < numberOfPNJs; i++)
        {
            Debug.Log(i);
            // Spawn at random position near spawnPoint
            PNJScript newPNJ = Instantiate(pnjPrefab, spawnPoint.position, Quaternion.identity);
            newPNJ.pnjManager = this;
            newPNJ.spaceship = this.transform.parent.GetComponent<SpaceStationScript>().currentlydockedSpaceship;
            CustomizePNJ(newPNJ);

            // Wait for half a second before spawning the next PNJ
            // yield return new WaitForSeconds(0.5f);
        }
    }

    void CustomizePNJ(PNJScript pnj)
    {
        // Get the PNJ character model
        Transform model = pnj.transform.GetChild(0); // Assuming the model is the first child

        // Randomize skin texture
        Renderer renderer = model.GetComponent<Renderer>();
        if (renderer != null && skinMaterials.Length > 0)
        {
            renderer.material = skinMaterials[Random.Range(0, skinMaterials.Length)];
        }

    }
}
