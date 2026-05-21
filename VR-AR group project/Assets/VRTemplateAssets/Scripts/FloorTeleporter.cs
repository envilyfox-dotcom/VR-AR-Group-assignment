using UnityEngine;

public class FloorTeleporter : MonoBehaviour
{
    public Transform destination;
    public Transform player;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.position = destination.position;

            if (cc != null)
                cc.enabled = true;

            canTeleport = false;

            Invoke(nameof(ResetTeleport), 1f);
        }
    }

    void ResetTeleport()
    {
        canTeleport = true;
    }
}