using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
     ///<summary> Allows the players to join a perticular server.
     ///<para> Requires: NetworkManagerLobby script
     ///</summary>
    #region Variables
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;
    #endregion

     ///<summary> Executes HandleClientConneded/HandleClientDisconnected to all player joining when the menu is enabled
     ///<para> Requires: NetworkManagerLobby script
     ///<para> Requires: NetworkManagerLobby.OnClientConnected
     ///<para> Requires: NetworkManagerLobby.OnClientDisconnected
     ///<para> Requires: HandleClientConnected method
     ///<para> Requires: HandleClientDisconnected method
     ///</summary>
    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

     ///<summary> Reverse executes HandleClientConneded/HandleClientDisconnected to all player joining when the menu is disabled
     ///<para> Requires: NetworkManagerLobby script
     ///<para> Requires: NetworkManagerLobby.OnClientConnected
     ///<para> Requires: NetworkManagerLobby.OnClientDisconnected
     ///<para> Requires: HandleClientConnected method
     ///<para> Requires: HandleClientDisconnected method
     ///</summary>
    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

     ///<summary> Tries the join a lobby using the IP-address that the user typed
     ///<para> Requires: NetworkManagerLobby script
     ///</summary>
    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

     ///<summary> Disables landing page and allows the player to press the join button when the client tries to connect.
     ///</summary>
    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

     ///<summary> Sets the join button to interactable when the client decides to disconnect.
     ///</summary>
    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
