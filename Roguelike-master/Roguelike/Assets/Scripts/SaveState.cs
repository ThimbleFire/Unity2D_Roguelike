
namespace AlwaysEast
{
    [System.Serializable]
    public class CharacterProfile
    {
        public string PlayerName;
        public int PlayerSpeed;
        public int PlayerExperience;
        public int PlayerLifeMax;
        public int PlayerLifeCurrent;
        public int PlayerManaMax;
        public int PlayerManaCurrent;
        public int PlayerResAll;
        public int playerBaseStrength;
        public int playerBaseDexterity;
        public int playerBaseConstitution;
        public int playerBaseIntelligence;
    }

    [System.Serializable]
    public class MapProfile
    {
        public int MapIndex;
        public int MapSeed;
    }

    [System.Serializable]
    public class ItemProfile
    {
        public ItemState.Type Type;
        public string ItemPath;
    }
}