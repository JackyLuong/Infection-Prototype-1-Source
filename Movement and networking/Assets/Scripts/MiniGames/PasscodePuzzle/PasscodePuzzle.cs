using UnityEngine;
using UnityEngine.UI;
/*
    This puzzle has the player create the correct word.
    each column have randomly generated letters and the player has to organize these 
    letter to create a word.
*/
public class PasscodePuzzle : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform[] columns;
    [SerializeField] private string[] differentPasscodeOptions; // different answers

    public bool PuzzleComplete = false;
    private int SetRandomLetterToRightOne; // set the right letter in each column
    private string passcode; // password
    private string previousFirstLetter; // cycles the first letter down to the bottom when the player cycles the letters up 
    private string previousLastLetter; // moves last letter to the top when the player cycles the letters down
    private string[] Letters = new string[5];
    private string[] Alphabet = new string[26] 
    {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
    #endregion
    // Displays random letter in each column and sets the letter needed to complete the puzzle.
    public void StartPuzzle()
    {   
        passcode = differentPasscodeOptions[Random.Range(0, (differentPasscodeOptions.Length - 1) )]; // selects a random word for the puzzle

        foreach(Transform column in columns) // Gets each column in array.
        {
            SetRandomLetterToRightOne = Random.Range(1, 5); // one of the letters in the column will be the correct letter

            while(SetRandomLetterToRightOne == 3) // Ensures that not all the letters for the passcode is at the centre.
            SetRandomLetterToRightOne = Random.Range(1, 5);

            foreach(Transform child in column) // Gets each children in Column.
            {
                if(child.name.StartsWith("Letter"))
                {
                    if(child.parent.name == "Column 1" && child.name == ("Letter " + SetRandomLetterToRightOne)) // Sets Passocode Letter to first column.
                    {
                        child.gameObject.GetComponentInChildren<Text>().text = passcode.Substring(0,1); 
                    }
                    else if(child.parent.name == "Column 2" && child.name == ("Letter " + SetRandomLetterToRightOne)) // Sets Passocode Letter to second column.
                    {
                        child.gameObject.GetComponentInChildren<Text>().text = passcode.Substring(1,1);
                    }
                    else if(child.parent.name == "Column 3" && child.name == ("Letter " + SetRandomLetterToRightOne)) // Sets Passocode Letter to third column.
                    {
                        child.gameObject.GetComponentInChildren<Text>().text = passcode.Substring(2,1);
                    }
                    else
                    {
                        child.gameObject.GetComponentInChildren<Text>().text = Alphabet[Random.Range(0, (Alphabet.Length - 1) )]; // Sets random to every other letter box.
                    }
                }
            }
        }
    }
    // Read the current letters and move them all Down.
    public void MoveLetterDown(int columnNumber)
    { 
        ReadCurrentLetterPositions(columnNumber); // get current letter position
        for(int i = Letters.Length - 1; i >= 0; i--)
        {
            if(i == 0)
            {
                Letters[i] = previousLastLetter; // move first letter down to the last
            }
            else 
            {
                Letters[i] = Letters[i - 1]; // move letters up
            }
        }
        SetNewLetterPositions(columnNumber); // set new array order
    }

    // Read the current letters and move them all up.
    public void MoveLetterUP(int columnNumber)
    { 
        ReadCurrentLetterPositions(columnNumber);
        for(int i = 0; i < Letters.Length; i++)
        {
            if(i == Letters.Length - 1)
            {
                Letters[i] = previousFirstLetter;
            }
            else 
            {
                Letters[i] = Letters[i + 1];
            }
        }
        SetNewLetterPositions(columnNumber);
    }

    // Check the letters in the middle and match with the passcode
    public void VerifyPasscode()
    {
        string playerPasscode = "";
        foreach(Transform column in columns)
        {
            foreach(Transform child in column)
            {
                if(child.name == ("Letter " + 3)) // gets letter in the middle
                {
                    playerPasscode += child.gameObject.GetComponentInChildren<Text>().text;
                }
            }
        }
        if(playerPasscode == passcode) // puzzle is complete if the letters match with the passcode
        {
            Debug.Log("Pass");
            PuzzleComplete = true;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("fail");
        }
    }

    // get the current letters in a column.
    private void ReadCurrentLetterPositions(int i)
    {
        int letterOrder = 0; // counter to set the current order of the letters
        foreach(Transform letter in columns[i]) // sets the letter in the array
        {
            if(letter.name.StartsWith("Letter")) // gets all letter objects
            {
                Letters[letterOrder] = letter.gameObject.GetComponentInChildren<Text>().text; //put letter into array
            }
            letterOrder ++;
        }
        previousFirstLetter = Letters[0];
        previousLastLetter = Letters[Letters.Length - 1];
    }

    // Sets the new Position of the letters.
    private void SetNewLetterPositions(int i)
    {
        int letterOrder = 0;
        foreach(Transform letter in columns[i])
        {
            if(letter.name.StartsWith("Letter"))
            {
                letter.gameObject.GetComponentInChildren<Text>().text = Letters[letterOrder]; // puts letters into an array
            }
            letterOrder ++;
        }  
    }
}
