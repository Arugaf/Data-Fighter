using System.Collections;
using System.Linq;
using Actors;
using InputModule.GameRelated;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Fighters {
    public class Skill : MonoBehaviour {
        [SerializeField] private int skillPower;
        [SerializeField] private bool aoe;
        [SerializeField] private float cooldown;

        [SerializeField] private Button button;

        [SerializeField] private Actor actor;

        private FighterSelector _fighterSelector;

        private float _nextTimeSkillAvailable;

        private bool _waitingForTarget;

        private void Start() {
            _fighterSelector = GameObject.FindGameObjectWithTag("Meta").GetComponent<FighterSelector>();

            GotUnselectAllSkills += OnUnselectAllSkills;
        }

        public void InvokeSkill() {
            GotUnselectAllSkills?.Invoke();

            if (Time.time < _nextTimeSkillAvailable) return;
            // [[likely]]
            if (!aoe) {
                if (_waitingForTarget) return;

                _waitingForTarget = true;
                FighterSelector.GotFighterClicked += OnTargetSelected;
                return;
            }

            foreach (var fighter in _fighterSelector.GetFighters())
                if (fighter)
                    fighter.ApplyDamage(skillPower);
            StartCoroutine(CooldownWaiter());
        }

        public static event UnityAction GotUnselectAllSkills;

        private void OnTargetSelected(GameObject go) {
            var fighter = go.GetComponent<Fighter>();
            if (actor.GetFighterList().Contains(fighter)) return;

            _waitingForTarget = false;
            fighter.ApplyDamage(skillPower);
            FighterSelector.GotFighterClicked -= OnTargetSelected;

            StartCoroutine(CooldownWaiter());
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
