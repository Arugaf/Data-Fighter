using System.Collections.Generic;
using Actors;
using Fighters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace InputModule.GameRelated {
    // todo: refactor
    public class FighterSelector : MonoBehaviour {
        private bool _alreadyHovered;
        private GameObject _currentHoveredTarget;
        private Actor _enemyActor;
        private List<GameObject> _fighters;
        private bool _isLookingForTarget;
        private Camera _mainCamera;
        private Actor _playerActor;
        private GameObject _previousTarget;
        private GameObject _selectedTarget;
        private bool _someFighterHovered;

        private void Start() {
            _playerActor = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
            _enemyActor = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Actor>();

            _fighters = new List<GameObject>();

            InputHandler.GotPrimaryMouseButtonDown += OnPrimaryMouseButtonDown;
            InputHandler.GotPrimaryMouseButtonUp += OnPrimaryMouseButtonUp;
            _mainCamera = Camera.main;

            var fighters = FindObjectsOfType<Fighter>();
            foreach (var fighter in fighters) _fighters.Add(fighter.GameObject());
        }

        private void Update() {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100f)) {
                if (!_someFighterHovered) return;

                GotFighterUnhovered?.Invoke(_previousTarget);

                _previousTarget = null;
                _currentHoveredTarget = null;
                _someFighterHovered = false;
                return;
            }

            if (!hit.transform) return;

            var go = hit.transform.GameObject();
            _currentHoveredTarget = _fighters.Contains(go) ? go : null;

            if (_isLookingForTarget) _selectedTarget = _currentHoveredTarget;

            if (!_currentHoveredTarget || _someFighterHovered) return;

            GotFighterHovered?.Invoke(_currentHoveredTarget);
            _previousTarget = _currentHoveredTarget;
            _someFighterHovered = true;
        }

        public static event UnityAction<GameObject> GotFighterHovered;
        public static event UnityAction<GameObject> GotFighterUnhovered;

        public static event UnityAction<GameObject> GotFighterClicked;

        private void OnPrimaryMouseButtonDown() {
            _isLookingForTarget = true;
        }

        private void OnPrimaryMouseButtonUp() {
            if (!_isLookingForTarget || !_currentHoveredTarget) return;
            GotFighterClicked?.Invoke(_currentHoveredTarget);
            _isLookingForTarget = false;
            _selectedTarget = null;
        }

        public IEnumerable<Fighter> GetFighters(bool isPlayer = false) {
            // [[likely]]
            return !isPlayer ? _enemyActor.GetFighters() : _playerActor.GetFighters();
        }
    }
}
