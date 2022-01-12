using UnityEngine;
/*
    Disables the landiing page and starts the game
*/
public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby _networkManager;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel;

    //Disables the landiing page and starts the game
    public void HostLobby()
    {
        _networkManager.StartHost();

        _landingPagePanel.SetActive(false);
    }
}


