using UnityEngine;
using UnityEngine.UI;

public class AttributesUI : MonoBehaviour
{
    public static AttributesUI instance;

    public Text StrengthValue;
    public Text DexterityValue;
    public Text AgilityValue;
    public Text InteligenceValue;

    private PlayerStats _playerStats;

    private void Awake ( )
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start ( )
    {
        _playerStats = PlayerStats.instance;        
    }

    public void UpdateAttributes()
    {
        StrengthValue.text = _playerStats.Strength.GetValue ().ToString ();
        DexterityValue.text = _playerStats.Dexterity.GetValue().ToString ();
        AgilityValue.text = _playerStats.Agility.GetValue().ToString ();
        InteligenceValue.text = _playerStats.Inteligence.GetValue().ToString ();
    }
}
