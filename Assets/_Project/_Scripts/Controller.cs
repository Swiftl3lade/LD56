using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private float deceleration = 10f;// New braking force
    [SerializeField] private float defaultDrag = 0f;// New braking force
    [SerializeField] private float brakeForce = 4f;// New braking force
    [SerializeField] private float steerStrength = 15f;
    [SerializeField] private AnimationCurve turningCurve;
    [SerializeField] private float dragCoefficient;
    
    [Header("Drifting Settings")]
    [SerializeField] private float driftFactor = 0.95f; // The factor that reduces the sideways speed during a drift
    [SerializeField] private float maxDriftAngle = 45f; // The maximum angle of drift

    [Header("Visuals")] 
    [SerializeField] private float tireRotationSpeed = 3000f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float minSideSkidVelocity = 10f;
    
    private Vector3 currentCarLocalVelocity = Vector3.zero;
    private float carVelocityRatio = 0;
    private float previousMoveInput = 0f; // Store previous move input
    
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
                float springCompression = Mathf.Clamp((restLength - currentSpringLength) / springTravel, 0f, 1f);

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
        var tempGroundedWheels = wheelIsGrounded.Sum();

        isGrounded = tempGroundedWheels > 1;
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
        }

        Turn();

        SidewaysDrag();
    }

    private void Acceleration()
    {
        float currentSpeed = carRB.velocity.magnitude;
        
        Vector3 localVelocity = transform.InverseTransformDirection(carRB.velocity);

        carRB.drag = (moveInput > 0 && localVelocity.z < 0) || (moveInput < 0 && localVelocity.z > 0)
            ? brakeForce
            : defaultDrag;

        if (currentSpeed < maxSpeed)
        {
            carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }
    }

    private void Deceleration()
    {
        // If moving backward, apply more acceleration until reaching speed 0
        if (carRB.velocity.z > 0)
        {
            // If moving forward, apply more braking force until speed is 0
            carRB.AddForceAtPosition(transform.forward * (deceleration * Mathf.Abs(moveInput) * 2), accelerationPoint.position, ForceMode.Acceleration);
        }
        else
        {
            // Apply reverse acceleration
            carRB.AddForceAtPosition(deceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }
    }

    private void Turn()
    {
        float steerInputAdjusted = Mathf.Clamp(steerInput, -1f, 1f);
        float currentSidewaysSpeed = currentCarLocalVelocity.x;

        // Adjusting torque based on drift
        float driftTorqueAdjustment = Mathf.Abs(currentSidewaysSpeed) > 1 ? driftFactor : 1;

        carRB.AddRelativeTorque(steerStrength * steerInputAdjusted * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * carRB.transform.up * driftTorqueAdjustment, ForceMode.Acceleration);
    }

    private void SidewaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;
        float dragForceMagnitude = -currentSidewaysSpeed * dragCoefficient;

        // Reduce drag when drifting
        if (Mathf.Abs(currentSidewaysSpeed) > 1) 
        {
            dragForceMagnitude *= 0.5f; // Example value, adjust for your needs
        }

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
                tires[i].transform.Rotate(Vector3.right, tireRotationSpeed * Mathf.Abs(carRB.velocity.z) * Time.deltaTime, Space.Self);
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
            // skidMark.emitting = toggle;
        }
    }
    
    private void ToggleSkidSmokes(bool toggle)
    {
        // foreach (var smoke in skidSmokes)
        // {
        //     if (toggle)
        //     {
        //         smoke.Play();
        //     }
        //     else
        //     {
        //         smoke.Stop();
        //     }
        // }
    }
}
