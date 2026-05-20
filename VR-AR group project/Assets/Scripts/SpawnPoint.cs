using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // draws a visible icon in the editor so you can see where the spawn is
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}