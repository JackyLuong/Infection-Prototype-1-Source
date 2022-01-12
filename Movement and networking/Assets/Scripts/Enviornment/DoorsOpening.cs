using UnityEngine;

/*
    Plays opening and closing door animation clips every time the player collides with
    a collision detector.
*/
public class DoorsOpening : MonoBehaviour
{
    [SerializeField] private Animation doorOpeningAndClosing;
    [SerializeField] private AnimationClip [] clips; // opening and closing clips

    // Door opens when a player enteres the collision
    void OnTriggerEnter(Collider other)
    {
        if((other.transform.tag == "Player" || other.transform.tag == "Monster") && doorOpeningAndClosing.isPlaying == false)
        {
            doorOpeningAndClosing.Play(clips[0].name);
        }
    }
    
    // Door closes when the player exits the collision
    void OnTriggerExit(Collider other)
    {
        if((other.transform.tag == "Player" || other.transform.tag == "Monster") && doorOpeningAndClosing.isPlaying == false)
        {
            doorOpeningAndClosing.Play(clips[1].name);
        }
    }
}
