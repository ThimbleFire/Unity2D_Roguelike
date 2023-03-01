using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlwaysEast
{
    public class MainMenuControls : MonoBehaviour
    {
        public Text newGmaOrContinue;

        private void Awake()
        {
            bool saveExists = Game.SessionExists;

            if (saveExists)
            {
                if (newGmaOrContinue != null)
                    newGmaOrContinue.text = "Continue";
            }
        }

        public void NewGameOrContinue()
        {
            if (Game.SessionExists)
            {
                SceneManager.LoadScene("Gameplay");
                return;
            }

            SceneManager.LoadScene("CharacterCreation");
        }

        public void NewGameMelee()
        {
            Game.NewSession(Enums.Class.Melee);
            SceneManager.LoadScene("Gameplay");
        }

        public void NewGameRanged()
        {
            Game.NewSession(Enums.Class.Ranged);
            SceneManager.LoadScene("Gameplay");
        }

        public void NewGameMagic()
        {
            Game.NewSession(Enums.Class.Magic);
            SceneManager.LoadScene("Gameplay");
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}