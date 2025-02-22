using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{   
    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame () {
        SceneManager.LoadScene("Intro");
    }

    public void QuitGame (){
        Debug.Log("GoodBye");
        Application.Quit();
    }

 
}
