using UnityEngine;
using System.Collections;
public class ExplosionLightScript : MonoBehaviour
{
    public Light explosionLight;
    public float fadeDuration = 0.5f; // Durée d'atténuation

    void start(){
        explosionLight.enabled = false;
    }

    public void Explosion()
    {
        // Lancer la réduction progressive de l’intensité
        StartCoroutine(FadeLight());
    }

    IEnumerator FadeLight()
    {   
        explosionLight.enabled = true;
        float startIntensity = explosionLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            explosionLight.intensity = Mathf.Lerp(startIntensity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Désactiver la lumière après l’animation
        explosionLight.enabled = false;
    }
}