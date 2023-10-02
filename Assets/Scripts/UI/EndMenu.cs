using TMPro;
using UnityEngine;

namespace UI {
    public class EndMenu : MonoBehaviour {
        private TextMeshProUGUI _gameOverTitle;
        private GameStateManager _gameStateManager;

        private void Start() {
            _gameStateManager = FindAnyObjectByType<GameStateManager>();
            _gameOverTitle = GameObject.FindGameObjectWithTag("GameOverTitle").GetComponent<TextMeshProUGUI>();

            if (!_gameStateManager || !_gameOverTitle) return;

            _gameOverTitle.text = _gameOverTitle.text.Replace("${placeholder}",
                _gameStateManager.currentGameStatus == GameStateManager.GameStatus.Lose
                    ? "Вы проиграли!"
                    : "Вы Победили!");
        }

        public void LoadMainMenu() {
            _gameStateManager.LoadMenu();
        }

        public void Exit() {
            _gameStateManager.Exit();
        }

        public void ShowStatistics() {
            // todo
        }
    }
}
