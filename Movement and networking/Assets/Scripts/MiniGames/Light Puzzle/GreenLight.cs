using UnityEngine;
using System.Collections;

/*
    Handles how the lights change base on the player's timing
*/
public class GreenLight : MonoBehaviour
{
    
    #region Class Variables 
    public Transform LightPuzzle; // light tower
    public GameObject[] Lights; // light blocks
    public Material[] LightColour; // colours the lights can change to

    public float DelayTime;

    [SerializeField] private string ActionKey = "f";
    [SerializeField] private ButtonTrigger _buttonTrigger;

    private bool isCoroutineExecuting = false; // lights are changing to blue
    private bool isCoroutineExecutingRed = false; // lights are changing to red
    private bool blueLightPattern = true;
    public bool puzzleComplete = false;

    private int greenLightsLit = 0; // number of lights that are green
    private int incorrectLight = 0; // counts ligths that are not blue 
    #endregion

    ///<summary> This is the light puzzle where the player needs to time the lights and ensure that all the lights turn green.
    ///<para> On idle, the puzzle's blue light will rotate upwards form light to light.
    ///<para> If the player presses the button when there are no blue buttons, the puzzle will turn red and reset.
    ///<para> If the player presses the button when there is a blue light, it will replace it with a green light.
    ///<para> Requires: ButtonTrigger script
    ///<para> Requires: ButtonTrigger.GetIsPlayerAtButton method
    ///</summary>

    //constantly checks what lights are green, which one shoud fluctuate on and off, and when to reset the puzzle
    void Update()
    {
        //Consantly loops through all the lights
        for (int i = 0; i < 3; i ++)
        {
            //Changes Colour to Green if the square is blue
            if(Lights[i].GetComponent<MeshRenderer>().material.name.Equals(LightColour[0].name + " (Instance)") 
                && Input.GetKeyDown(KeyCode.F) && _buttonTrigger.GetIsPlayerAtButton() == true) 
            {
                Lights[i].GetComponent<MeshRenderer>().material = LightColour[1]; //  change to green
                incorrectLight = 0; // resets the counter
            } 
            
            //If the Light isnt blue then it will be placed in the counter. If the counter adds up to 3 then the puzzle resets.
            else if (!Lights[i].GetComponent<MeshRenderer>().material.name.Equals(LightColour[0].name + " (Instance)") 
                        && Input.GetKeyDown(KeyCode.F) && _buttonTrigger.GetIsPlayerAtButton() == true)
            {
                incorrectLight ++;
            } 
            
            // The puzzle on idle will have the blue light fluctuate from bottom-up. 
            else 
            {
                blueLightPattern = true;
                greenLightsLit = 0;
            }
            //Checks what lights are green and adds them to the counter
            if(Lights[i].GetComponent<MeshRenderer>().material.name.Equals(LightColour[1].name + " (Instance)"))
            {
                greenLightsLit ++;
            }
        }
        // puzzle ends when all the lights are green
        if (greenLightsLit >= 3)
        {
            puzzleComplete = true;
        }
        //Resets the puzzle and lets the user know that they puzzle is resetting by turning all the lights to red. 
        else if(incorrectLight >= 3 && puzzleComplete == false)
        {
            StartCoroutine (ChangeLightToRed(DelayTime));
        } 
        
        //blue pattern is continued if nothing else is happening.
        else if (blueLightPattern == true && incorrectLight < 3)
        {
            StartCoroutine (ChangeLightToBlue(DelayTime));
        }
    }
    // changes all the lights to red and resets the puzzle
    IEnumerator ChangeLightToRed(float time)
    {
        if(isCoroutineExecutingRed)
        yield break;

        isCoroutineExecutingRed = true;

        for (int i = 0; i < 3; i ++)
        {
            Lights[i].GetComponent<MeshRenderer>().material = LightColour[2]; // change all lights to red
        }
        yield return new WaitForSeconds(time);

        for (int i = 0; i < 3; i ++)
        {
            Lights[i].GetComponent<MeshRenderer>().material = LightColour[3]; // turn all lights off
        }
        yield return new WaitForSeconds(time);

        incorrectLight = 0;

        isCoroutineExecutingRed = false;
    }

    //Changes lights to blue to let the player know when the press f
    IEnumerator ChangeLightToBlue(float time)
    {
        if(isCoroutineExecuting)
        yield break;

        isCoroutineExecuting = true;
        
        for (int i = 0; i < 3; i ++)
        {
            // Consantly flucuates the lights form blue to off. Will not change ones that are already green
            if(!Lights[i].GetComponent<MeshRenderer>().material.name.Equals(LightColour[1].name + " (Instance)")) 
            {
                Lights[i].GetComponent<MeshRenderer>().material = LightColour[0]; // change colour to blue
                yield return new WaitForSeconds(time);
               
                if(!Lights[i].GetComponent<MeshRenderer>().material.name.Equals(LightColour[1].name + " (Instance)"))
                    Lights[i].GetComponent<MeshRenderer>().material = LightColour[3]; // turn lights off
                
                yield return new WaitForSeconds(time);
            } 
            
        }
        isCoroutineExecuting = false;
    }
}
