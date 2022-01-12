using UnityEngine;
/*
    Checks and allows players to activate and deactivate the minigame
*/
public class PuzzleTrigger : MonoBehaviour
{
    [SerializeField] private Canvas PressF;
    [SerializeField] private Canvas keyPad;

    private GameObject player;

    ///<summary> Disables all UI
    ///<para> Requires: keyPad Canvas
    ///</summary>
    void Start()
    {
        keyPad.enabled = false;
    }

    // Allows the player to start the puzzle and disables all player movement
    void OnTriggerEnter(Collider playerObject)
    {
        enableGame(playerObject);
    }

    // Allows the player to start the puzzle and disables all player movement
    void OnTriggerStay(Collider playerObject)
    {
        enableGame(playerObject);
    }
    void OnTriggerExit()
    {
        PressF.enabled = false;
    }
    
    ///<summary> Allows the player to start the puzzle and disables all player movement
    ///<para> Requires: pressF Canvas
    ///<para> Requires: keyPad Canvas
    ///<para> Requires: PlayerMovement Script
    ///<para> Requires: PasscodePuzzle Script
    ///<para> Requires: PasscodePuzzle.StartPuzzle method
    ///<para> Requires: PasscodePuzzle.PuzzleComplete bool
    private void enableGame(Collider playerObject)
    {
        PasscodePuzzle PasscodePuzzleScript = keyPad.GetComponent<PasscodePuzzle>();
        
        if(PasscodePuzzleScript.PuzzleComplete == false && keyPad.enabled == false)
        PressF.enabled = true;
        
        if(playerObject.tag == "Player" && Input.GetKeyDown("f") && keyPad.enabled == false)
        {
            keyPad.enabled = true;
            if(playerObject.GetComponent<PlayerMovement>() != null)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = false; 
            }
            PressF.enabled = false;
            PasscodePuzzleScript.StartPuzzle();
        }

        if(PasscodePuzzleScript.PuzzleComplete == true)
        {
            if(playerObject.GetComponent<PlayerMovement>() != null)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = true; 
            }
        }
    }
}
