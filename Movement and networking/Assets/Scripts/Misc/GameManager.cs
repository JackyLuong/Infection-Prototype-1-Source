using System.Collections.Generic;
using UnityEngine;

/*
    Creates and registers players into the game
*/
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // all scenes share the same game manager

    // Checks if there is already a game manager that exists
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
    private const string PLAYER_ID_PREFIX = "Player"; // beginning string of each playerId

    private static Dictionary<string, Player> allPlayersInGame = new Dictionary<string, Player>(); // Stores all player ids in the game

    /*
        Creates a player id for each players that entered in the game and adds it into the dictionary
    */
    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        allPlayersInGame.Add(playerID, player);
        player.transform.name = playerID;
    }
    /*
        Removes the players from the dictionary when the exit the game
    */
    public static void UnRegisterPlayer (string playerID)
    {
        //remove the player once they lost or have disconnected
        allPlayersInGame.Remove(playerID);
    }
    /*
        Get the player based on their playerId
    */
    public static Player GetPlayer (string playerID)
    {
        //Returns Player oject based on thier id
        return allPlayersInGame[playerID];
    }

    /*
        Display all player Id and their 
    */ 
    void OnGUI()
    {
        //Displays all the players in the game and their names
        GUILayout.BeginArea (new Rect(200,200,200,500));

        GUILayout.BeginVertical();

        foreach (string playerID in allPlayersInGame.Keys)
        {
            GUILayout.Label (allPlayersInGame[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    # endregion

}

