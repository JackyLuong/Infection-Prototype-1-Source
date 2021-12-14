using UnityEngine;
using Cinemachine;
using Mirror;

public class ImposterDamage : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] private Player playerScript;
    [SerializeField] private int playerDamage;
    [SerializeField] private int damageDistance;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
    [SerializeField] private LayerMask mask;
     
     ///<summary> Previous movement needs to be called as movements are only called once rather than once every frame.
     ///<para>    So it needs to know when to change its movement by knowing when to reset.
     ///</summary>
    private Vector2 previousInput;
    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }
    
     ///<summary> Everytime when a movement key is pressed, it either continues its current path or resets it.
     ///<para> Requires: SetMovement method
     ///<para> Requires: ResetMovement method
     ///</summary>
    public override void OnStartAuthority()
    {
        enabled = true;
        controls.Player.Damage.performed += ctx => DamagePlayer();
    }

     ///<summary> Only allows the player to jump if they are one the ground, and applies gravity once its above the ground.
     ///<para> Requires: jumpDirection variable
     ///<para> Requires: gravity variable
     ///<para> Requires: controller object
     ///</summary>
    
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    
    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [Client]
    private void DamagePlayer()
    {
        Debug.Log("Monster is attacking");
        if(!hasAuthority)
        return;

        RaycastHit hit;
        if (Physics.Raycast(virtualCamera.transform.position, virtualCamera.transform.forward, out hit, damageDistance, mask))
        {
            if(hit.collider.tag == PLAYER_TAG && hit.collider.name != gameObject.name)
            {
                CmdDamagePlayer(hit.collider.name);
                if(hit.collider.gameObject.GetComponent<Player>().isPlayerDead == true)
                {
                    BecomePlayer();
                    Debug.Log("player dead");
                }
            }
        }
    }

    [Command]
    private void CmdDamagePlayer(string playerID)
    {
        Debug.Log (playerID + "has been shot");
        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(playerDamage);
    }
    
    private void BecomePlayer()
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
