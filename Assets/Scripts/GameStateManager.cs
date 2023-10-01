using System;
using Actors;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {
    private Scene _currentScene = Scene.MainMenu;

    private Actor _player;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void Start() {
        Actor.GotActorDead += OnActorDead;
    }

    public void LoadGame() {
        _currentScene = Scene.FirstLevel;
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMenu() {
        _currentScene = Scene.MainMenu;
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
}
