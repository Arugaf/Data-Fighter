using System.Linq;
using Actors;
using Fighters;
using UnityEngine;

namespace AI {
    public class BattleMind : MonoBehaviour {
        [SerializeField] private Actor player;
        [SerializeField] private Actor enemy;

        private Fighter _enemyTarget;
        private Fighter _playerTarget;

        private void Start() {
            SelectTarget(enemy, ref _playerTarget);
            SelectTarget(player, ref _enemyTarget);

            Fighter.GotFighterDeath += OnFighterDeath;
        }

        public Fighter GetTarget(bool isForEnemy = false) {
            return isForEnemy ? _enemyTarget : _playerTarget;
        }

        private static void SelectTarget(Actor oppositeActor, ref Fighter target) {
            var fighterList = oppositeActor.GetFighterList();
            var enumerable = fighterList as Fighter[] ?? fighterList.ToArray();
            if (!enumerable.Any()) {
                target = null;
                return;
            }

            var maxTargetHp = 0;

            foreach (var f in enumerable) {
                if (f.Hp <= maxTargetHp) continue;
                maxTargetHp = f.Hp;
                target = f;
            }
        }

        private void OnFighterDeath(Fighter fighter) {
            if (fighter == _playerTarget)
                SelectTarget(enemy, ref _playerTarget);
            else if (fighter == _enemyTarget) SelectTarget(player, ref _enemyTarget);
        }
    }
}
