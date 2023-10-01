using System.Collections;
using AI;
using UnityEngine;
using UnityEngine.Events;

namespace Fighters {
    [RequireComponent(typeof(Health))]
    public class Fighter : MonoBehaviour {
        public string fighterName;
        [SerializeField] private bool isEnemyFighter;

        [SerializeField] private float autoCooldown = 1.0f;

        [SerializeField] private int autoDamagePower;
        [SerializeField] private int healPower;

        private BattleMind _ai;

        // private bool _allowedToHover = true;

        private Health _hp;

        private SpriteRenderer _renderer;

        public bool IsAlive { get; private set; }
        public int Hp { get; private set; }

        private void Awake() {
            _hp = GetComponent<Health>();
            _renderer = GetComponent<SpriteRenderer>();

            IsAlive = _hp.Status == Status.Alive;
            Hp = _hp.Hp;

            _ai = GameObject.FindGameObjectWithTag("Meta").GetComponent<BattleMind>();
        }

        private void Start() {
            StartCoroutine(AutoDamager());

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
            if (!fighter) return;
            fighter.ApplyDamage(autoDamagePower);
        }

        public void DoHeal(Fighter fighter) {
            fighter.ApplyHeal(healPower);
        }

        public void ApplyDamage(int damage) {
            _hp.ApplyHpChange(damage);
            Hp = _hp.Hp;
            Debug.Log("Fighter: " + name /*todo*/ + " has " + Hp + " left");

            if (_hp.Status != Status.Dead) return;

            GotFighterDeath?.Invoke(this);
            IsAlive = false;
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

        private IEnumerator AutoDamager() {
            while (true) {
                DoDamage(_ai.GetTarget(isEnemyFighter));
                yield return new WaitForSeconds(autoCooldown);
            }
        }
    }
}
