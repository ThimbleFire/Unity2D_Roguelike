using System.Collections.Generic;
using UnityEngine;

namespace AlwaysEast
{
    [RequireComponent(typeof(Animator))]
    public abstract class Entity : MonoBehaviour
    {
        public const int BlockRecoveryBase = 5;
        public const int StaggerRecoveryBase = 1; // Stagger is not yet implemented.

        [SerializeField]
        public EntityReplacement _base { get; set; }

        public int TotalMagicFind => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Magic_Find] + stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Magic_Find] + stats[(Enums.StatID)ItemState.Prefix.PType.Plus_Magic_Find];
        public int TotalSpeed => _base.baseStats.Speed + (int)stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Speed_Movement] + stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Speed_Movement];
        public int TotalDefDmgReductionPhys => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Dmg_Reduction_Phys] + stats[(Enums.StatID)ItemState.Suffix.SType.Def_Dmg_Reduction_All] + stats[(Enums.StatID)ItemState.Implicit.IType.Def_Dmg_Reduction_All];
        public int TotalDefDmgReductionMagic => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Dmg_Reduction_Magic] + stats[(Enums.StatID)ItemState.Suffix.SType.Def_Dmg_Reduction_All] + stats[(Enums.StatID)ItemState.Implicit.IType.Def_Dmg_Reduction_All];
        public float TotalLifeMax => _base.baseStats.LifeMax + TotalConstitution * 3 + stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Life] + _base.baseStats.Level * 2;
        public float TotalManaMax => _base.baseStats.ManaMax + Mathf.Floor(TotalIntelligence * 1.5f) + stats[(Enums.StatID)ItemState.Prefix.PType.Plus_Mana] + Mathf.Floor(_base.baseStats.Level * 1.5f);
        public int TotalDmgPhysMin => _base.baseStats.DmgPhyMin + TotalStrength / 10 + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Phys_Min];
        public int TotalDmgPhysMax => _base.baseStats.DmgPhyMax + TotalStrength / 10 + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Phys_Max];
        public float TotalDefense => _base.baseStats.Defense + (TotalDexterity / 10) + stats[Enums.StatID.Def_Phys_Flat];
        public int TotalDmgEleFireMin => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Fire] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Fire];
        public int TotalDmgEleFireMax => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Fire] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Fire];
        public int TotalDmgEleColdMin => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Cold] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Cold];
        public int TotalDmgEleColdMax => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Cold] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Cold];
        public int TotalDmgEleLightningMin => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Lightning] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Lightning];
        public int TotalDmgEleLightningMax => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Lightning] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Lightning];
        public int TotalDmgElePoisonMin => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Poison] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Poison];
        public int TotalDmgElePoisonMax => stats[(Enums.StatID)ItemState.Prefix.PType.Dmg_Ele_Poison] + stats[(Enums.StatID)ItemState.Suffix.SType.Dmg_Ele_Poison];
        public int TotalDefResFire => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_All] + stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_Fire];
        public int TotalDefResCold => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_All] + stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_Cold];
        public int TotalDefResLightning => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_All] + stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_Lightning];
        public int TotalDefResPoison => stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_All] + stats[(Enums.StatID)ItemState.Prefix.PType.Def_Ele_Res_Poison];
        public int TotalOnHitLife => stats[(Enums.StatID)ItemState.Prefix.PType.On_Hit_Life];
        public int TotalOnKillLife => stats[(Enums.StatID)ItemState.Prefix.PType.On_Kill_Life];
        public int TotalOnHitMana => stats[(Enums.StatID)ItemState.Suffix.SType.On_Hit_Mana];
        public int TotalOnKillMana => stats[(Enums.StatID)ItemState.Suffix.SType.On_Kill_Mana];
        public int TotalRegenLife => stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Regen_Life] + stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Regen_Life];
        public int TotalRegenMana => stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Regen_Mana] + stats[(Enums.StatID)ItemState.Prefix.PType.Plus_Regen_Mana];
        public int TotalBlockRecovery => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Block_Recovery] + stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Block_Recovery];
        public int TotalStaggerRecovery => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Stagger_Recovery] + stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Stagger_Recovery];
        public float TotalBlockRate => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Blockrate] + stats[(Enums.StatID)ItemState.Implicit.IType.Plus_Blockrate] + _base.baseStats.ChanceToBlock;
        public float TotalAttackRating => TotalDexterity / 2 + stats[(Enums.StatID)ItemState.Prefix.PType.Plus_Attack_Rating] + _base.baseStats.AttackRating;
        public int TotalStrength => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Str] + _base.baseStats.Strength;
        public int TotalDexterity => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Dex] + _base.baseStats.Dexterity;
        public int TotalConstitution => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Con] + _base.baseStats.Constitution;
        public int TotalIntelligence => stats[(Enums.StatID)ItemState.Suffix.SType.Plus_Int] + _base.baseStats.Intelligence;

        protected Dictionary<Enums.StatID, int> stats = new Dictionary<Enums.StatID, int>();
        protected SpriteRenderer spriteRenderer;

        public bool IsAggressive
        {
            get
            {
                int disX = Mathf.Abs(Entities.GetPCS._coordinates.x - _coordinates.x);
                int disY = Mathf.Abs(Entities.GetPCS._coordinates.y - _coordinates.y);
                int distance = disX + disY;

                return distance <= _base.baseStats.RangeOfAggression;
            }
        }

        public AudioClip onAttack, onHit, onMove, miss, block;

        protected List<Node> _chain = new List<Node>();

        public Vector3Int _coordinates;
        protected Animator _animator;
        protected int BlockRecoveryTurnsRemaining = 0;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            for (int i = 0; i < System.Enum.GetNames(typeof(Enums.StatID)).Length; i++)
            {
                stats.Add((Enums.StatID)i, 0);
            }
        }

        public virtual void Attack()
        {
            _animator.SetTrigger("Attack");
            AudioDevice.Play(onAttack);
        }

        public virtual void Move()
        {
        }

        public virtual void Interact()
        {
        }

        /// <summary>
        /// This method is used by the AI. Player actions are separate in PlayerCharacter.cs
        /// </summary>
        public virtual void Action()
        {
            if (TotalSpeed == 0)
            {
                Entities.Step(false);
                return;
            }

            Vector3Int playerCharacterCoordinates = Entities.GetPCS._coordinates;

            // some AI shit

            int disX = Mathf.Abs(playerCharacterCoordinates.x - _coordinates.x);
            int disY = Mathf.Abs(playerCharacterCoordinates.y - _coordinates.y);

            bool canAttack = disX + disY == 1;
            if (canAttack)
            {
                Attack();
                AttackSplash.Show(playerCharacterCoordinates, AttackSplash.Type.Pierce);
                Entities.Attack(playerCharacterCoordinates, Random.Range(TotalDmgPhysMin, TotalDmgPhysMax + 1), TotalAttackRating, _base.baseStats.Level);
                return;
            }

            if (IsAggressive)
            {
                _chain = Pathfind.GetPath(_coordinates, playerCharacterCoordinates, false);

                if (Helper.IsNullOrDefault(_chain))
                {
                    Entities.Step(spriteRenderer.isVisible);
                    return;
                }
            }
            else _chain = Pathfind.Wander(_coordinates);
            
            if (spriteRenderer.isVisible)
                AudioDevice.Play(onMove);
        }

        public virtual void RecieveDamage(int incomingDamage, float attackerCombatRating, float attackerLevel) { }

        public virtual void PreTurn()
        {
            //recover from blocking
            if (BlockRecoveryTurnsRemaining > 0)
                BlockRecoveryTurnsRemaining--;
        }

        protected virtual void Die()
        {
            //TextLog.Print( string.Format("<color=#FF0000>{0}</color> is slain", Name ) );
            _animator.SetTrigger("Die");
            Pathfind.Unoccupy(_coordinates);
            Entities.Remove(this);
            TileMapCursor.Hide();
        }

        public void DestroyAfterDeathAnimation()
        {
            Destroy(gameObject);
        }

        public void AlertObservers(string message)
        {
            if (message.Equals("AttackAnimationEnd"))
            {
                Entities.Step(spriteRenderer.isVisible);
            }
        }

        protected void UpdateAnimator(Vector3Int dir)
        {
            if (dir != Vector3Int.zero)
            {
                transform.localScale = -dir.x > 0 ? new Vector3(1.0f, 1.0f) : new Vector3(-1.0f, 1.0f);
                _animator.SetBool("Moving", true);
            }
        }

        public void SetEntity(EntityReplacement replacement)
        {
            _base = replacement;

            _base.baseStats.LifeCurrent = (int)TotalLifeMax;
            _base.baseStats.ManaCurrent = (int)TotalManaMax;

            this._animator.runtimeAnimatorController = Resources.Load<AnimatorOverrideController>(replacement.animatorOverrideControllerFileName);
        }
    }
}
