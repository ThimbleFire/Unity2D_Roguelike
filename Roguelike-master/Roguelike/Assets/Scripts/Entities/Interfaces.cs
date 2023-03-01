Namespace AlwaysEast
{
    public interface IAnimate
    {
        Animator _animator { get; set; }
        void OnAnimationEnd( string message );
        // void DestroyAfterDeathAnimation();
        // Redundant , use OnAnimationEnd.
    }

    public interface IMove
    {
        public List<Node> _chain { get; set; }
        public SpriteRenderer _spriteRenderer { get; set; }

        void UpdateAnimator( Vector3Int dir );
    }

    public interface IAttack
    {
        
    }

    public interface IEquip
    {
    
    }
}
