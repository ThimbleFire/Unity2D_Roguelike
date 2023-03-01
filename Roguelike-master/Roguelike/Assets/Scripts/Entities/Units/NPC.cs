
using UnityEngine;

namespace AlwaysEast
{

    public class NPC : Navigator
    {

        private void Start()
        {

        }

        protected override void Die()
        {
            LootDropper.RollLoot(transform, _base.baseStats.Level, _base.baseStats.TreasureClass);

            base.Die();
        }

        public override void RecieveDamage(int incomingDamage, float attackerCombatRating, float attackerLevel)
        {
            //Roll dodge
            float CRvDR = attackerCombatRating / (attackerCombatRating + Defense);
            float ALvDL = attackerLevel / (attackerLevel + _base.baseStats.Level);
            float chanceToHit = 200 * CRvDR * ALvDL;
            float value = Random.Range(0.0f, 100.0f);
            if (chanceToHit < value)
            {
                Entities.DrawFloatingText("Miss", transform, Color.gray);
                return;
            }

            //Roll block
            if (BlockRecoveryTurnsRemaining == 0)
            {
                value = Random.Range(0.0f, 100.0f);
                if (value <= _base.baseStats.ChanceToBlock)
                {
                    AudioDevice.Play(block);
                    Entities.DrawFloatingText("Blocked", transform, Color.gray);
                    BlockRecoveryTurnsRemaining = BlockRecoveryBase;
                    return;
                }
            }

            // reduce incoming damage by this entities flat damage reduction
            incomingDamage -= DefDmgReductionPhys;

            // reduce incoming damage by armour. This code desparately needs refining.
            float actualIncomingDamage = incomingDamage;
            float percentReduction = Defense / 1000 * 70;
            float percentLeftOver = 100 - percentReduction;
            actualIncomingDamage *= percentLeftOver / 100;
            actualIncomingDamage = Mathf.Clamp(actualIncomingDamage, 1.0f, float.MaxValue);

            Entities.DrawFloatingText(((int)actualIncomingDamage).ToString(), transform, Color.red);
            _base.baseStats.LifeCurrent -= (int)actualIncomingDamage;
            AudioDevice.Play(onHit);

            if (_base.baseStats.LifeCurrent <= 0)
            {
                Die();
            }
        }
    }
}