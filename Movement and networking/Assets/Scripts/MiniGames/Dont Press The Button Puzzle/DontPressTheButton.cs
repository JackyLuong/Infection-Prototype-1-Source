using System.Collections;
using UnityEngine;
/*
    This puzzle has the player wait for the button till it turns green before they are able to press the button.
*/
/*
    Checks if the player is near the game and allows the player to activate and deactivate the game.
    Constantly changes the colour of the bottom from red to green and checks if the player has 
    timed input properly. 
*/
public class DontPressTheButton : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject button;
    [SerializeField] private Canvas pressF;

    public Material[] LightColour;
    
    private bool puzzleStarted;

    private bool isChangingColour;

    private bool turnOnButton;

    private bool buttonPressed = false;
    #endregion

    ///<summary> When the player is close enough to the puzzle, they can activate it by pressing "f" and start the puzzle.
    ///<para> Requires: pressF Canvas
    ///<para> Requires: puzzleStarted bool
    ///</summary>
    void OnTriggerStay(Collider playerObject)
    {

        if(playerObject.tag == "Player" && puzzleStarted == false)
        {
            pressF.enabled = true;
            puzzleStarted = true;
            Debug.Log("PuzzleStarted");
        }
    }

    // Button changes back to red and press f icon is hidden when the player leaves the trigger
    void OnTriggerExit()
    {
        pressF.enabled = false; // disable press f icon
        
        if(buttonPressed == false)
        {
            button.GetComponent<MeshRenderer>().material = LightColour[1]; // change button back to red
            puzzleStarted = false;
        }
    }

    // Changes the button colour to red or green and checks if the player presses "f" when it turns green
    void Update()
    {
        if (puzzleStarted == true) // game started
        {
            turnOnButton = (Random.value < 0.5); // generates random number to turn the button red or green
            
            if(turnOnButton == true) // turn button green
            {
                StartCoroutine(ChangeColourToGreen());
            } 
            else // turn button red
            {
                StartCoroutine(ChangeColourToRed());
            }
            
            //The button will stay green if the player presses f when the button turns green. The puzzle will be completed
            if(Input.GetKeyDown("f") && button.GetComponent<MeshRenderer>().material.name.Equals(LightColour[0].name + " (Instance)") && buttonPressed == false)
            {
                buttonPressed = true;
                pressF.enabled = false;
                isChangingColour = true;

                Debug.Log("Pass");
            } 
            else if(Input.GetKeyDown("f") && buttonPressed == false) // Button will stay red if the player is constantly pressing f
            {
                turnOnButton = false;
                StartCoroutine(ChangeColourToRed());
                
                Debug.Log("Failed");
            }
        } 
    }

    // Changes the button colour to green and has a random time
    IEnumerator ChangeColourToGreen()
    {
        if(isChangingColour)
        yield break;

        isChangingColour = true;

        if(buttonPressed == false)
        {
            turnOnButton = true;
            float randomTime = Random.Range(0.1f, 1.5f); // random delay

            button.GetComponent<MeshRenderer>().material = LightColour[0]; // change button colour to green
            yield return new WaitForSeconds(randomTime); // delay
        }
        
        isChangingColour = false;
    }
    
    // Changes the button colour to red and has a random time
        IEnumerator ChangeColourToRed()
    {
        if(isChangingColour)
        yield break;

        isChangingColour = true;

        if(buttonPressed == false)
        {
            turnOnButton = false;
            float randomTime = Random.Range(1f, 4f); // randome delay time

            button.GetComponent<MeshRenderer>().material = LightColour[1]; // change colour to red
            yield return new WaitForSeconds(randomTime); // delay

        }
        isChangingColour = false;
    }
}
