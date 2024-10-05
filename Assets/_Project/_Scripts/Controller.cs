using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("References")] 
    
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask driveable;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private GameObject[] tires = new GameObject[4];
    [SerializeField] private GameObject[] frontTireParents = new GameObject[2];
    [SerializeField] private TrailRenderer[] skidMarks = new TrailRenderer[2];
    [SerializeField] private ParticleSystem[] skidSmokes = new ParticleSystem[2];

    [Header("Suspension Settings")] 
    
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float wheelRadius;

    private int[] wheelIsGrounded = new int[4];
    private bool isGrounded = false;

    [Header("Input")] 
    private float moveInput = 0;
    private float steerInput = 0;

    [Header("Car Settings")] 
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float steerStrength = 15f;
    [SerializeField] private AnimationCurve turningCurve;
    [SerializeField] private float dragCoeficient;

    private Vector3 currentCarLocalVelocity = Vector3.zero;
    private float carVelocityRatio = 0;

    [Header("Visuals")] 
    [SerializeField] private float tireRotationSpeed = 3000f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float minSideSkidVelocity = 10f;

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Movement();
        Visuals();
    }

    private void Suspension()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, driveable))
            {
                wheelIsGrounded[i] = 1;
                
                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLength - currentSpringLength) / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float dampForce = damperStiffness * springVelocity;
                    
                float springForce = springStiffness * springCompression;

                float netForce = springForce - dampForce;
                
                carRB.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);
                
                SetTirePosition(tires[i], hit.point + rayPoints[i].up * wheelRadius);
                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelIsGrounded[i] = 0;
                
                SetTirePosition(tires[i], rayPoints[i].position - rayPoints[i].up * (maxLength / 1.5f));
                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (maxLength + wheelRadius) * -rayPoints[i].up, Color.green);
            }
        }
    }

    private void GroundCheck()
    {
        int tempGroundedWheels = 0;

        for (int i = 0; i < wheelIsGrounded.Length; i++)
        {
            tempGroundedWheels += wheelIsGrounded[i];
        }

        if (tempGroundedWheels > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void GetPlayerInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void CalculateCarVelocity()
    {
        currentCarLocalVelocity = transform.InverseTransformDirection(carRB.velocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
    }

    private void Movement()
    {
        if (isGrounded)
        {
            Acceleration();
            Deceleration();
            Turn();
            SidewaysDrag();
        }
    }

    private void Acceleration()
    {
        carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Deceleration()
    {
        carRB.AddForceAtPosition(deceleration * moveInput * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    private void Turn()
    {
        carRB.AddRelativeTorque (steerStrength * steerInput * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * carRB.transform.up, ForceMode.Acceleration);
    }

    private void SidewaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;

        float dragForceMagnitude = -currentSidewaysSpeed * dragCoeficient;

        Vector3 dragForce = transform.right * dragForceMagnitude;
        
        carRB.AddForceAtPosition(dragForce, carRB.worldCenterOfMass, ForceMode.Acceleration);
    }

    private void SetTirePosition(GameObject tire, Vector3 targetPosition)
    {
        tire.transform.position = targetPosition;
    }

    private void Visuals()
    {
        TireVisuals();
        Vfx();
    }

    private void TireVisuals()
    {
        float steeringAngle = maxSteeringAngle * steerInput;
        
        for (int i = 0; i < tires.Length; i++)
        {
            if (i < 2)
            {
                tires[i].transform.Rotate(Vector3.right, tireRotationSpeed * carVelocityRatio * Time.deltaTime, Space.Self);

                frontTireParents[i].transform.localEulerAngles = new Vector3(frontTireParents[i].transform.localEulerAngles.x,
                    steeringAngle, frontTireParents[i].transform.localEulerAngles.z);
            }
            else
            {
                tires[i].transform.Rotate(Vector3.right, tireRotationSpeed * moveInput * Time.deltaTime, Space.Self);
            }
        }
    }

    private void Vfx()
    {
        if (isGrounded && Mathf.Abs(currentCarLocalVelocity.x) > minSideSkidVelocity)
        {
            ToggleSkidMarks(true);
            ToggleSkidSmokes(true);
        }
        else
        {
            ToggleSkidMarks(false);
            ToggleSkidSmokes(false);
        }
    }

    private void ToggleSkidMarks(bool toggle)
    {
        foreach (var skidMark in skidMarks)
        {
            skidMark.emitting = toggle;
        }
    }
    
    private void ToggleSkidSmokes(bool toggle)
    {
        foreach (var smoke in skidSmokes)
        {
            if (toggle)
            {
                smoke.Play();
            }
            else
            {
                smoke.Stop();
            }
        }
    }
}
