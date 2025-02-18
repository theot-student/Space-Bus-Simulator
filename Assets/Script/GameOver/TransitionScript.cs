using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionScript : MonoBehaviour
{
    public GameObject spaceShip;
    public DialogueScript dialogueScript;

    // Update is called once per frame
    void Update()
    {
        if (!dialogueScript.gameObject.activeSelf & !spaceShip.activeSelf){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
