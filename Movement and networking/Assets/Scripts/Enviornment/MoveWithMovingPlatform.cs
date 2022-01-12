using System.Collections;
using UnityEngine;

/*
    Players and objects move with the platform
*/
public class MoveWithMovingPlatform : MonoBehaviour
{
    #region Class Variables
    [SerializeField] private float _delayTime; // Stops the platform each time it reaches to the end
    [SerializeField] private Vector3 _platformVelocity; // Speed that the platform travels
    
    private Vector3 _startPosition; // start position of the platform
   
    private bool isCoroutine = false;
    private bool _reachedTheEnd = false;
    private bool _reachedTheStart = false;
   #endregion

   // Assigns the start position to the current position of the platform
    private void Start()
    {
        //Reads where the platform starts
        _startPosition = transform.position;
    }

    /*
        When the player enters the platform's collision, 
        the player will move with the platform
    */
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
        {
            other.transform.SetParent(other.transform); // platform becomes the player object's parent
            other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(true); // allows player to jump on the platform 
        }
    }
    /*
        When the player stays on the platform, the player will move with the
        platform.
    */
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
        {
            other.transform.SetParent(transform);// platform becomes the player object's parent
            other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(true); // allows player to jump on the platform 
        }
    }

    /*
        When the player exits the platform's collision,
        the player is allowed to move freely without 
        being affected by the platform
    */
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
         other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(false); // player is not allowed to jump when the exits the collsion platform
        
        other.transform.SetParent(null);// the player no longer has a parent object
    }

    /*
        Constantly moves the platform
    */
    private void FixedUpdate()
    {
        StartCoroutine (MovingPlatform());
    }

    /*
        Animates the moving platform
    */
     IEnumerator MovingPlatform()
     {
        if(isCoroutine)
        yield break;

        isCoroutine = true;

        //Delay when the platform reaches the start point
        if(transform.position.x >= _startPosition.x)
        {
            _reachedTheEnd = false;
            _reachedTheStart = false;
            yield return new WaitForSeconds(_delayTime);
            _reachedTheStart = true;
        }

        //Delay when the platfrom reaches the other end
        else if(transform.position.x <= _startPosition.x - 20)
        {
            _reachedTheStart = false;
            _reachedTheEnd = false;
            yield return new WaitForSeconds(_delayTime);
            _reachedTheEnd = true;
        }

        //Moves from the start to the other end
        if(transform.position.x >= _startPosition.x - 20 && _reachedTheStart == true)
        {
            transform.position -= (_platformVelocity * Time.fixedDeltaTime);
        } 

        //Moves from the end to where it started
        else if (transform.position.x <= _startPosition.x && _reachedTheEnd == true)
        {
            transform.position += (_platformVelocity * Time.fixedDeltaTime);
        }

        //If nothing is happening, the platform stays stationary
        else 
        {
            transform.position = transform.position;
        }

         isCoroutine = false;
     }
}
