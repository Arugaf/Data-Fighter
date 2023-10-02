using System;
using Actors;
using Fighters;
using InputModule;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// todo: prefabing
// todo: statistics
// todo: target selecting
// todo: adaptive ui + anchoring
// todo: shield ability
// todo: random target selection ?
// todo: scaling

public class GameStateManager : MonoBehaviour {
    public enum GameStatus {
        Active,
        Paused,
        Win,
        Lose
    }

    public GameStatus currentGameStatus = GameStatus.Active;
    private GameScene _currentScene = GameScene.MainMenu;

    private PauseMenu _pauseMenu;

    private Actor _player;

    private bool _skillSelected;

    private void Awake() {
        DontDestroyOnLoad(this);

        Actor.GotActorDead += OnActorDead;
        InputHandler.GotEscapeKeyDown += GotPauseGame;

        Skill.GotSkillActivated += () => _skillSelected = true;
        Skill.GotUnselectAllSkills += () => _skillSelected = false;
    }

    private void Start() {
        _pauseMenu = FindObjectOfType<PauseMenu>();
        _pauseMenu.GameObject().SetActive(false);
    }

    public void LoadGame() {
        _currentScene = GameScene.FirstLevel;
        SceneManager.LoadScene("MainScene");
        currentGameStatus = GameStatus.Active;
        Time.timeScale = 1.0f;
    }

    public void LoadMenu() {
        _currentScene = GameScene.MainMenu;
        SceneManager.LoadScene("IntroScene");
        _pauseMenu.GameObject().SetActive(false);
        currentGameStatus = GameStatus.Paused;
    }

    public void LoadGameOverScene() {
        _currentScene = GameScene.End;
        SceneManager.LoadScene("EndScene");
        _pauseMenu.GameObject().SetActive(false);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Unpause() {
        Time.timeScale = 1.0f;
        currentGameStatus = GameStatus.Active;
        _pauseMenu.GameObject().SetActive(false);
    }

    private void GotPauseGame() {
        if (_currentScene is GameScene.MainMenu or GameScene.End) return;

        if (_skillSelected) return;

        Time.timeScale = currentGameStatus switch {
            GameStatus.Active => 0.0f,
            GameStatus.Paused => 1.0f,
            _ => Time.timeScale
        };

        currentGameStatus = currentGameStatus == GameStatus.Active ? GameStatus.Paused : GameStatus.Active;
        _pauseMenu.GameObject().SetActive(currentGameStatus == GameStatus.Paused);
    }

    public void ShowStatistic() {
        // ...
    }

    private void LoadNextLevel() {
        if (_currentScene == GameScene.End) {
            currentGameStatus = GameStatus.Win;
            LoadGameOverScene();
            return;
        }

        var scene = (int)_currentScene == Enum.GetNames(typeof(GameScene)).Length - 1
            ? _currentScene = 0
            : ++_currentScene;

        LoadScene(scene);
    }

    private void LoadScene(GameScene scene) {
        _currentScene = scene;

        string sceneName;

        switch (scene) {
            case GameScene.FirstLevel: {
                sceneName = "MainScene";
                break;
            }
            case GameScene.SecondLevel: {
                sceneName = "SecondLevel";
                break;
            }
            case GameScene.ThirdLevel: {
                sceneName = "ThirdLevel";
                break;
            }
            case GameScene.End: {
                sceneName = "MenuScene";
                break;
            }
            case GameScene.MainMenu:
            default: {
                sceneName = "IntroScene";
                break;
            }
        }

        Debug.Log(scene + " loaded");
        SceneManager.LoadScene(sceneName);
    }

    private void OnActorDead(Actor actor) {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
        if (actor == _player) {
            LoadGameOverScene();
            currentGameStatus = GameStatus.Lose;
            return;
        }

        LoadNextLevel();
    }

    private enum GameScene {
        MainMenu = 0,
        FirstLevel,
        SecondLevel,
        ThirdLevel,
        End
    }
}
