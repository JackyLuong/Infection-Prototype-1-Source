using UnityEngine;
using UnityEngine.UI;

    /* 
        Constantly updates and displays the number of targets hit
    */
public class ShowTargetHit : MonoBehaviour
{
    [SerializeField] AimPuzzle aimPuzzleScript;
    void Update()
    {
        gameObject.GetComponent<Text>().text = aimPuzzleScript.numberOfTargetHit.ToString(); // display number of targets hit
    }
}
