using System.Collections;
using System.Linq;
using Actors;
using InputModule;
using InputModule.GameRelated;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Fighters {
    public class Skill : MonoBehaviour {
        [SerializeField] private bool heal;

        [SerializeField] private int skillPower;
        [SerializeField] private bool aoe;
        [SerializeField] private float cooldown;

        [SerializeField] private Button button;

        [SerializeField] private Actor actor;

        [SerializeField] private Color selectColor = Color.cyan;
        [SerializeField] private Color idleColor = Color.white;
        [SerializeField] private Color disabledColor = Color.black;

        private Image _buttonImage;
        private Fighter _fighter;

        private FighterSelector _fighterSelector;

        private float _nextTimeSkillAvailable;
        private bool _selected;

        private bool _waitingForTarget;

        private void Start() {
            _fighter = GetComponent<Fighter>();
            _fighterSelector = GameObject.FindGameObjectWithTag("Meta").GetComponent<FighterSelector>();

            _buttonImage = button.GetComponent<Image>();
            _buttonImage.color = idleColor;

            GotUnselectAllSkills += OnUnselectAllSkills;
            InputHandler.GotEscapeKeyDown += () => {
                GotUnselectAllSkills?.Invoke();
                _selected = false;
                _buttonImage.color = idleColor;
            };

            Fighter.GotFighterDeath += fighter => {
                GotUnselectAllSkills?.Invoke();
                if (fighter != _fighter) return;
                button.interactable = false;
                _buttonImage.color = disabledColor;
            };
        }

        public static event UnityAction GotSkillActivated;

        public void InvokeSkill() {
            if (_selected) {
                GotUnselectAllSkills?.Invoke();
                return;
            }

            GotUnselectAllSkills?.Invoke();

            if (Time.time < _nextTimeSkillAvailable) return;
            // [[likely]]
            if (!aoe) {
                _selected = true;
                _waitingForTarget = true;
                GotSkillActivated?.Invoke();
                FighterSelector.GotFighterClicked += OnTargetSelected;
                _buttonImage.color = selectColor;
                return;
            }

            // aoe
            foreach (var fighter in _fighterSelector.GetFighters(heal))
                if (fighter)
                    // [[likely]]
                    if (!heal)
                        fighter.ApplyDamage(skillPower);
                    else
                        fighter.ApplyHeal(skillPower);
            StartCoroutine(CooldownWaiter());
        }

        public static event UnityAction GotUnselectAllSkills;

        private void OnTargetSelected(GameObject go) {
            var fighter = go.GetComponent<Fighter>();
            var fighters = actor.GetFighterList();
            var fightersArray = fighters as Fighter[] ?? fighters.ToArray();
            if ((!heal && fightersArray.Contains(fighter)) || (heal && !fightersArray.Contains(fighter))) return;

            _waitingForTarget = false;
            // [[likely]]
            if (!heal)
                fighter.ApplyDamage(skillPower);
            else
                fighter.ApplyHeal(skillPower);
            FighterSelector.GotFighterClicked -= OnTargetSelected;

            _selected = false;
            _buttonImage.color = idleColor;
            StartCoroutine(CooldownWaiter());
            GotUnselectAllSkills?.Invoke();
        }

        private void OnUnselectAllSkills() {
            if (_waitingForTarget) FighterSelector.GotFighterClicked -= OnTargetSelected;

            _waitingForTarget = false;
            _buttonImage.color = _buttonImage.color == disabledColor ? disabledColor : idleColor;
            _selected = false;
        }

        private IEnumerator CooldownWaiter() {
            _nextTimeSkillAvailable = Time.time + cooldown;

            if (cooldown <= 0.0f) yield break;

            button.interactable = false;
            yield return new WaitForSeconds(cooldown);
            button.interactable = true;
        }
    }
}
