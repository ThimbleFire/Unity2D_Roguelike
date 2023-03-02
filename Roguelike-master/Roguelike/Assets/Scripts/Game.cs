using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AlwaysEast
{
    public class Game
    {
        public static bool SessionExists { get { return File.Exists(Application.persistentDataPath + "/CharacterProfile.east"); } }

        public static void NewSession(Enums.Class startingClass)
        {
            CharacterProfile characterProfile = new CharacterProfile()
            {
                PlayerExperience = 0,
                PlayerResAll = 0,
                PlayerName = "Player Character",
                PlayerSpeed = 4
            };

            MapProfile mapProfile = new MapProfile()
            {
                MapIndex = 0,
                MapSeed = Random.Range(int.MinValue, int.MaxValue)
            };

            ItemProfile itemProfile = new ItemProfile();

            switch (startingClass)
            {
                case Enums.Class.Melee:
                    characterProfile.playerBaseStrength = 25;
                    characterProfile.playerBaseDexterity = 20;
                    characterProfile.playerBaseIntelligence = 15;
                    characterProfile.playerBaseConstitution = 25;
                    characterProfile.PlayerLifeMax = -22;
                    characterProfile.PlayerManaMax = 15;

                    itemProfile.ItemPath = "PRIMARY/1/Short Sword";
                    itemProfile.Type = ItemState.Type.PRIMARY;
                    SaveState(itemProfile, "PRIMARY.east");

                    itemProfile.ItemPath = "SECONDARY/1/Buckler";
                    itemProfile.Type = ItemState.Type.SECONDARY;
                    SaveState(itemProfile, "SECONDARY.east");
                    break;
                case Enums.Class.Ranged:
                    characterProfile.playerBaseStrength = 20;
                    characterProfile.playerBaseDexterity = 15;
                    characterProfile.playerBaseIntelligence = 15;
                    characterProfile.playerBaseConstitution = 20;
                    characterProfile.PlayerLifeMax = -22;
                    characterProfile.PlayerManaMax = 40;

                    itemProfile.ItemPath = "PRIMARY/1/Wooden Short Bow";
                    itemProfile.Type = ItemState.Type.PRIMARY;
                    SaveState(itemProfile, "PRIMARY.east");
                    break;
                case Enums.Class.Magic:
                    characterProfile.playerBaseStrength = 10;
                    characterProfile.playerBaseDexterity = 25;
                    characterProfile.playerBaseIntelligence = 35;
                    characterProfile.playerBaseConstitution = 10;
                    characterProfile.PlayerLifeMax = -22;
                    characterProfile.PlayerManaMax = 50;

                    itemProfile.ItemPath = "PRIMARY/1/Short Staff";
                    itemProfile.Type = ItemState.Type.PRIMARY;
                    SaveState(itemProfile, "PRIMARY.east");
                    break;
            }

            characterProfile.PlayerManaCurrent = characterProfile.PlayerManaMax + Mathf.FloorToInt(characterProfile.playerBaseIntelligence * 1.5f) + 1;
            characterProfile.PlayerLifeCurrent = characterProfile.PlayerLifeMax + characterProfile.playerBaseConstitution * 3 + 2;
            SaveState(characterProfile, "CharacterProfile.east");
            SaveState(mapProfile, "MapProfile.east");
        }

        public static void SaveState<T>(T state, string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Create);
            formatter.Serialize(stream, state);
            stream.Close();
        }

        public static T LoadState<T>(string filename)
        {
            if (File.Exists(Application.persistentDataPath + "/" + filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Open);
                T product = (T)formatter.Deserialize(stream);
                stream.Close();
                return product;
            }
            else
            {
                Debug.LogWarning("File not found in " + Application.persistentDataPath + "/" + filename);
                return default;
            }
        }
    }
}