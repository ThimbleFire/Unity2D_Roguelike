using System;

namespace AlwaysEast
{
    [Serializable]
    public class EntityBaseStats : ICloneable
    {
        public string Name;
        public int Level;
        public int Speed;
        public int RangeOfAggression;
        public int Experience;
        public int TreasureClass;
        public int LifeMax;
        public int LifeCurrent;
        public int ManaMax;
        public int ManaCurrent;
        public int ItemFind;
        public int MagicFind;
        public int AttackRating;
        public int ChanceToBlock;
        public int Defense;
        public int ResFire;
        public int ResCold;
        public int ResLight;
        public int ResPoison;
        public int ResAll;
        public int DmgPhyMin;
        public int DmgFireMin;
        public int DmgColdMin;
        public int DmgLightMin;
        public int DmgPoisonMin;
        public int DmgEleAllMin;
        public int DmgPhyMax;
        public int DmgFireMax;
        public int DmgColdMax;
        public int DmgLightMax;
        public int DmgPoisonMax;
        public int DmgEleAllMax;
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Intelligence;

        public object Clone()
        {
            //EntityBaseStats a = (EntityBaseStats)this.MemberwiseClone();
            return new EntityBaseStats()
            {
                Name = Name,
                Level = Level,
                Speed = Speed,
                RangeOfAggression = RangeOfAggression,
                Experience = Experience,
                TreasureClass = TreasureClass,
                LifeMax = LifeMax,
                LifeCurrent = LifeCurrent,
                ManaMax = ManaMax,
                ManaCurrent = ManaCurrent,
                ItemFind = ItemFind,
                MagicFind = MagicFind,
                AttackRating = AttackRating,
                ChanceToBlock = ChanceToBlock,
                Defense = Defense,
                ResFire = ResFire,
                ResCold = ResCold,
                ResLight = ResLight,
                ResPoison = ResPoison,
                ResAll = ResAll,
                DmgPhyMin = DmgPhyMin,
                DmgFireMin = DmgFireMin,
                DmgColdMin = DmgColdMin,
                DmgLightMin = DmgLightMin,
                DmgPoisonMin = DmgPoisonMin,
                DmgEleAllMin = DmgEleAllMin,
                DmgPhyMax = DmgPhyMax,
                DmgFireMax = DmgFireMax,
                DmgColdMax = DmgColdMax,
                DmgLightMax = DmgLightMax,
                DmgPoisonMax = DmgPoisonMax,
                DmgEleAllMax = DmgEleAllMax,
                Strength = Strength,
                Dexterity = Dexterity,
                Constitution = Constitution,
                Intelligence = Intelligence,
            };
        }
    }
}