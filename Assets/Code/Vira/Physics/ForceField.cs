using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [SerializeField] float _force;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 target = _transform.position - other.transform.position;
        target = target.x * Vector3.right + target.z * Vector3.forward;
        Vector3 velocity = other.attachedRigidbody.velocity;
        velocity = velocity.y * Vector3.up;
        other.attachedRigidbody.velocity = velocity;

        other.attachedRigidbody.AddForce(target *  _force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 target = _transform.position - other.transform.position;
        target = target.x * Vector3.right + target.z * Vector3.forward;
        Vector3 velocity = other.attachedRigidbody.velocity;
        velocity = velocity.y * Vector3.up;
        other.attachedRigidbody.velocity = velocity;

        other.attachedRigidbody.AddForce(target *  + _force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

}
