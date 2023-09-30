using System.Collections.Generic;
using Fighters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace InputModule.GameRelated {
    public class FighterSelector : MonoBehaviour {
        private bool _alreadyHovered;
        private List<GameObject> _fighters;

        private bool _isLookingForTarget;

        private Camera _mainCamera;
        private GameObject _previousTarget;

        private bool _someFighterHovered;
        private GameObject _target;

        private void Start() {
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
                _someFighterHovered = false;
                return;
            }

            if (!hit.transform) return;

            var go = hit.transform.GameObject();
            _target = _fighters.Contains(go) ? go : null;

            if (_target) Debug.Log(_target.name);

            if (!_target || _someFighterHovered) return;

            GotFighterHovered?.Invoke(_target);
            _previousTarget = _target;
            _someFighterHovered = true;

            if (!_isLookingForTarget) return;
            Debug.Log("banana");
            GotFighterSelected?.Invoke(_target);
        }

        public static event UnityAction<GameObject> GotFighterHovered;
        public static event UnityAction<GameObject> GotFighterUnhovered;

        public static event UnityAction<GameObject> GotFighterSelected;
        public static event UnityAction<GameObject> GotFighterClicked;

        private void OnPrimaryMouseButtonDown() {
            _isLookingForTarget = true;
        }

        private void OnPrimaryMouseButtonUp() {
            if (_isLookingForTarget && _target) GotFighterClicked?.Invoke(_target);
            Debug.Log("potato");
            _isLookingForTarget = false;
        }
    }
}
