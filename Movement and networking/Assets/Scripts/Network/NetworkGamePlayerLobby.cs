using Mirror;
/*
    Allows all players that joined a server to load into the game
*/
public class NetworkGamePlayerLobby : NetworkBehaviour
{
    #region Class Variable
    [SyncVar]
    private string DisplayName = "Loading..."; // All name with by displayed the same across all clients and hosts

    private NetworkManagerLobby room; // lobby room
    
    //Getter for lobby room
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby; // Allow only one network manager to exist
        }
    }
    #endregion

    //Load players that joined the host
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject); // prevent the client from refreshing when loading into the game
        Room.GamePlayers.Add(this); //  add player object to the room
    }
    // Remove the client when the leave the lobby
    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    // Server Displays all the players names to every client
    [Server]
    public void SetDisplayName(string _displayName)
    {
        this.DisplayName = _displayName;
    }
}

