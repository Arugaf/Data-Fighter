using UnityEngine;
using UnityEngine.Events;

namespace Fighters {
    [RequireComponent(typeof(Health))]
    public class Fighter : MonoBehaviour {
        public string fighterName;

        [SerializeField] private int damagePower;
        [SerializeField] private int healPower;

        // private bool _allowedToHover = true;

        private Health _hp;

        private SpriteRenderer _renderer;

        private void Start() {
            _hp = GetComponent<Health>();
            _renderer = GetComponent<SpriteRenderer>();

            /*FighterSelector.GotFighterHovered += Hover;
            FighterSelector.GotFighterUnhovered += Unhover;
            FighterSelector.GotFighterSelected += Selected;
            FighterSelector.GotFighterClicked += Clicked;*/
        }

        public void OnDestroy() {
            /*FighterSelector.GotFighterHovered -= Hover;
            FighterSelector.GotFighterUnhovered -= Unhover;
            FighterSelector.GotFighterSelected -= Selected;
            FighterSelector.GotFighterClicked -= Clicked;*/
        }

        // public static event UnityAction<Fighter> GotFighterClicked; 

        public static event UnityAction<Fighter> GotFighterDeath;

        public void DoDamage(Fighter fighter) {
            fighter.ApplyDamage(damagePower);
        }

        public void DoHeal(Fighter fighter) {
            fighter.ApplyHeal(healPower);
        }

        public void ApplyDamage(int damage) {
            _hp.ApplyHpChange(damage);
            if (_hp.Status == Status.Dead) GotFighterDeath?.Invoke(this);
        }

        public void ApplyHeal(int heal) {
            _hp.ApplyHpChange(-heal);
        }

        private void Hover(GameObject go) {
            /*if (go != gameObject || !_allowedToHover) return;
            _renderer.color = Color.cyan;*/
        }

        private void Unhover(GameObject go) {
            /*if (go != gameObject || !_allowedToHover) return;
            _renderer.color = Color.white;*/
        }

        private void Selected(GameObject go) {
            /*if (go != gameObject) return;
            _renderer.color = Color.red;
            _allowedToHover = false;*/
        }

        private void Clicked(GameObject go) {
            /*if (go != gameObject) {
                _renderer.color = Color.white;
                return;
            }

            _renderer.color = Color.yellow;
            _allowedToHover = false;
            GotFighterClicked?.Invoke(this); // todo

            ApplyDamage(damagePower);*/
        }
    }
}
