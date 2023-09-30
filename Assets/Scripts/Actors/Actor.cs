using System;
using System.Linq;
using Fighters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public class Actor : MonoBehaviour { // todo: inherit
        [SerializeField] private Fighter[] fighters;
        
        private int _fightersCount;

        private void Start() {
            Fighter.GotFighterDeath += OnFighterDeath;
        }

        private void Update() {
            _fightersCount = fighters.Length;
        }

        public static event UnityAction GotPlayerDeath;

        private void OnFighterDeath(Fighter fighter) {
            if (!fighters.Contains(fighter)) return;
            Debug.Log("Got Fighter Dead:" + fighter.fighterName);
            Destroy(fighter.GameObject()); // todo: delete

            fighters[Array.IndexOf(fighters, fighter)] = null;
            --_fightersCount;

            if (_fightersCount <= 0) GotPlayerDeath?.Invoke();
            Debug.Log("Got Player Dead");
        }

        public Fighter[] GetFighters() {
            return fighters;
        }
    }
}
