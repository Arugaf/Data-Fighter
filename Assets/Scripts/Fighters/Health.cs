using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fighters {
    public enum Status {
        Alive,
        Dead
    }

    public class Health : MonoBehaviour {
        [SerializeField] private HpBar bar; // todo: find in child or fighter prefab
        
        [SerializeField] private int hp = 100;
        public Status Status { get; private set; } = Status.Alive;

        private void Start() {
            if (!bar) return;
            bar.MaxHp = hp;
            bar.Hp = hp;
            bar.UpdateState();
        }

        public void ApplyHpChange(int hpChange) {
            hp -= hpChange;
            Status = hp <= 0 ? Status.Dead : Status.Alive;

            if (!bar) return;
            
            bar.Hp = hp > 0 ? hp : 0;
            bar.UpdateState();
        }

        public int GetCurrentHp() {
            return hp;
        }
    }
}
