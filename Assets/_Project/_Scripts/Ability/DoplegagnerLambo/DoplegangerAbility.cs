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

        var _carPosition = transform.position;
        var _carRotation = transform.rotation;

        transform.position = doplegangerInstance.transform.position;
        transform.rotation = doplegangerInstance.transform.rotation;

        doplegangerInstance.transform.position = _carPosition + Vector3.up;
        doplegangerInstance.transform.rotation = _carRotation;

        //rb.velocity = transform.TransformDirection(localVelocity);
    }
}
