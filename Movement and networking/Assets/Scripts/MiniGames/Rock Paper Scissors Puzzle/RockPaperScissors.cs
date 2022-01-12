using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    #region Class Variables
    [SerializeField] private GameObject showPaper; // player selects paper
    [SerializeField] private GameObject showRock; // player selects rock
    [SerializeField] private GameObject showScissors; // player selects scissors

    [SerializeField] private GameObject showEnemyPaper; // CPU uses paper
    [SerializeField] private GameObject showEnemyRock; // CPU uses Rock
    [SerializeField] private GameObject showEnemyScissors; //CPU uses Scissors

    [SerializeField] private GameObject enemyTextBox; // CPU comments

    [SerializeField] private GameObject versusText; // Displays who won

    [SerializeField] private RectTransform playerHealth;
    [SerializeField] private RectTransform enemyHealth;

    [SerializeField] private GameObject puzzleParent;

    [SerializeField] private float damage; // damage to health
    [SerializeField] private int timer;

    private string rock, paper, scissors;
    private string playerDecision;
    private string enemyDecision;

    private float initialPlayerHealth;
    private float initialEnemyHealth;

    private bool isCourotine;
    #endregion

    //Disables all UI and images
    void Start()
    {
        rock = "Rock";
        paper = "Paper";
        scissors = "Scissors";
        
        disableResults();

        initialEnemyHealth = enemyHealth.rect.width; // enemy health bar
        initialPlayerHealth = playerHealth.rect.width; // player health bar
    }
    //Disable all UI and sets up the health bar for the player and CPU
    public void SetDefault()
    {
        disableResults();

        enemyHealth.sizeDelta = new Vector2(initialEnemyHealth, 20); // CPU health
        playerHealth.sizeDelta = new Vector2(initialPlayerHealth, 20); // player health

        playerDecision = null;
        enemyDecision = null;

        enemyTextBox.GetComponent<Text>().text = ""; // CPU text box
    }

    //Sets players decision to rock and displays it.
    public void chooseRock()
    {
        EnemySelects(); // CPU makes a choice
        playerDecision = rock; // player chooses rock

        showPaper.SetActive(false);
        showRock.SetActive(true);
        showScissors.SetActive(false);
    }

    //Sets players decsision to paper and displays it.
    public void choosePaper()
    {
        EnemySelects(); // CPU makes a choice
        playerDecision = paper; // player chooses paper

        showPaper.SetActive(true);
        showRock.SetActive(false);
        showScissors.SetActive(false);
    }
    
    //Sets players decsision to scissors and displays it.
    public void chooseScissors()
    {
        EnemySelects(); //  CPU makes a choise
        playerDecision = scissors; // player chooses scissors

        showPaper.SetActive(false);
        showRock.SetActive(false);
        showScissors.SetActive(true);
    }

    //Checks who won and inflicts damage accordingly
    public void CheckResults()
    {
        versusText.SetActive(true);

        //if player and enemy is the same
        if(playerDecision == enemyDecision)
        {
            versusText.GetComponent<Text>().text = "Tie";
        }

        //if player chooses rock
         else if(playerDecision == rock && enemyDecision == scissors)
         {
             versusText.GetComponent<Text>().text = "Player Wins";
             enemyHealth.sizeDelta = new Vector2(enemyHealth.rect.width - damage, 20);
         }
         else if(playerDecision == rock && enemyDecision == paper)
         {
             versusText.GetComponent<Text>().text = "Enemy Wins";
             playerHealth.sizeDelta = new Vector2(playerHealth.rect.width - damage, 20); 
         }
         
         // if player chooses paper
         else if(playerDecision == paper && enemyDecision == scissors)
         {
             versusText.GetComponent<Text>().text = "Enemy Wins";
             playerHealth.sizeDelta = new Vector2(playerHealth.rect.width - damage, 20); 
         }
         else if(playerDecision == paper && enemyDecision == rock)
         {
             versusText.GetComponent<Text>().text = "Player Wins";
             enemyHealth.sizeDelta = new Vector2(enemyHealth.rect.width - damage, 20);
         }
        
        //if player chooses scissors
         else if(playerDecision == scissors && enemyDecision == paper)
         {
             versusText.GetComponent<Text>().text = "Player Wins";
             enemyHealth.sizeDelta = new Vector2(enemyHealth.rect.width - damage, 20); 
         }
         else if(playerDecision == scissors && enemyDecision == rock)
         {
             versusText.GetComponent<Text>().text = "Enemy Wins";
             playerHealth.sizeDelta = new Vector2(playerHealth.rect.width - damage, 20);
         }
         
        StartCoroutine(IfEitherWon());
    }

    //Controls what the enemy selects base on random actions
    void EnemySelects()
    {
        if(enemyHealth.rect.width == initialEnemyHealth && playerHealth.rect.width == initialPlayerHealth)
        {
            int randomChoice = Random.Range(1, 3);

            if(randomChoice == 1)
            {
                enemyDecision = rock;
            }
            else if(randomChoice == 2)
            {
                enemyDecision = paper;
            }

            else if(randomChoice == 3)
            {
                enemyDecision = scissors;
            }  
        }
         else
         {
            int randomChoice = Random.Range(1, 3);

            if(randomChoice == 1)
            {
                enemyTextBox.GetComponent<Text>().text = "I wont let you use the same move and win.";
                if(playerDecision == rock)
                {
                    enemyDecision = paper;
                } 

                 else if(playerDecision == paper)
                 {
                    enemyDecision = scissors;
                 }

                 else if (playerDecision == scissors)
                 {
                    enemyDecision = rock;
                 }
            }

             else if(randomChoice == 2)
             {
                enemyTextBox.GetComponent<Text>().text = "I like the move you just use.";
                enemyDecision = playerDecision;
             }

             else if(randomChoice == 3)
             {
                enemyTextBox.GetComponent<Text>().text = "I'm sticking with this'";
             } 
         }

        if(enemyDecision == rock)
        {
            showEnemyPaper.SetActive(false);
            showEnemyRock.SetActive(true);
            showEnemyScissors.SetActive(false);
        }
         
         else if(enemyDecision == paper)
         {
            showEnemyPaper.SetActive(true);
            showEnemyRock.SetActive(false);
            showEnemyScissors.SetActive(false);
         }
         else if(enemyDecision == scissors)
         {
            showEnemyPaper.SetActive(false);
            showEnemyRock.SetActive(false);
            showEnemyScissors.SetActive(true);
         }
         Debug.Log("Enemy: " + enemyDecision);
    }

    IEnumerator IfEitherWon()
    {
        if(isCourotine)
        yield break;

        isCourotine= true;

        if(playerHealth.rect.width <= 0)
        {
            this.enabled = false;
            yield return new WaitForSeconds(timer);
            puzzleParent.GetComponent<RockPaperScissorsPuzzleTrigger>().puzzleFailed = true;
            gameObject.SetActive(false);
        } 
         else if(enemyHealth.rect.width <= 0)
         {
            this.enabled = false;
            yield return new WaitForSeconds(timer);
            puzzleParent.GetComponent<RockPaperScissorsPuzzleTrigger>().puzzleCompleted = true;
            gameObject.SetActive(false);
         }

        isCourotine = false;
    }

    private void disableResults()
    {
        showPaper.SetActive(false);
        showRock.SetActive(false);
        showScissors.SetActive(false);

        showEnemyPaper.SetActive(false);
        showEnemyRock.SetActive(false);
        showEnemyScissors.SetActive(false);

        versusText.SetActive(false);
    }
}
