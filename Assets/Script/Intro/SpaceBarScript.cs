using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class SpaceBarScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            textComponent.gameObject.SetActive(false);
        }
    }
}
