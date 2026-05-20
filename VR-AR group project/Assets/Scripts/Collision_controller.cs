using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CharacterController))]
public class Collision_controller : MonoBehaviour
{
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private float height = 1.8f;
    [SerializeField] private LayerMask collisionMask;

    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 headPosition = xrOrigin.Camera.transform.position;
        Vector3 lowerPosition = new Vector3(headPosition.x, transform.position.y, headPosition.z);

        Collider[] hits = Physics.OverlapCapsule(lowerPosition, headPosition, radius, collisionMask);

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(transform) || hit.transform == transform)
                continue;

            if (Physics.ComputePenetration(
                _controller, transform.position, transform.rotation,
                hit, hit.transform.position, hit.transform.rotation,
                out Vector3 direction, out float distance))
            {
                transform.position += direction * distance;
            }
        }
    }
}