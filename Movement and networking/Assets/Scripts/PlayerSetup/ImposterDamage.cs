using UnityEngine;
using Cinemachine;
using Mirror;

// Monster can damage players and infected can become players again after killing a player
public class ImposterDamage : NetworkBehaviour
{
    #region Variables
    [SerializeField] private Player playerScript;
    [SerializeField] private int playerDamage;
    [SerializeField] private int damageDistance;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
    [SerializeField] private LayerMask mask;
    
    private const string PLAYER_TAG = "Player";
    
    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    private bool isInfected = false;
    #endregion

     ///<summary> Enables the script and subscribes DamagePlayer() method to left mouse button
     ///<para> Requires: DamagePlayer method
     ///<para> Requires: Controls script
     ///</summary>
    public override void OnStartAuthority()
    {
        enabled = true;
        controls.Player.Damage.performed += ctx => DamagePlayer();
    }
    
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    
    [ClientCallback]
    private void OnDisable() => Controls.Disable();

     ///<summary> Creates a raycast to see how far the infector or infected is to the player. It will damage the player if they are not infected and cause them to die after they have no health left.
     ///<para> Players can only attack those that are not infected
     ///<para> Requires: CmdDamagePlayer method
     ///<para> Requires: CmdBecomePlayer method
     ///<para> Requires: Player Script
     ///</summary>
    [Client]
    private void DamagePlayer()
    {
        if(!hasAuthority)
        return;

        RaycastHit hit;
        if (Physics.Raycast(virtualCamera.transform.position, virtualCamera.transform.forward, out hit, damageDistance, mask))
        {
            if(hit.collider.tag == PLAYER_TAG && hit.collider.name != gameObject.name)
            {
                isInfected = hit.collider.transform.Find("Monster Capsule Visual").gameObject.activeSelf;

                if(isInfected == false)
                {
                    CmdDamagePlayer(hit.collider.name);
                
                    if(hit.collider.gameObject.GetComponent<Player>().isPlayerDead == true)
                    {
                        Debug.Log("BecomePlayer");
                        CmdBecomePlayer();
                    }
                }
            }
        }
    }

     ///<summary> Finds and damages the player when called
     ///<para> Requires: RpcTakeDamage method
     ///<para> Requires: GameManager Script
     ///<para> Requires: Player Script
     ///</summary>
    [Command]
    private void CmdDamagePlayer(string playerID)
    {
        Debug.Log (playerID + "has been shot");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(playerDamage);
    }
    
     ///<summary> Tells the Server to make all clients do RpcBecomePlayer
     ///<para> Requires: RpcBecomePlayer method
     ///</summary>
    [Command]
    private void CmdBecomePlayer()
    {
        RpcBecomePlayer();
    }

     ///<summary> When an infected kills another player, they return as a player
     ///</summary>
    [ClientRpc]
    private void RpcBecomePlayer()
    {
        Debug.Log(gameObject.name);
        if(this.gameObject.tag == "Player")
        {
            transform.Find("Player Capsule Visual").gameObject.SetActive(true);
            transform.Find("Monster Capsule Visual").gameObject.SetActive(false);
            this.enabled = false;
        }
    }
}
