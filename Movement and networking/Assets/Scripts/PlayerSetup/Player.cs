using System.Collections;
using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{
    #region Variables
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Behaviour[] disableOnDeath;

    [SerializeField] private ImposterDamage imposterDamageScript;


    [SyncVar]
    private bool isDead = false;

    public bool isPlayerDead
    {
        get {return isDead;}
        protected set {isDead = value;}
    }

    private bool [] wasEnabled;

    [SyncVar]
    private int currentHealth;
    #endregion

    ///<summary> Checks what methods are enabled. This is to see what method to disable once the player dies.
    ///<para> Requires: wasEnabled bool.
    ///<para> Requires: disableOnDeath List
    ///<para> Requires: SetDefaults method.
    ///</summary>
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    ///<summary> Sets up the player's health and all the methods that the player needs to play the game.
    ///<para> Requires: disableOnDeath list.
    ///<para> Requires: currentHealth float.
    ///<para> Requires: maxHealth float.
    ///<para> Requires: isPlayerDead bool.
    ///</summary>
    public void SetDefaults()
    {
        isPlayerDead = false;

        currentHealth = maxHealth;

        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
    }

    ///<summary> When the player's health goes to zero, methods will be disabled and respawned is called.
    ///<para> Requires: isPlayerDead bool.
    ///<para> Requires: disableOnDeath list.
    ///<para> Requires: Respawn IEnumerator.
    ///</summary>
    private void Die()
    {
        isPlayerDead = true;
        //Disable Components
        for (int i = 0; i < disableOnDeath.Length; i ++)
        {
            disableOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name + "is dead");

        //Respawn
        StartCoroutine(BecomeInfected());
        
    }

    ///<summary> When the player dies. They become infected and they can attack other players to return to normal.
    ///</summary>
    IEnumerator BecomeInfected()
    {
        yield return new WaitForSeconds(5f);
        SetDefaults();
        transform.Find("Player Capsule Visual").gameObject.SetActive(false);
        transform.Find("Monster Capsule Visual").gameObject.SetActive(true);
        Debug.Log("turned Infected");
        imposterDamageScript.enabled = true;
    }

    ///<summary> Decreases the player's health when this is called.
    ///</summary>
    [ClientRpc]
    public void RpcTakeDamage(int weaponDamage)
    {
        if (isPlayerDead) 
        {
            return;
        }

        currentHealth -= weaponDamage;

        Debug.Log(transform.name + "now has " + currentHealth + "Health");

        if(currentHealth <= 0) 
        {
            Die();
        }
    }
}
