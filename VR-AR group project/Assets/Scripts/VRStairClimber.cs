using UnityEngine;

/// <summary>
/// VR Stair Climber
/// Attach to your VR Rig / Player root GameObject.
/// Requires a CharacterController on the same GameObject.
/// Works with any locomotion system that moves the rig via transform or CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRStairClimber : MonoBehaviour
{
    [Header("Step Detection")]
    [Tooltip("Maximum height the player can step up (match your stair riser height)")]
    public float maxStepHeight = 0.4f;

    [Tooltip("How far forward to probe for a step")]
    public float stepDepthProbe = 0.3f;

    [Tooltip("Layers considered as walkable geometry")]
    public LayerMask walkableLayers = ~0;

    [Header("Step Smoothing")]
    [Tooltip("How fast the player is lifted onto the step (higher = snappier)")]
    public float stepSmoothing = 10f;

    [Header("Debug")]
    public bool showGizmos = true;

    // ── Private state ───────────────────────────────────────────────────────
    private CharacterController _cc;
    private float _targetStepOffset;   // accumulated vertical lift this frame
    private Vector3 _lastPosition;

    // ── Unity lifecycle ─────────────────────────────────────────────────────
    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _lastPosition = transform.position;
    }

    void Update()
    {
        if (_cc.isGrounded)
            TryClimbStep();
    }

    // ── Core logic ───────────────────────────────────────────────────────────

    /// <summary>
    /// Fires two rays in the player's movement direction:
    ///   1. Low ray  – detects a vertical face (the stair riser)
    ///   2. High ray – confirms there is floor above (the stair tread)
    /// If both conditions are met we smoothly push the CC upward.
    /// </summary>
    void TryClimbStep()
    {
        // Direction of travel this frame
        Vector3 moveDir = transform.position - _lastPosition;
        _lastPosition = transform.position;

        // Only bother when actually moving
        if (moveDir.sqrMagnitude < 0.00001f) return;

        Vector3 moveDirFlat = new Vector3(moveDir.x, 0f, moveDir.z).normalized;

        // ── 1. Low ray: look for a wall/riser just below maxStepHeight ──────
        Vector3 lowOrigin = transform.position + Vector3.up * 0.05f;

        if (!Physics.Raycast(lowOrigin, moveDirFlat, out RaycastHit lowHit,
                             _cc.radius + stepDepthProbe, walkableLayers))
            return; // no obstacle ahead → nothing to step over

        // ── 2. High ray: check if there is a walkable surface above ─────────
        Vector3 highOrigin = transform.position
                           + Vector3.up * (maxStepHeight + 0.05f)
                           + moveDirFlat * (_cc.radius + stepDepthProbe);

        if (!Physics.Raycast(highOrigin, Vector3.down, out RaycastHit highHit,
                             maxStepHeight, walkableLayers))
            return; // no tread surface found

        float stepHeight = highHit.point.y - transform.position.y;

        // Clamp to valid step range
        if (stepHeight <= 0f || stepHeight > maxStepHeight) return;

        // ── 3. Smoothly lift the character onto the step ────────────────────
        Vector3 stepUp = Vector3.up * stepHeight;
        _cc.Move(Vector3.Lerp(Vector3.zero, stepUp, Time.deltaTime * stepSmoothing));
    }

    // ── Optional Gizmos ──────────────────────────────────────────────────────
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!showGizmos || _cc == null) return;

        Vector3 forward = transform.forward;
        Vector3 lowOrigin = transform.position + Vector3.up * 0.05f;
        Vector3 highOrigin = transform.position
                           + Vector3.up * (maxStepHeight + 0.05f)
                           + forward * (_cc.radius + stepDepthProbe);

        // Low probe (red)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(lowOrigin, forward * (_cc.radius + stepDepthProbe));

        // High probe (green)
        Gizmos.color = Color.green;
        Gizmos.DrawRay(highOrigin, Vector3.down * maxStepHeight);
    }
#endif
}