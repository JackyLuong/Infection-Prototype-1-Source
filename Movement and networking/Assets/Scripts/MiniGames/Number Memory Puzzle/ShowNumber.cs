using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
    Dipslay random Numbers
*/
public class ShowNumber : MonoBehaviour
{
    #region Class Variables
    [SerializeField] private Canvas keyPad;
    [SerializeField] private float DelayTime;
    [SerializeField] private Button[] keyPadButtons;
    [SerializeField] private Canvas PressF;

    public TMP_Text numberOnKeypad;
    private int numberOfDigits;
    private GameObject player;
    public int randomNumber;
    private int numberPlayerPressed;
    private string previousNumbers;
    #endregion

    // The puzzle starts off with one digit and deactivates the UI for the puzzle.
    void Start()
    {
        numberOfDigits = 1;
        keyPad.enabled = false;
    }
    //player can start the game when the enter the trigger
    void OnTriggerEnter(Collider playerObject)
    {
        enableGame(playerObject);
    }
    //player can start the game when the stays the trigger
    void OnTriggerStay(Collider playerObject)
    {
        enableGame(playerObject);
    }
    //Disable press f icon
    void OnTriggerExit()
    {
        PressF.enabled = false;
    }
    // Clear display on keypad
    private void DefaultNumbers()
    {
        numberOnKeypad.text = "";
    }
    //When the player is close enough to the puzzle, they can activate it by pressing "f" and start the puzzle.
    private void enableGame(Collider playerObject)
    {
        if(numberOfDigits <= 6 && keyPad.enabled == false)
        PressF.enabled = true;

        if(playerObject.tag == "Player" && Input.GetKeyDown("f") && keyPad.enabled == false) // player starts game by pressing f
        {
            keyPad.enabled = true;
            if(playerObject.transform.GetComponent<PlayerMovement>() != null)
            {
                player = playerObject.gameObject;
                playerObject.transform.GetComponent<PlayerMovement>().enabled = false; // disable player movement
            }
            PressF.enabled = false;
            
            ShowNewNumbers();
        }
    }
    // Generates a random Number and adds onto the previous numbers that the player has to memorize. It will display it for the player for them to see.
    private void DisplayNumber()
    {
        randomNumber = UnityEngine.Random.Range(0, 9); // random number
        numberOnKeypad.text += previousNumbers + randomNumber.ToString(); // new number
        numberOfDigits ++;
        previousNumbers = numberOnKeypad.text; // adds the new number as the new previousNumber
    }
    
    // Shows a new number till it exceeds 6 digits
    public void ShowNewNumbers()
    {
        if(numberOfDigits <= 6)
        {
            StartCoroutine(ChangeNumber(DelayTime));  //display new number
        }  
        else 
        {
            player.transform.GetComponent<PlayerMovement>().enabled = true; // enable player movement
        }
    }

    // Shows previous number
    public void ShowRepeatNumbers()
    {
        if(numberOfDigits <= 6)
        {
            StartCoroutine(RepeatNumber(DelayTime));  //display previous number
        }  
    }
    
    // Delays the time it takes to display and hide the number as well as timing when the buttons are interactable
    public IEnumerator ChangeNumber(float time)
    {
        for(int i = 0; i < keyPadButtons.Length; i++)
        {
            keyPadButtons[i].interactable = false; // disable keypad
        }

        DefaultNumbers(); // clear display
        yield return new WaitForSeconds(time/5);

        DisplayNumber(); // show new number
        yield return new WaitForSeconds(time);

        DefaultNumbers(); // clear display

        for(int i = 0; i < keyPadButtons.Length; i++)
        {
            keyPadButtons[i].interactable = true; // enable keypad
        }
    }

    // Repeats previous number if the player gets it wrong
    public IEnumerator RepeatNumber(float time)
    {
        for(int i = 0; i < keyPadButtons.Length; i++)
        {
            keyPadButtons[i].interactable = false; // disable keypad
        }

        DefaultNumbers(); // clear display
        yield return new WaitForSeconds(time/5);

        numberOnKeypad.text = previousNumbers; // show previous number
        yield return new WaitForSeconds(time);

        DefaultNumbers(); // clear display

        for(int i = 0; i < keyPadButtons.Length; i++)
        {
            keyPadButtons[i].interactable = true; // enable keypad
        }
    }
    // number of digits getter
    public int GetNumberOfDigits()
    {
        return numberOfDigits;
    }
}
