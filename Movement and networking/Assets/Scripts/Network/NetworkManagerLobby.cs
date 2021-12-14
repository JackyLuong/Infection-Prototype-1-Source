using UnityEngine;
using Mirror;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    ///<summary> This class extends from Mirror's network manager. 
    ///<para> Requires: NetworkRoomPlayerLobby script
    ///<para> Requires: NetworkGamePlayerLobby script           
    ///</summary>

    #region Variables
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private string menuScene = string.Empty;
    [SerializeField] private string gameScene = string.Empty;
    
    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    
    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;

    private int SelectAsMonsterNumber;
    private int numberOfPlayers;
    #endregion
    
    #region Actions
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;
    #endregion
    
    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();
    
     ///<summary> Spawns all the prefabs in Resources/SpawnablePrefabs folder on the server when the server starts up. 
     ///<para> Require: Resources/SpawnablePrefabs folder
     ///</summary>
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    
     ///<summary> Spawns all the prefabs in Resources/SpawnablePrefabs folder on the client when the client starts up. 
     ///<para> Require: Resources/SpawnablePrefabs folder
     ///</summary>
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    //when the client connects to the network
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    //when the client disconnects from the network
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

     ///<summary> Players will be disconnected form the server if the max amount of people is already reached or if they are in the wrong scene. 
     ///</summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

     ///<summary> Adds player to the lobby when they join the server.
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: isLeader boolean
     ///</summary>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0; // varibale returns true if the player is first on the list
            
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            
            roomPlayerInstance.IsLeader = isLeader; // Set the first player on the list as the leader of the party
            
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

     ///<summary> Disconnects the person from the party if they decide to leave.
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: NotifyPlayersOfReadyState() method
     ///</summary>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            RoomPlayers.Remove(player);
            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

     ///<summary> Disconnects everyone form the server if the host of the party abruptly leaves 
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: RoomPlayer list
     ///<para> Requires: GamePlayer list
     ///</summary>
    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

     ///<summary> Lets everyone in the party know who is ready to start and who isnt.
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: RoomPlayer list
     ///<para> Requires: NetworkRoomPlayerLobby.HandleReadyToStart()
     ///</summary>
    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

     ///<summary> Lets everyone in the party know who is ready to start and who isnt.
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: RoomPlayer list
     ///<para> Requires: NetworkRoomPlayerLobby.IsReady boolean
     ///</summary>
    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }
        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }
        return true;
    }

     ///<summary> Lets everyone in the party know who is ready to start and who isnt.
     ///<para> Requires: menuScene string
     ///<para> Requires: IsReadyToStart() boolean method
     ///<para> Requires: ServerChanged Scene method
     ///<para> Requires: gameScene variable
     ///</summary>
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            // returns a number that allows one of the players to become an immposter

            if (!IsReadyToStart()) { return; }
            ServerChangeScene(gameScene);
        }
    }

     ///<summary> Changes the player prefabs from RoomPlayer to gamePlayer when the game goes from the menu to the gameScene.
     ///<para> Requires: NetworkRoomPlayerLobby script
     ///<para> Requires: NetworkRoomPlayerLobby.DisplayName
     ///<para> Requires: menuScene variable
     ///<para> Requires: gamePlayerPrefab variable
     ///<para> Requires: RoomPlayers list
     ///</summary>
    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
            SelectAsMonsterNumber = UnityEngine.Random.Range(0, (RoomPlayers.Count()));
            numberOfPlayers = RoomPlayers.Count();
        }
        base.ServerChangeScene(newSceneName);
    }

     ///<summary> Changes the player prefabs from RoomPlayer to gamePlayer when the game goes from the menu to the gameScene.
     ///<para> Requires: playerSpawnSystem GameObject
     ///</summary>
    public override void OnServerSceneChanged(string sceneName)
    {
        //As soon as the scene is loaded, the server will spawn the players throught its connection.
        if (sceneName.StartsWith("Scene_Map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    //When the server is ready
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);

    }

    public int GetSelectAsMonsterNumber()
    {
        return SelectAsMonsterNumber;
    }

    public int GetNumberOfPlayers()
    {
        return numberOfPlayers;
    }

    public GameObject GetPlayerSpawnSystem()
    {
        return playerSpawnSystem;
    }
}


