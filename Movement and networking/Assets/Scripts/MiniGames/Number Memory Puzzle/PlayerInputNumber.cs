using UnityEngine;
/*
    This puzzle has the player memorize the numbers on the screen and type it in.
*/
/*
    Players press numbers on the keypad and is displayed
*/
public class PlayerInputNumber : MonoBehaviour
{
    [SerializeField] ShowNumber displayNumber;

    public int PlayerNumberInput;

    [SerializeField] private int DelayTime;
    
    public void Display1()
    {
        PlayerNumberInput = 1;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display2()
    {
        PlayerNumberInput = 2;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display3()
    {
        PlayerNumberInput = 3;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display4()
    {
        PlayerNumberInput = 4;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display5()
    {
        PlayerNumberInput = 5;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display6()
    {
        PlayerNumberInput = 6;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display7()
    {
        PlayerNumberInput = 7;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display8()
    {
        PlayerNumberInput = 8;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display9()
    {
        PlayerNumberInput = 9;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    public void Display0()
    {
        PlayerNumberInput = 0;
        displayNumber.numberOnKeypad.text += PlayerNumberInput.ToString();
    }

    //Checks if the player's input matches with the number that was displayed
    public void CheckPlayerInput()
    {
        if(PlayerNumberInput == displayNumber.randomNumber)
        {
            Debug.Log("Correct");
            if(displayNumber.GetNumberOfDigits() <= 6)
            {
                displayNumber.ShowNewNumbers(); // shower a new number
            } 
            else //Puzzle is complete
            {
                displayNumber.ShowNewNumbers();
                gameObject.SetActive(false);
            }
        } 
        else
        {
            displayNumber.ShowRepeatNumbers();
        }
    }

    //Clears the keypad display
    public void Clear()
    {
        displayNumber.numberOnKeypad.text = "";
    }
}
