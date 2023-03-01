using UnityEngine;

namespace AlwaysEast
{
    public class PlayerCharacter : Navigator
    {
        private const int UNARMED_DMG_PHYS_MIN = 2;
        private const int UNARMED_DMG_PHYS_MAX = 3;

        private void Start()
        {
            SaveState state = Game.LoadState();

            _base = new EntityReplacement();
            _base.baseStats.Name = state.PlayerName;
            _base.baseStats.Speed = state.PlayerSpeed;
            _base.baseStats.Experience = state.PlayerExperience;
            _base.baseStats.Level = Mathf.Clamp(_base.baseStats.Experience / 4 * (_base.baseStats.Experience - 1 + 300 * 2 * (_base.baseStats.Experience - 1 / 7)), 1, 99);
            _base.baseStats.DmgPhyMin = 2;
            _base.baseStats.DmgPhyMax = 3;
            _base.baseStats.AttackRating = 5;
            _base.baseStats.Strength = state.playerBaseStrength;
            _base.baseStats.Constitution = state.playerBaseConstitution;
            _base.baseStats.Dexterity = state.playerBaseDexterity;
            _base.baseStats.Intelligence = state.playerBaseIntelligence;
            _base.baseStats.LifeMax = state.PlayerLifeMax;
            _base.baseStats.LifeCurrent = state.PlayerLifeCurrent;
            _base.baseStats.ManaMax = state.PlayerManaMax;
            _base.baseStats.ManaCurrent = state.PlayerManaCurrent;

            PlayerHealthBar.SetMaximumLife((int)Life_Max);
            PlayerHealthBar.SetCurrentLife(_base.baseStats.LifeCurrent);
            Inventory.RefreshCharacterStats(this);

            Inventory.OnEquipmentChange += Inventory_OnEquipmentChange;

            //Inventory.Pickup("Primary/1/Short Sword");
            //Inventory.Pickup("Chest/1/Animal Skin");
            //Inventory.Pickup("Secondary/1/Buckler");
        }

        public override void Move()
        {
            int disX = Mathf.Abs(TileMapCursor.SelectedTileCoordinates.x - _coordinates.x);
            int disY = Mathf.Abs(TileMapCursor.SelectedTileCoordinates.y - _coordinates.y);
            int distance = disX + disY;

            // If we're at the location then we don't need to move
            if (distance <= 0)
                return;

            _chain = Pathfind.GetPath(_coordinates, TileMapCursor.SelectedTileCoordinates, false);

            if (IsNullOrDefault(_chain))
                return;

            //if (_primary != null)
            //    _primary.SetBool("Moving", true);

            AudioDevice.Play(onMove);

            TileMapCursor.Hide();
            HUDControls.Hide();
            base.Move();
        }

        protected override void OnArrival()
        {
            //if (_primary != null)
            //    _primary.SetBool("Moving", false);

            base.OnArrival();
        }

        public override void Attack()
        {
            // If there are no enemies, return
            if (Entities.Search(TileMapCursor.SelectedTileCoordinates).Count <= 0)
                return;

            int disX = Mathf.Abs(TileMapCursor.SelectedTileCoordinates.x - _coordinates.x);
            int disY = Mathf.Abs(TileMapCursor.SelectedTileCoordinates.y - _coordinates.y);
            int distance = disX + disY;

            //If we're not in melee range, return
            if (distance != 1)
                return;

            HUDControls.Hide();

            AttackSplash.Show(TileMapCursor.SelectedTileCoordinates, AttackSplash.Type.Slash);
            Entities.Attack(TileMapCursor.SelectedTileCoordinates, Random.Range(DmgPhysMin, DmgPhysMax), IncAttackRating, _base.baseStats.Level);

            //if (_primary != null)
            //    _primary.SetTrigger("Attack");

            base.Attack();
        }

        private void Inventory_OnEquipmentChange(ItemStats itemStats, bool adding)
        {
            if (itemStats.MinDamage > 0)
            {
                if (adding)
                {
                    _base.baseStats.DmgPhyMin = itemStats.MinDamage;
                }
                else
                {
                    _base.baseStats.DmgPhyMin = UNARMED_DMG_PHYS_MIN;
                }
            }
            if (itemStats.MaxDamage > 0)
            {
                if (adding)
                {
                    _base.baseStats.DmgPhyMax = itemStats.MaxDamage;
                }
                else
                {
                    _base.baseStats.DmgPhyMax = UNARMED_DMG_PHYS_MAX;
                }
            }
            if (itemStats.Defense > 0)
            {
                if (adding)
                {
                    stats[StatID.Def_Phys_Flat] += itemStats.Defense;
                }
                else
                {
                    stats[StatID.Def_Phys_Flat] -= itemStats.Defense;
                }
            }
            if (itemStats.Blockrate > 0)
            {
                if (adding)
                {
                    _base.baseStats.ChanceToBlock += itemStats.Blockrate;
                }
                else
                {
                    _base.baseStats.ChanceToBlock -= itemStats.Blockrate;
                }
            }

            foreach (Item.Prefix item in itemStats.Prefixes)
            {
                if (adding)
                {
                    stats[(StatID)item.type] += item.value;
                }
                else
                {
                    stats[(StatID)item.type] -= item.value;
                }
            }
            foreach (Item.Suffix item in itemStats.Suffixes)
            {
                if (adding)
                {
                    stats[(StatID)item.type] += item.value;
                }
                else
                {
                    stats[(StatID)item.type] -= item.value;
                }
            }
            foreach (Item.Implicit item in itemStats.Implicits)
            {
                if (adding)
                {
                    stats[(StatID)item.type] += item.value;
                }
                else
                {
                    stats[(StatID)item.type] -= item.value;
                }
            }

            PlayerHealthBar.SetMaximumLife((int)Life_Max);
            Inventory.RefreshCharacterStats(this);
        }

        public override void PreTurn()
        {
            //regen life
            if (_base.baseStats.LifeCurrent < Life_Max && RegenLife > 0)
            {
                _base.baseStats.LifeCurrent = Mathf.Clamp(_base.baseStats.LifeCurrent + RegenLife, 0, (int)Life_Max);
                Entities.DrawFloatingText(RegenLife.ToString(), transform, Color.green);
            }

            base.PreTurn();
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

            PlayerHealthBar.SetCurrentLife(_base.baseStats.LifeCurrent);

            if (_base.baseStats.LifeCurrent <= 0)
            {
                Die();
            }
        }
    }
}