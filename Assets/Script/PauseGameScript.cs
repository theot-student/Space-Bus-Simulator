using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class PauseGameScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public List<AudioClip> audioClips;
    public AudioSource audio;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame(); 
        }
    }

    public void PauseGame ()
    {
        if(gameIsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ResumeGame () {
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame (){
        Debug.Log("GoodBye");
        Application.Quit();
    }

    public void RestartGame () {
        PauseGame();
        SceneManager.LoadScene("SampleScene");
    }

       public void PlayPauseMusic(){
        if (audio.isPlaying){
            audio.Stop();
        } else {
            audio.Play();
        }
    }

    public void NextMusic(){
        if (audio.isPlaying){
            audio.Stop();
            AudioClip currentClip = audio.clip;
            int index = audioClips.IndexOf(currentClip);
            AudioClip newClip;
            if (index + 1 >= audioClips.Count) {
                newClip = audioClips[0];
            } else {
                newClip = audioClips[index + 1];
            }
            audio.clip = newClip;
            audio.Play();
        }
    }

    public void PreviousMusic(){
        if (audio.isPlaying){
            audio.Stop();
            AudioClip currentClip = audio.clip;
            int index = audioClips.IndexOf(currentClip);
            AudioClip newClip;
            if (index <= 0) {
                newClip = audioClips[audioClips.Count - 1];
            } else {
                newClip = audioClips[index - 1];
            }
            audio.clip = newClip;
            audio.Play();
        }
    }
}

