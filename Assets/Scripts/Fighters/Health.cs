using UnityEngine;

namespace Fighters {
    public enum Status {
        Alive,
        Dead
    }

    public class Health : MonoBehaviour {
        public int Hp { get; private set; }= 100;
        public Status Status { get; private set; } = Status.Alive;
        
        public void ApplyHpChange(int hpChange) {
            Hp -= hpChange;
            Status = Hp <= 0 ? Status.Dead : Status.Alive;
        }
    }
}
