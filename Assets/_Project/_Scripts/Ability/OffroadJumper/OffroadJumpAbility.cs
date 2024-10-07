using _Project._Scripts.Ability;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffroadJumpAbility : Ability
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Controller controller;
    [SerializeField] float jumpForce = 10000f;
    [SerializeField] float stompForce = 20000f;

    // Update is called once per frame
    protected override void Update()
    {
        isPossibleToActivateAbility = controller.isGrounded;
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

    public override void ActivateAbility()
    {
        if (!controller.isGrounded)
        {
            return;
        }

        Jump();
    }

    private void Jump()
    {
        // Apply an upward force for the jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Stomp()
    {
        // Apply a strong downward force for the stomp
        rb.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
    }
}
