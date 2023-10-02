using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class StatisticsUI : MonoBehaviour {
        private GameStateManager _gameStateManager;
        private StatisticsObserver _statisticsObserver;
        private TextMeshProUGUI _statisticsText;

        public static event UnityAction GotGoBackToEndMenu;

        private void Start() {
            _gameStateManager = FindAnyObjectByType<GameStateManager>();
            _statisticsObserver = FindObjectOfType<StatisticsObserver>();
            _statisticsText = GetComponent<TextMeshProUGUI>();

            foreach (var kv in _statisticsObserver.GetDamageTakenStatistics()) {
                _statisticsText.text += kv.Key + " получил " + kv.Value + " урона\n";
            }

            // EndMenu.GotShowStatistics += OnShowStatistics;
        }

        private void Back() {
            gameObject.SetActive(false);
            GotGoBackToEndMenu?.Invoke();
        }

        private void OnShowStatistics() {
            gameObject.SetActive(true);
        }
    }
}
