using UnityEngine;

public class ResetOnDrop : MonoBehaviour
{
    private Vector3 _originPos;
    private Quaternion _originRot;
    public float resetY = -5f;

    void Start()
    {
        _originPos = transform.position;
        _originRot = transform.rotation;
    }

    void Update()
    {
        if (transform.position.y < resetY)
            ResetObject();
    }

    public void ResetObject()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = _originPos;
        transform.rotation = _originRot;
    }
}