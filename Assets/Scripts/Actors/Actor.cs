using System;
using System.Collections.Generic;
using System.Linq;
using Fighters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Actors {
    public class Actor : MonoBehaviour { // todo: inherit
        [SerializeField] private Fighter[] fighters;
        private List<Fighter> _fightersInternal = new List<Fighter>();
        
        private void Start() {
            Fighter.GotFighterDeath += OnFighterDeath;
            foreach (var f in fighters) {
                _fightersInternal.Add(f);
            }
        }

        public static event UnityAction GotPlayerDeath;

        private void OnFighterDeath(Fighter fighter) {
            if (!fighters.Contains(fighter)) return;
            Debug.Log("Got Fighter Dead:" + fighter.fighterName);
            Destroy(fighter.GameObject()); // todo: delete

            fighters[Array.IndexOf(fighters, fighter)] = null;
            _fightersInternal.Remove(fighter);

            if (_fightersInternal.Count > 0) return;
            
            GotPlayerDeath?.Invoke();
            Debug.Log("Got Actor " + name + " Dead");
        }

        public Fighter[] GetFighters() {
            return fighters;
        }
        
        public List<Fighter> GetFighterList() {
            return _fightersInternal;
        }
    }
}
