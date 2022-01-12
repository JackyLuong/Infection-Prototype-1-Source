using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Create a spawn point for players to spawn
*/
public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake() => PlayerSpawnSystem.AddSpawnPoint(transform); // adds spawn point when the class is called
    private void OnDestroy() => PlayerSpawnSystem.RemoveSpawnPoint(transform); // remove spawn when the game ends

    //Creates blue sphere as spawn points
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
