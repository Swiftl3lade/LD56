using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffroadJumpAbility : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Controller controller;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float stompForce = 10f;

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        Debug.Log("Space");

        if (!controller.isGrounded)
        {
            Stomp();
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
