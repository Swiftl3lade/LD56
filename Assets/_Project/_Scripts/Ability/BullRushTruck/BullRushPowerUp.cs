using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullRushPowerUp : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 5f;
    private Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = transform.forward * speedMultiplier;;
        }
    }
}
