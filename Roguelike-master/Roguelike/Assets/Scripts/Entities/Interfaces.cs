using UnityEngine;

namespace AlwaysEast
{

    public interface IAnimate
    {
        Animator _animator { get; set; }
        void OnAnimationEnd(string message);
    }

    public interface ICanMove
    {
        public System.Collections.Generic.List<Node> _chain { get; set; }
        public SpriteRenderer _spriteRenderer { get; set; }

        void UpdateAnimator(Vector3Int dir);
    }

    public interface ICanAttack
    {

    }

    public interface ICanBeAttacked
    {
        void ReceiveDamage(int incomingDamage, float attackerCombatRating, float attackerLevel);
        void Die();
    }

    public interface ICanEquip
    {

    }
}