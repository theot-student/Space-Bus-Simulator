using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWonScript : MonoBehaviour
{   
    public DialogueScript dialogueScript;
    public GameObject panel;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.Play();
        dialogueScript.newDialogue(new string[] { "Félicitations, vous avez réussi à protéger votre SpaceBus et tous ses passagers, vous êtes maintenant un homme libre !" });
        Invoke("transition", 10f);
    }

    void transition()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
