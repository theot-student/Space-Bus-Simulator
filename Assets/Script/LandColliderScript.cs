using UnityEngine;
using TMPro;
public class LandColliderScript : MonoBehaviour
{
    public SpaceshipController spaceship;
    public Vector3 landingAera = new Vector3(0,0.2f,0);
    public TextMeshProUGUI textOnScreen;

    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {   if (other.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.canLand = true;
            spaceshipController.landingPosition = landingAera;
            ShowMessage("Appuyez sur 'L' pour aterrir");
        }
    }

    
    void OnTriggerExit (Collider other)
    {
        if (other.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.canLand = false;
            HideMessage();
        }
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        textOnScreen.text = message;
        Invoke(nameof(HideMessage), duration);
    }

    void HideMessage()
    {
        textOnScreen.text = "";
    }
}
