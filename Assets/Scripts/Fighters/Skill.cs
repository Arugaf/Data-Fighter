using System.Collections;
using System.Linq;
using Actors;
using InputModule;
using InputModule.GameRelated;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fighters {
    public class Skill : MonoBehaviour {
        [SerializeField] private bool heal;

        [SerializeField] private int skillPower;
        [SerializeField] private bool aoe;
        [SerializeField] private float cooldown;

        [SerializeField] private Button button;

        [SerializeField] private Actor actor;

        private FighterSelector _fighterSelector;

        private float _nextTimeSkillAvailable;

        private bool _waitingForTarget;
        private bool _selected;

        [SerializeField] private Color selectColor = Color.cyan;
        [SerializeField] private Color idleColor = Color.white;

        private Image _buttonImage = null;

        private void Start() {
            _fighterSelector = GameObject.FindGameObjectWithTag("Meta").GetComponent<FighterSelector>();

            _buttonImage = button.GetComponent<Image>();
            _buttonImage.color = idleColor;

            GotUnselectAllSkills += OnUnselectAllSkills;
            InputHandler.GotEscapeKeyDown += () => {
                GotUnselectAllSkills?.Invoke();
                _selected = false;
                _buttonImage.color = idleColor;
            };
        }

        public static event UnityAction GotSkillActivated;

        public void InvokeSkill() {
            GotUnselectAllSkills?.Invoke();

            if (Time.time < _nextTimeSkillAvailable) return;
            // [[likely]]
            if (!aoe) {
                if (_selected) {
                    _selected = false;
                    _buttonImage.color = idleColor;
                    GotUnselectAllSkills?.Invoke();
                    return;
                }

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
            if (!heal && actor.GetFighterList().Contains(fighter)) return;

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
