    using UnityEngine;

    public class DebugMovement : MonoBehaviour
    {
        private Vector3 lastXROrigin;
        private Vector3 lastCameraOffset;
        private Vector3 lastCamera;

        void Start()
        {
            lastXROrigin = transform.position;
            lastCameraOffset = transform.Find("Camera Offset").position;
            lastCamera = Camera.main.transform.position;
        }

        void Update()
        {
            Vector3 xrDelta = transform.position - lastXROrigin;
            Vector3 camOffsetDelta = transform.Find("Camera Offset").position - lastCameraOffset;
            Vector3 camDelta = Camera.main.transform.position - lastCamera;

            xrDelta.y = 0;
            camOffsetDelta.y = 0;
            camDelta.y = 0;

            Debug.Log($"XR Origin moved: {xrDelta.magnitude}");
            Debug.Log($"Camera Offset moved: {camOffsetDelta.magnitude}");
            Debug.Log($"Camera moved: {camDelta.magnitude}");

            lastXROrigin = transform.position;
            lastCameraOffset = transform.Find("Camera Offset").position;
            lastCamera = Camera.main.transform.position;
        }
    }