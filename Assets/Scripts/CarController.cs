
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CarController : MonoBehaviour
{

    public Rigidbody car;

    public AudioClip carAccelerate;
    public AudioClip carBrake;

    private bool canPlayAccelerate = true;
    private bool canPlayBrake = true;

    public AudioSource objectSound;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        car = GetComponent<Rigidbody>();
        car.mass = 5000;
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        if (transform.position.y < 0)
        {
            GameObject MapGen = GameObject.FindGameObjectWithTag("MapGenerator");
            MapGen.GetComponent<MapGenerator>().SpawnCar();
            Destroy(gameObject);
        }
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
        if (verticalInput > 0)
        {
            if (canPlayAccelerate)
            {
                StartCoroutine(PlayAccelerate());
            }
        }
        if (isBreaking)
        {
            if (canPlayBrake)
            {
                StartCoroutine(PlayBrake());
            }
        }
    }

    IEnumerator PlayAccelerate()
    {
        canPlayAccelerate = false;
        objectSound.PlayOneShot(carAccelerate, 1);
        yield return new WaitForSeconds(1.5f);
        canPlayAccelerate = true;
    }

    IEnumerator PlayBrake()
    {
        canPlayBrake = false;
        objectSound.PlayOneShot(carBrake, 0.2f);
        yield return new WaitForSeconds(1.5f);
        canPlayBrake = true;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * 360;
        frontRightWheelCollider.motorTorque = verticalInput * 360;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
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

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            PublicVars.levelPassed += 1;
            GameObject MapGen = GameObject.FindGameObjectWithTag("MapGenerator");
            MapGen.GetComponent<MapGenerator>().GenerateNewMap();
        }
    }
}