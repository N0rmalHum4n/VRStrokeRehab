using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings() {
        SceneManager.LoadScene("SettingsScene");    
    }

    public void OpenInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void QuitGame() { 
        Application.Quit();
    }
}
