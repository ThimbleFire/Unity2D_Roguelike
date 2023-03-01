Namespace AlwaysEast
{
    public interface IAnimate
    {
        Animator _animator { get; set; }

        [Obsolete("Redundant method. Use OnAnimationEnd instead.", true)]
        void OnAnimationEnd( string message );
    }

    public interface ICanMove
    {
        public List<Node> _chain { get; set; }
        public SpriteRenderer _spriteRenderer { get; set; }

        void UpdateAnimator( Vector3Int dir );
    }

    public interface ICanAttack
    {

    }

    public interface ICanBeAttacked
    {
        void ReceiveDamage(int incomingDamage, float attackerCombatRating, float attackerLevel );
        void Die();
    }

    public interface ICanEquip
    {
    
    }
}
