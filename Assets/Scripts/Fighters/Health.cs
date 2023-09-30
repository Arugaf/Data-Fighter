using UnityEngine;

namespace Fighters {
    public enum Status {
        Alive,
        Dead
    }

    public class Health : MonoBehaviour {
        [SerializeField] private int hp = 100;
        public Status Status { get; private set; } = Status.Alive;
        
        public void ApplyHpChange(int hpChange) {
            hp -= hpChange;
            Status = hp <= 0 ? Status.Dead : Status.Alive;
        }
    }
}
