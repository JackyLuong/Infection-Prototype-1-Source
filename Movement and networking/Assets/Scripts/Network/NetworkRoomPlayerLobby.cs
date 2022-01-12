using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

///<summary> Puts all the players into the lobby menu where they can ready up and start the game.
///<para> NetworkManagerLobby script
///</summary>
public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    #region Variables
    [Header("UI")]
    [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

     ///<summary> Sets the first person as leader and give them the ability to start the game.
     ///</summary>
    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;

     ///<summary> Creates its own networkManager/server for the party
     ///<para> Requires: networkManagerLobby script
     ///</summary>
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }
    #endregion
    
     ///<summary> Shows player name when the RoomPlayer of that client starts up the game and displays the lobby menu.
     ///<para> Requires: CmdSetDisplayName method
     ///<para> Requires: PlayerNameInput script
     ///<para> Requires: PlayerNameInput.DisplayName method
     ///</summary>
    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        lobbyUI.SetActive(true);
    }

     ///<summary> Adds player into a lobby and updates the lobby menu
     ///<para> Requires: networkManagerLobby script
     ///<para> Requires: networkManagerLobby.RoomPlayers object
     ///<para> Requires: UpdateDisplay method
     ///</summary>
    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

     ///<summary> Removes player from the lobby and updates the lobby menu
     ///<para> Requires: networkManagerLobby script
     ///<para> Requires: networkManagerLobby.RoomPlayers object
     ///<para> Requires: UpdateDisplay method
     ///</summary>
    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

     ///<summary> Updates display when a player changes from "ready" to "not ready" and vise-versa
     ///<para> Requires: UpdateDisplay method
     ///</summary>
    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

     ///<summary> Updates display when a player changes name
     ///<para> Requires: UpdateDisplay method
     ///</summary>
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

     ///<summary> Updates the lobby menu to all players
     ///<para> Requires: networkManagerLobby script
     ///<para> Requires: networkManagerLobby.RoomPlayers object
     ///<para> Requires: networkManagerLobby.RoomPlayers.DisplayName metohod
     ///<para> Requires: networkManagerLobby.RoomPlayers IsReady method
     ///<para> Requires: playerNameTexts object
     ///<para> Requires: playerReadyTexts object
     ///</summary>
    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

     ///<summary> Allows the party leader to start if everyone is ready
     ///<para> Requires: isLeader boolean
     ///</summary>
    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader) 
        { 
            return; 
        }
        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }
        Room.StartGame();
    }
}

