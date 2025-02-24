using UnityEngine;
using TMPro;
public class LandColliderScript : MonoBehaviour
{
    public SpaceshipController spaceship;
    public Vector3 landingAera;
    public TextMeshProUGUI textOnScreen;

    void Start(){
        landingAera = transform.position + new Vector3(0f,2.6f,0f);   
    }

    // “other” refers to the collider on the GameObject inside this trigger
    void OnTriggerEnter (Collider other)
    {   if (other.transform.gameObject.name == spaceship.name){
            SpaceshipController spaceshipController = other.gameObject.GetComponent<SpaceshipController>();
            spaceshipController.canLand = true;
            spaceshipController.landingPosition = landingAera;
            spaceshipController.possibleSpaceStation = this.transform.parent.GetComponent<SpaceStationScript>();
            ShowMessage("Appuyez sur 'L' pour atterrir");
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
