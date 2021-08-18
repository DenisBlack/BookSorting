using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyFIeld : MonoBehaviour
{
    [SerializeField] private float _buoyancyForce;
    float _time = 1.5f;


    private Vector3 _buoyancyVector = Vector3.up;
    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        _time += Time.fixedDeltaTime / 3;
        other.attachedRigidbody.AddForce(_buoyancyVector * Mathf.Abs(Mathf.Exp(_time)) * (Physics.gravity.y * -1 + _buoyancyForce), ForceMode.Acceleration);
    }
    private void OnTriggerStay(Collider other)
    {

        _time += Time.fixedDeltaTime / 3;
        other.attachedRigidbody.AddForce(_buoyancyVector * Mathf.Abs(Mathf.Exp(_time))   * (Physics.gravity.y * -1 + _buoyancyForce), ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        _time = 0.1f;
        other.attachedRigidbody.velocity = other.attachedRigidbody.velocity  / 3;
    }
    

}
