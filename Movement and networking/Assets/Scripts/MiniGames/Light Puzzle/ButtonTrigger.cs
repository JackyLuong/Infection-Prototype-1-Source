using UnityEngine;
/*
    This puzzle has the player wait for any light to turn blue before pressing f
    Player has to time the lights properly 3 times to complete the puzzle.
    If the player decides to press f when there are not blue lights, the puzzle resets
*/
public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private Canvas pressF;
    [SerializeField] private GreenLight LightScript;
    
    private bool isPlayerAtButton = false; // Checks if the player is at the puzzle. 
                                           //In order for the player to use the puzzle, 
                                           //they have to enter the collision trigger or be near the puzzle to play.
    
    // Allows player to interact with the puzzle when they are near it
    void OnTriggerEnter()
    {
        isPlayerAtButton = true; // allows player to play with the puzzle
        
        if(LightScript.puzzleComplete == false) // disables the game if its already complete
        pressF.enabled = true;
    }

    // Disables the game if its already completed
    void OnTriggerStay()
    {
        if(LightScript.puzzleComplete == true)
        {
            pressF.enabled = false;
        }
    }
    // Disables the press f icon and prevents the player from interacting with the puzzle
    void OnTriggerExit()
    {
        isPlayerAtButton = false;
        pressF.enabled = false;
    }

    // isPlayerAtButton getter
    public bool GetIsPlayerAtButton()
    {
        return isPlayerAtButton;
    }
}
