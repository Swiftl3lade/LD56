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

    protected float moveInput;
    protected float steerInput;
    
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

    protected virtual void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        // Get the current speed of the car
        float currentSpeed = carRigidbody.velocity.magnitude;

        foreach (var wheel in wheels)
        {
            // Only apply motor torque to wheels on the rear axle
            if (wheel.axel == Axel.Rear)
            {
                // If the current speed is less than maxSpeed, allow acceleration
                if (currentSpeed < maxSpeed)
                {
                    // Apply motor torque proportional to the current speed for smooth acceleration
                    float torque = moveInput * maxAcceleration * (1 - (currentSpeed / maxSpeed));
                    wheel.wheelCollider.motorTorque = torque;
                }
                else
                {
                    // If speed exceeds maxSpeed, stop applying motor torque
                    wheel.wheelCollider.motorTorque = 0;
                }
            }
        }
    }

    private void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                // wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                wheel.wheelCollider.steerAngle = steerAngle;
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
                if (wheel.axel == Axel.Rear)
                {
                    // Apply brake torque only to the rear wheels
                    wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
}
