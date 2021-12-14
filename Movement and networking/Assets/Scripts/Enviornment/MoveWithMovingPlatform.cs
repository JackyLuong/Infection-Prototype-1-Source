using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMovingPlatform : MonoBehaviour
{
    [SerializeField] private float _delayTime;
    [SerializeField] private Vector3 _platformVelocity;
    
    private Vector3 _startPosition;
   
    private bool isCoroutine = false;
    
    private bool _reachedTheEnd = false;
    private bool _reachedTheStart = false;
   
    private void Start()
    {
        //Reads where the platform starts
        _startPosition = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
        {
            other.transform.SetParent(other.transform);
            other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(true);
            Debug.Log(other.name);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
        {
            other.transform.SetParent(transform);
            other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Monster")
         other.gameObject.GetComponent<PlayerMovement>().SetisGrounded(false);
        
        other.transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        StartCoroutine (MovingPlatform());
    }

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
