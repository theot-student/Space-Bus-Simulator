using UnityEngine;
public class GameOverScript : MonoBehaviour
{   
    public bool explode;
    public bool exploded;
    public DialogueScript dialogueScript;
    public AudioSource audioSource;
    void Start()
    {
        exploded = false;
        explode = false;
        Invoke("Explosion", 2f);
        Invoke("PlayExplosion", 1.8f);
    }

    void Update()
    {
        if (explode & !exploded) {
            exploded = true;
            dialogueScript.newDialogue(new string[] {"Malheureusement, vous n'avez pas réussi à protéger votre SpaceBus et tous ses passagers."});
            gameObject.SetActive(false);
        }
    }

    void Explosion() {
        explode =true;
    }

    void PlayExplosion() {
        audioSource.Play();
    }
}
