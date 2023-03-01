using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AlwaysEast
{
    public class Game
    {
        public static readonly string path = Application.persistentDataPath + "/GameSave.ae";

        public static bool SessionExists { get { return File.Exists(path); } }

        public static void NewSession(Enums.Class startingClass)
        {
            SaveState saveState = new SaveState()
            {
                MapIndex = 0,
                MapSeed = Random.Range(int.MinValue, int.MaxValue),
                PlayerExperience = 0,
                PlayerResAll = 0,
                PlayerName = "Player Character"
            };

            switch (startingClass)
            {
                case Enums.Class.Melee:
                    saveState.playerBaseStrength = 25;
                    saveState.playerBaseDexterity = 20;
                    saveState.playerBaseIntelligence = 15;
                    saveState.playerBaseConstitution = 25;
                    saveState.PlayerSpeed = 4;
                    saveState.PlayerLifeMax = 50;
                    saveState.PlayerLifeCurrent = 50;
                    saveState.PlayerManaMax = 25;
                    saveState.PlayerManaCurrent = 25;
                    //PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Sword");
                    //PlayerPrefs.SetString("Secondary", "Items/SECONDARY/1/Buckler");
                    break;
                case Enums.Class.Ranged:
                    saveState.playerBaseStrength = 20;
                    saveState.playerBaseDexterity = 15;
                    saveState.playerBaseIntelligence = 15;
                    saveState.playerBaseConstitution = 20;
                    saveState.PlayerSpeed = 4;
                    saveState.PlayerLifeMax = 40;
                    saveState.PlayerLifeCurrent = 40;
                    saveState.PlayerManaMax = 40;
                    saveState.PlayerManaCurrent = 40;
                    //PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Wooden Bow");
                    break;
                case Enums.Class.Magic:
                    saveState.playerBaseStrength = 10;
                    saveState.playerBaseDexterity = 25;
                    saveState.playerBaseIntelligence = 35;
                    saveState.playerBaseConstitution = 10;
                    saveState.PlayerSpeed = 4;
                    saveState.PlayerLifeMax = 30;
                    saveState.PlayerLifeCurrent = 30;
                    saveState.PlayerManaMax = 50;
                    saveState.PlayerManaCurrent = 50;
                    //PlayerPrefs.SetString("Primary", "Items/PRIMARY/1/Short Staff");
                    break;
            }

            SaveState(saveState);
        }

        public static void SaveState(SaveState state)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, state);
            stream.Close();
        }

        public static SaveState LoadState()
        {
            if (SessionExists)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                SaveState SaveState = (SaveState)formatter.Deserialize(stream);
                stream.Close();
                return SaveState;
            }
            else
            {
                Debug.LogError("File not found in " + path);
                return null;
            }
        }
    }
}