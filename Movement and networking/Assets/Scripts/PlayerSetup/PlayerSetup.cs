using UnityEngine;
using Mirror;

//Spawns players and create an ID for each player
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    public override void OnStartClient() 
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent <Player>();

        GameManager.RegisterPlayer(netID, player);
    }
    void Start() 
    {
        if(!hasAuthority)
        {
            AssignRemoteLayer();
        }
        GetComponent<Player>().Setup();
    }
    void AssignRemoteLayer() 
    {
        this.gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    void OnDisable()
    {
        GameManager.UnRegisterPlayer(transform.name);
    }
}
