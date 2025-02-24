using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int index;

    void Awake()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        foreach (char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){
        if (index < lines.Length - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void newDialogue(string[] strings){
        StopAllCoroutines();
        gameObject.SetActive(true);
        lines = strings;
        textComponent.text = string.Empty;
        StartDialogue();
    }
}
