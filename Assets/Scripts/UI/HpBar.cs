using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HpBar : MonoBehaviour {
        private Slider _slider;
        public int Hp { get; set; }
        public int MaxHp { get; set; }

        private void Awake() {
            _slider = GetComponent<Slider>();
        }

        public void UpdateState() {
            _slider.maxValue = MaxHp;
            _slider.value = Hp; // todo: fix underflow ui bug
        }
    }
}
