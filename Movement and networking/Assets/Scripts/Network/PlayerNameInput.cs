using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    Player types their username in the main menu and its saved for them
*/
public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button continueButton;

    public static string DisplayName 
    {
        get; 
        private set;
    }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUpInputField();
    
    private void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) 
            return;
        
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);

    }
    public void SetPlayerName(string name)
    {
        continueButton.interactable = nameInputField.text.Length >= 4;
    }
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
