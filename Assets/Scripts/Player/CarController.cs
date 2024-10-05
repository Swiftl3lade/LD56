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

    private void FixedUpdate()
    {
        // Move();
        // Steer();
        // Brake();
    }

    protected virtual void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    private void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
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

// public class CarController : MonoBehaviour
// {
//     [Header("Input")]
//     private float horizontalInput;
//     private float verticalInput;
//     private Vector2 movementVector;
//
//     [Header("WheelColliders")] 
//     [SerializeField] private WheelCollider frontLeftWheelCollider;
//     [SerializeField] private WheelCollider frontRightWheelCollider;
//     [SerializeField] private WheelCollider rearLeftWheelCollider;
//     [SerializeField] private WheelCollider rearRightWheelCollider;
//
//     [Header("WheelTransforms")] 
//     [SerializeField] private Transform frontLeftWheelTransform;
//     [SerializeField] private Transform frontRightWheelTransform;
//     [SerializeField] private Transform rearLeftWheelTransform;
//     [SerializeField] private Transform rearRightWheelTransform;
//
//     [Header("Settings")] 
//     [SerializeField] private float motorForce;
//     [SerializeField] private float maxSteerAngle;
//     [SerializeField] private float breakForce;
//     private float currentSteerAngle;
//     private float currentBreakForce;
//     private bool isBreaking;
//
//     
//     private void FixedUpdate()
//     {
//         GetInput();
//         HandleMotor();
//         HandleSteering();
//         UpdateWheelsVisuals();
//     }
//
//     private void GetInput()
//     {
//         horizontalInput = Input.GetAxis("Horizontal");
//         verticalInput = Input.GetAxis("Vertical");
//         isBreaking = Input.GetKey(KeyCode.Space);
//     }
//
//     private void HandleMotor()
//     {
//         if (isBreaking)
//         {
//             ApplyBreaking();
//         }
//         else
//         {
//             ResetBrakes();
//         
//             frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
//             frontRightWheelCollider.motorTorque = verticalInput * motorForce;
//             rearLeftWheelCollider.motorTorque = verticalInput * motorForce/2;
//             rearRightWheelCollider.motorTorque = verticalInput * motorForce/2;
//         }
//         
//     }
//     
//     private void ApplyBreaking()
//     {
//         frontLeftWheelCollider.brakeTorque = breakForce;
//         frontRightWheelCollider.brakeTorque = breakForce;
//         rearLeftWheelCollider.brakeTorque = breakForce;
//         rearRightWheelCollider.brakeTorque = breakForce;
//     }
//     
//     private void ResetBrakes()
//     {
//         frontLeftWheelCollider.brakeTorque = 0f;
//         frontRightWheelCollider.brakeTorque = 0f;
//         rearLeftWheelCollider.brakeTorque = 0f;
//         rearRightWheelCollider.brakeTorque = 0f;
//     }
//
//     private void HandleSteering()
//     {
//         currentSteerAngle = maxSteerAngle * horizontalInput;
//         
//         frontLeftWheelCollider.steerAngle = currentSteerAngle;
//         frontRightWheelCollider.steerAngle = currentSteerAngle;
//     }
//     
//     private void UpdateWheelsVisuals()
//     {
//         UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
//         UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
//         UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
//         UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
//     }
//
//     private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
//     {
//         Vector3 position;
//         Quaternion rotation;
//         
//         wheelCollider.GetWorldPose(out position, out rotation);
//         wheelTransform.rotation = rotation;
//         wheelTransform.position = position;
//     }
// }
