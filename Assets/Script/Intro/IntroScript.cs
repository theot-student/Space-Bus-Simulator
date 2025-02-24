using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class IntroScript : MonoBehaviour
{
    public Animator animator;
    public DialogueScript dialogueScript;
    private bool startDialogue = false;
    // Update is called once per frame
    public TextMeshProUGUI textComponent;
    public float vitesse;
    void Update()
    {
        bool isStatic = animator.GetCurrentAnimatorStateInfo(0).IsName("HumanoidIdle");
        
        if (isStatic & !startDialogue) {
            dialogueScript.newDialogue(new string[] {"Bonjour prisonnier, ta peine vient d'être décidée par le capitaine.", 
            "Pour réparer tes erreurs, tu vas devoir travailler au compte du gouvernement galactique.", 
            "C'est pour ça qu'à partir de maintenant tu seras conducteur de spaceBus.", 
            "En tant que tel, ta mission sera de conduire les honnêtes gens à leur destination dans ton spaceBus.",
            "Il te faudra aussi faire attention aux pirates et aux astéroides.",
            "Sur ce, je te souhaites bien du courage."});
            textComponent.gameObject.SetActive(true);
            startDialogue = true;
        }
        if (!dialogueScript.gameObject.activeSelf & startDialogue){
            SceneManager.LoadScene("SampleScene");
        }
    }

    void FixedUpdate(){
        bool isStatic = animator.GetCurrentAnimatorStateInfo(0).IsName("HumanoidIdle");
        if (!isStatic) {
            transform.position += vitesse * transform.forward;
        }
    }
}
