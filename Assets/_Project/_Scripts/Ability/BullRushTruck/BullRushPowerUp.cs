using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Ability;
using UnityEngine;

public class BullRushPowerUp : Ability
{
    [SerializeField] private float speedMultiplier = 5f;
    private Rigidbody rb;
    

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public override void ActivateAbility()
    {
        rb.velocity = transform.forward * speedMultiplier;
    }
           
}
