using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperScissorsPuzzleTrigger : MonoBehaviour
{
#region Variables
    [SerializeField] private Canvas pressF;
    [SerializeField] private Canvas puzzleUI;

    [SerializeField] private GameObject puzzlePanel;

    private bool puzzleStarted = false;
    public bool puzzleCompleted = false;

    public bool puzzleFailed = false;
 
    #endregion

    ///<summary> Disables all UI
    ///<para> Requires: pressF Canvas
    ///<para> Requires: puzzleUI Canvas
    ///</summary>
    void Start()
    {
        pressF.enabled = false;
        puzzleUI.enabled = false;
    }
    void OnTriggerEnter(Collider playerObject)
    {
        enableGame(playerObject);
    }
    void OnTriggerStay(Collider playerObject)
    {
        enableGame(playerObject);

        if(puzzleCompleted == true)
        {
            if(playerObject.GetComponent<PlayerMovement>() != null)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = true; 
            }
            pressF.enabled = false;
            this.enabled = false;
            puzzleUI.enabled = false;
        } 
         else if(puzzleFailed == true)
         {
            if(playerObject.GetComponent<PlayerMovement>() != null)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = true; 
            }
            pressF.enabled = false;
            this.enabled = false;

            puzzlePanel.GetComponent<RockPaperScissors>().SetDefault();

            puzzleUI.enabled = false;
            puzzleStarted = false;
            puzzleFailed = false;
         }
    }
    void OnTriggerExit()
    {
        pressF.enabled = false;
    }
    ///<summary> Allows the player to start the puzzle and disables all player movement
    ///<para> Requires: pressF Canvas
    ///<para> Requires: puzzleUI Canvas
    ///<para> Requires: PlayerMovement Script
    ///<para> Requires: numberOfTargetHit Integer
    ///</summary>
    private void enableGame(Collider playerObject)
    {
        if(puzzleStarted == false)
        pressF.enabled = true;
            
        if(playerObject.tag == "Player" && puzzleStarted == false && Input.GetKeyDown("f"))
        {
            pressF.enabled = true;
            puzzleUI.enabled = true;
            puzzleStarted = true;
            puzzlePanel.SetActive(true);

            if(playerObject.GetComponent<PlayerMovement>() != null)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = false; 
            }

            Debug.Log("PuzzleStarted");
        }
    }
}
