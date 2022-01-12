using UnityEngine;
using UnityEngine.UI;
/*
    Spawns and checks of the player has hit the target
*/
public class TargetPressed : MonoBehaviour
{
    #region Class Variables
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private float radius;
    [SerializeField] private GameObject target;
    [SerializeField] private AimPuzzle aimPuzzleScript;
    [SerializeField] private Playsound playSound;

    private GameObject newTarget;
    #endregion

    /*
        Spawns targets player needs to hit. This is done by hiding the target and relocating it.
    */
    public void SpawnTarget()
    {
        if(IsInCircumference() == true)
        {
            Vector2 targetPosition = new Vector2(Random.Range(-width, width), Random.Range(-height, height));
        
            target.SetActive(false);
        
            target.transform.localPosition = targetPosition;

            playSound.Clicky();
        
            target.SetActive(true);
        
            aimPuzzleScript.numberOfTargetHit++;
        }
    }

    ///<summary> Checks if the player is hitting the circle area of the button
    ///<para> Requires: target GameObject
    ///</summary>
    private bool IsInCircumference()
    {
        float DeltaX = Input.mousePosition.x - target.transform.position.x;
        float DeltaY = Input.mousePosition.y - target.transform.position.y;
        
        if((DeltaX * DeltaX) + (DeltaY * DeltaY) <= radius * radius)
        {
            target.GetComponent<Button>().interactable = true;
            return true;
        }
        else 
        {
            target.GetComponent<Button>().interactable = false;
            return false;
        }
    }
    
    // Constantly checks of the player's cursor is within the target
    void Update()
    {
        IsInCircumference();
    }
}
