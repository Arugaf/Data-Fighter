using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
    private enum CurrentScene {
        MainMenu,
        FirstLevel,
        End
    }

    private CurrentScene _currentScene = CurrentScene.MainMenu;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void LoadGame() {
        _currentScene = CurrentScene.FirstLevel;
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMenu() {
        _currentScene = CurrentScene.MainMenu;
        SceneManager.LoadScene("IntroScene");
    }

    public void Exit() {
        Application.Quit();
    }

    public void PauseGame() {
        // ...
    }

    public void ShowStatistic() {
        // ...
    }

    public void LoadNextLevel() {
        // ...
    }
    
    
}
