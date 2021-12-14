using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in scene");
        } else
        {
            instance = this;
        }
    }
    #region Player Tracking
    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> allPlayersInGame = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        allPlayersInGame.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void UnRegisterPlayer (string playerID)
    {
        //remove the player once they lost or have disconnected
        allPlayersInGame.Remove(playerID);
    }

    public static Player GetPlayer (string playerID)
    {
        //get player ID
        return allPlayersInGame[playerID];
    }

    void OnGUI()
    {
        //Displays all the players in the game and their names
        GUILayout.BeginArea (new Rect(200,200,200,500));

        GUILayout.BeginVertical();

        foreach (string playerID in allPlayersInGame.Keys)
        {
            GUILayout.Label (playerID + " - " + allPlayersInGame[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }
    # endregion

}

