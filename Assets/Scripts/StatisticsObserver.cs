using System;
using System.Collections.Generic;
using System.Linq;
using Fighters;
using UnityEngine;

public class StatisticsObserver : MonoBehaviour {
    private Dictionary<string, int> _damageTaken;

    [SerializeField] private string[] fighters;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        // foreach (var fighter in fighters) {
        //     _damageTaken.Add(fighter, 0);
        // }
    }

    public void WriteAppliedDamage(Fighter fighter, int damage) {
        if (!fighters.Contains(fighter.name)) return;

        _damageTaken[fighter.name] += damage;
    }

    public Dictionary<string, int> GetDamageTakenStatistics() {
        return _damageTaken;
    }
}
