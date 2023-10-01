using System;
using System.Collections.Generic;
using System.Linq;
using Fighters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public class Actor : MonoBehaviour {
        // todo: inherit
        [SerializeField] private Fighter[] fighters;
        private readonly List<Fighter> _fightersInternal = new();

        private void Awake() {
            foreach (var f in fighters) _fightersInternal.Add(f);
        }

        private void Start() {
            Fighter.GotFighterDeath += OnFighterDeath;
        }

        public static event UnityAction<Actor> GotActorDead;

        private void OnFighterDeath(Fighter fighter) {
            if (!fighters.Contains(fighter)) return;
            Debug.Log("Got Fighter Dead:" + fighter.name);
            fighter.GameObject().SetActive(false);

            fighters[Array.IndexOf(fighters, fighter)] = null;
            _fightersInternal.Remove(fighter);

            if (_fightersInternal.Count > 0) return;

            GotActorDead?.Invoke(this);
            Debug.Log("Got Actor " + name + " Dead");
        }

        public Fighter[] GetFighters() {
            return fighters;
        }

        public IEnumerable<Fighter> GetFighterList() {
            return _fightersInternal;
        }
    }
}
