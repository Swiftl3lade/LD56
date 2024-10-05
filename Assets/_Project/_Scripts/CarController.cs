using System;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    [SerializeField] private float maxAcceleration = 30f;
    [SerializeField] private float brakeAcceleration = 50f;
    [SerializeField] private float maxSpeed = 25f;
    
    [SerializeField] private float turnSensitivity = 1f;
    [SerializeField] private float maxSteerAngle = 30f;

    [SerializeField] private Vector3 centerOfMass;
    

    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    
    private Rigidbody carRigidbody;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        GetInputs();
        AnimateWheels();
        Move();
        Steer();
        Brake();
        
    }

    private void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        float currentSpeed = carRigidbody.velocity.magnitude;

        if (currentSpeed < maxSpeed)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }

    private void Steer()
    {
        float speedFactor = carRigidbody.velocity.magnitude / 100f; 
        float adjustedMaxSteerAngle = Mathf.Lerp(maxSteerAngle, maxSteerAngle / 2f, speedFactor);

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * adjustedMaxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out rotation);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = rotation;
        }
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
                
                if (wheel.axel == Axel.Rear)
                {
                    WheelFrictionCurve sidewaysFriction = wheel.wheelCollider.sidewaysFriction;
                    sidewaysFriction.extremumValue = 0.1f; // Lower grip during drift
                    sidewaysFriction.asymptoteValue = 0.1f; // Allow more sliding
                    wheel.wheelCollider.sidewaysFriction = sidewaysFriction;

                    WheelFrictionCurve forwardFriction = wheel.wheelCollider.forwardFriction;
                    forwardFriction.extremumValue = 0.1f;
                    forwardFriction.asymptoteValue = 0.1f;
                    wheel.wheelCollider.forwardFriction = forwardFriction;
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;

                WheelFrictionCurve sidewaysFriction = wheel.wheelCollider.sidewaysFriction;
                sidewaysFriction.extremumValue = 1f; 
                sidewaysFriction.asymptoteValue = 0.5f;
                wheel.wheelCollider.sidewaysFriction = sidewaysFriction;

                WheelFrictionCurve forwardFriction = wheel.wheelCollider.forwardFriction;
                forwardFriction.extremumValue = 1f;
                forwardFriction.asymptoteValue = 0.5f;
                wheel.wheelCollider.forwardFriction = forwardFriction;
            }
        }
    }
}


