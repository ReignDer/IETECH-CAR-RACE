using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    public float MPH;
    private float downForce = 50f;
    private bool isBreaking;
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float maxSpeedMph;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    private WheelCollider[] wheelList = new WheelCollider[4];

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Start()
    {
        wheelList[0] = frontLeftWheelCollider;
        wheelList[1] = rearLeftWheelCollider;
        wheelList[2] = frontRightWheelCollider;
        wheelList[3] = rearRightWheelCollider;
        GetObject();
    }
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        ApplyDownForce();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void GetObject() { 
        rigidbody = GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find("CenterOfMass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void HandleMotor()
    {
        for (int i = 0; i < wheelList.Length; i++)
        {
            wheelList[i].motorTorque = verticalInput * (motorForce / 4);
        }
        //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        MPH = rigidbody.velocity.magnitude * 2.23694f;
        ApplyBreaking();

        if (MPH > maxSpeedMph)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * (maxSpeedMph / 2.23694f); 
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    private void ApplyDownForce()
    {
        rigidbody.AddForce(-transform.up * downForce * rigidbody.velocity.magnitude);
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
