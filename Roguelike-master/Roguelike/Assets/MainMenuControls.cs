using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControls : MonoBehaviour
{
    public UnityEngine.UI.Text newGmaOrContinue;

    private void Awake()
    {
        bool saveExists = Game.SessionExists;

        if(saveExists)
        {
            if(newGmaOrContinue != null)
                newGmaOrContinue.text = "Continue";
        }
    }

    public void NewGameOrContinue()
    {
        if(Game.SessionExists) {
            Game.LoadSession();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterCreation");
    }

    public void NewGameMelee()
    {
        Game.NewSession(PlayerCharacter.Class.Melee);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void NewGameRanged()
    {
        Game.NewSession(PlayerCharacter.Class.Ranged);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void NewGameMagic()
    {
        Game.NewSession(PlayerCharacter.Class.Magic);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
