using _Project._Scripts.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoplegangerAbility : Ability
{
    [SerializeField] GameObject dopleganger;
    [SerializeField] Rigidbody rb;

    private GameObject doplegangerInstance = null;
    // Update is called once per frame
    public override void ActivateAbility()
    {
        if (doplegangerInstance == null)
        {
            doplegangerInstance = Instantiate(dopleganger, transform.position + Vector3.up, transform.rotation);
            return;
        }

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);

        var _carPosition = transform.position;
        var _carRotation = transform.rotation;

        rb.position = doplegangerInstance.transform.position;
        rb.rotation = doplegangerInstance.transform.rotation;

        doplegangerInstance.transform.position = _carPosition + Vector3.up;
        doplegangerInstance.transform.rotation = _carRotation;

        rb.velocity = rb.rotation * localVelocity;
        //rb.angularVelocity = doplegangerInstance.transform.TransformDirection(localAngularVelocity);
    }
}
