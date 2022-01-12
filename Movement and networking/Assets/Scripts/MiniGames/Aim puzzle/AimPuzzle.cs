using UnityEngine;
/*
    This puzzle has the player click on targets displayed on the screen.
*/
/*
    Checks of the player is near the mini game and allows players to activate and deactivate the aim
    puzzle.
*/
public class AimPuzzle : MonoBehaviour
{
    #region Class Variables
    [SerializeField] private Canvas pressF; // F icon on the screen to tell the user that 
                                            //they need to press f to activate the minigame
    [SerializeField] private Canvas puzzleUI;// Opens canvas the contains the game

    private bool puzzleStarted;
    public int numberOfTargetHit = 0;
    #endregion

    // Disables all UI
    void Start()
    {
        pressF.enabled = false;
        puzzleUI.enabled = false;
    }

    //Game Can start the second they enter the trigger
    void OnTriggerEnter(Collider playerObject)
    {
        enableGame(playerObject); // start game
    }
    // Allows the player to start the puzzle and disables all player movement when they stay on the trigger
    void OnTriggerStay(Collider playerObject)
    {
        enableGame(playerObject); // start game

        if(numberOfTargetHit >= 20 && playerObject.tag == "Player") // Allows the player to move after the game is done 
        {
            if(playerObject.transform.GetComponent<PlayerMovement>() != null)
            {
                playerObject.transform.GetComponent<PlayerMovement>().enabled = true;
            }
            pressF.enabled = false;
            this.enabled = false; // prevents the player from playing the game after they completed it
            puzzleUI.enabled = false;
        }
        
    }

    // Disables the press f icon when the leave the trigger
    void OnTriggerExit()
    {
        pressF.enabled = false;
    }

    // Allows the player to start the mini game
    private void enableGame(Collider playerObject)
    {
        // show 
        if(puzzleStarted == false)
            pressF.enabled = true; // show press f icon
            
        if(playerObject.tag == "Player" && puzzleStarted == false && Input.GetKeyDown("f")) // start game when player presses F
        {
            pressF.enabled = true;
            puzzleUI.enabled = true;
            puzzleStarted = true;
            if(playerObject.transform.GetComponent<PlayerMovement>() != null)
            {
                playerObject.transform.GetComponent<PlayerMovement>().enabled = false;//disable player movement
            }
        }
    }
}
