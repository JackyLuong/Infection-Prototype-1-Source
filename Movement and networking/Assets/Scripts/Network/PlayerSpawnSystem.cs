using Mirror;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    ///<summary> Spawns players in order of the spawn points
    ///<para> Requires: NetworkManagerLobby script
    ///<para> Requires: NetworkManagerLobby.OnServerReadied
    ///<para> Requires: Player prefab
    ///</summary>
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject monsterPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();
    private static List<GameObject> playerSpawned = new List<GameObject>();

    public static PlayerSpawnSystem instance = null;
    private int nextIndex = 0;

    private GameObject selectedPrefab = null;
    
    //Create singleton to allow for global access to this class.
    void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    //Create NetworkManager singleton
    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    ///<summary> Add Spawn points to the game
    ///</summary>
    public static void AddSpawnPoint(Transform transform)
    {
        //Adds spawn points and spawns in player in order
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    ///<summary> Removes Spawn points to the game
    ///</summary>
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    ///<summary> Spawns the player into the game when they area ready
    ///</summary>
    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

    ///<summary> Removes the player from the game when they leave
    ///</summary>
    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    ///<summary> Spawns the player, and enables the imposter ablilites and featurs if they are selected to be the imposter
    ///<para> Requires: NetworkManagerLobby.GetSelectAsMonsterNumber() method.
    ///<para> Requires: NetworkManagerLobby script.
    ///</summary>
    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        //Spawns in the player and gets the order of the spawn points to spawn them in.
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
        if(spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }
        
        if(Room.GetSelectAsMonsterNumber() == nextIndex)
            selectedPrefab = monsterPrefab;
        else
            selectedPrefab = playerPrefab;

        GameObject serverPlayerInstance = Instantiate(selectedPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);

        NetworkServer.Spawn(serverPlayerInstance, conn);
        nextIndex++;
    }
    
    public List<GameObject> GetPlayerSpawned()
    {
        return playerSpawned;
    }
}
