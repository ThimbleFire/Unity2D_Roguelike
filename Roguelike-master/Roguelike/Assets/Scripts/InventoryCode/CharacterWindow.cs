using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : MonoBehaviour
{
    public static CharacterWindow Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Text t_poisonRes, t_lightRes, t_fireRes, t_coldRes, t_currentExperience, t_maxExperience, t_currentLife, t_maxLife, t_currentMana, t_maxMana, t_defense, t_skillAccuracy, t_weaponAccuracy, t_attackDamage, t_skillDamage, t_strength, t_dexterity, t_constitution, t_intelligence;

    public void UpdateDisplay()
    {
        t_strength.text = string.Format( "{0}", Character.Strength );
        t_dexterity.text = string.Format( "{0}", Character.Dexterity );
        t_constitution.text = string.Format( "{0}", Character.Constitution );
        t_intelligence.text = string.Format( "{0}", Character.Intelligence );

        t_maxLife.text = string.Format( "{0}", Character.Life_Max );
        t_maxMana.text = string.Format( "{0}", Character.Mana_Max );
        t_currentLife.text = string.Format( "{0}", Character.Life_Current );
        t_currentMana.text = string.Format( "{0}", Character.Mana_Current );

        t_defense.text = string.Format( "{0}", Character.Defense );

        t_attackDamage.text = string.Format( "{0} - {1}", Character.DmgPhysMin, Character.DmgPhysMax );

        t_weaponAccuracy.text = string.Format( "{0}", Character.DmgAccuracy );

        t_poisonRes.text = string.Format( "{0}%", Character.DefResPoison );
        t_lightRes.text = string.Format( "{0}%", Character.DefResLightning );
        t_fireRes.text = string.Format( "{0}%", Character.DefResFire );
        t_coldRes.text = string.Format( "{0}%", Character.DefResCold );
    }
}