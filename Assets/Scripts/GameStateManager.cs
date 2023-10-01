using System;
using Actors;
using InputModule;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo: menu endgame
// todo: statistics
// todo: skill cancel
// todo: target selecting
// todo: adaptive ui + anchoring

public class GameStateManager : MonoBehaviour {
    private GameStatus _currentGameStatus = GameStatus.Active;
    private Scene _currentScene = Scene.MainMenu;

    private PauseMenu _pauseMenu;

    private Actor _player;

    private void Awake() {
        DontDestroyOnLoad(this);

        _pauseMenu = FindObjectOfType<PauseMenu>();
        _pauseMenu.GameObject().SetActive(false);
    }

    private void Start() {
        Actor.GotActorDead += OnActorDead;
        InputHandler.GotEscapeKeyDown += GotPauseGame;
    }

    public void LoadGame() {
        _currentScene = Scene.FirstLevel;
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMenu() {
        _currentScene = Scene.MainMenu;
        SceneManager.LoadScene("IntroScene");
        _pauseMenu.GameObject().SetActive(false);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Unpause() {
        Time.timeScale = 1;
        _currentGameStatus = GameStatus.Active;
        _pauseMenu.GameObject().SetActive(false);
    }

    private void GotPauseGame() {
        if (_currentScene is Scene.MainMenu or Scene.End) return;

        Time.timeScale = _currentGameStatus switch {
            GameStatus.Active => 0,
            GameStatus.Paused => 1,
            _ => Time.timeScale
        };

        _currentGameStatus = _currentGameStatus == GameStatus.Active ? GameStatus.Paused : GameStatus.Active;
        _pauseMenu.GameObject().SetActive(_currentGameStatus == GameStatus.Paused);
    }

    public void ShowStatistic() {
        // ...
    }

    private void LoadNextLevel() {
        var scene = (int)_currentScene == Enum.GetNames(typeof(Scene)).Length - 1
            ? _currentScene = 0
            : ++_currentScene;

        LoadScene(scene);
    }

    private void LoadScene(Scene scene) {
        _currentScene = scene;

        string sceneName;

        switch (scene) {
            case Scene.FirstLevel: {
                sceneName = "MainScene";
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
                break;
            }
            case Scene.SecondLevel: {
                sceneName = "SecondLevel";
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
                break;
            }
            case Scene.ThirdLevel: {
                sceneName = "ThirdLevel";
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
                break;
            }
            case Scene.End: {
                sceneName = "MenuScene";
                break;
            }
            case Scene.MainMenu:
            default: {
                sceneName = "IntroScene";
                break;
            }
        }

        Debug.Log(scene + " loaded");
        SceneManager.LoadScene(sceneName);
    }

    private void OnActorDead(Actor actor) {
        if (actor == _player)
            // game over
            return;
        LoadNextLevel();
    }

    private enum Scene {
        MainMenu = 0,
        FirstLevel,
        SecondLevel,
        ThirdLevel,
        End
    }

    private enum GameStatus {
        Active,
        Paused
    }
}
