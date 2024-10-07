using _Project._Scripts.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBenderAbility : Ability
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject Ramp;
    [SerializeField] float stompForce = 30000f;
    [SerializeField] Controller controller;
    [SerializeField] Rigidbody rb;

    GameObject rampInstance;
    protected override void Update()
    {
        base.Update();

        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (!controller.isGrounded)
        {
            Stomp();
            return;
        }
    }
    private void Stomp()
    {
        // Apply a strong downward force for the stomp
        rb.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
    }
    public override void ActivateAbility()
    {
        Debug.Log("Space");
        var _spawnPossition = spawnPoint.position;
        _spawnPossition.y = -0.2f;
        Destroy(rampInstance);
        rampInstance = Instantiate(Ramp, _spawnPossition, transform.rotation);
    }
}
