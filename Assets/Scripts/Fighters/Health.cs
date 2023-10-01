using System;
using UI;
using UnityEngine;

namespace Fighters {
    public enum Status {
        Alive,
        Dead
    }

    public class Health : MonoBehaviour {
        [SerializeField] private HpBar bar; // todo: find in child or fighter prefab

        public int Hp { get; private set; } = 100;
        public Status Status { get; private set; } = Status.Alive;

        private void Start() {
            if (!bar) return;
            bar.MaxHp = Hp;
            bar.Hp = Hp;
            bar.UpdateState();
        }

        public void ApplyHpChange(int hpChange) {
            Hp -= hpChange;
            Status = Hp <= 0 ? Status.Dead : Status.Alive;

            if (!bar) return;
            
            bar.Hp = Hp > 0 ? Hp : 0;
            bar.UpdateState();
        }
    }
}
