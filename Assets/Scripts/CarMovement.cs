using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMovement : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] WheelCollider FRWheel;
    [SerializeField] WheelCollider FLWheel;
    [SerializeField] WheelCollider RRWheel;
    [SerializeField] WheelCollider RLWheel;

    [Header("Wheel Meshes")]
    [SerializeField] MeshRenderer FRWheelMesh;
    [SerializeField] MeshRenderer FLWheelMesh;
    [SerializeField] MeshRenderer RRWheelMesh;
    [SerializeField] MeshRenderer RLWheelMesh;

    [Header("lights")]

    [SerializeField] GameObject frontR;
    [SerializeField] GameObject frontL;
    [SerializeField] GameObject backR;
    [SerializeField] GameObject backL;
    [SerializeField] GameObject backMainR;
    [SerializeField] GameObject backMainL;

    [Header("Car settings")]

    [SerializeField] Vector3 centerOfMass;
    [SerializeField] float maxSpeed = 20;
    [SerializeField] float carPower = 20;
    [SerializeField] float breakPower = 40;
    [SerializeField] float skipAllowance = 0.1f;
    [SerializeField] AnimationCurve steerCurve;
    [SerializeField] GameObject smoke;
    [SerializeField] GameObject tireTrail;
    [SerializeField] bool isFWD = true;
    [SerializeField] bool isRWD = true;
    [SerializeField] float maxSteerAngle;
    [SerializeField] float decelerationMultiplier = 1.0f;

    // drift friction setting

    [Header("drift friction setting")]
    [SerializeField] float driftStiffness = 10.0f;
    [SerializeField] float driftExtremumSlip = 15.0f;
    [SerializeField] float driftExtremumValue = 2;
    //[Header("normal friction setting")]
    //[SerializeField] float normalStiffness = 10.0f;
    //[SerializeField] float normalExtremumSlip = 5f;
    //[SerializeField] float normalExtremumValue = 1;

    // tire skereching smoke 
    private ParticleSystem FRsmoke;
    private ParticleSystem FLsmoke;
    private ParticleSystem RRsmoke;
    private ParticleSystem RLsmoke;

    // tire trail 

    private TrailRenderer FRTrail;
    private TrailRenderer FLTrail;
    private TrailRenderer RRTrail;
    private TrailRenderer RLTrail;



    public int isEngineRunning;
    private Rigidbody carRB;
    private float gasInput;
    private float steerInput;
    private float breakInput;
    private float speed;
    private float speedClamped;
    private float slipAngle;
    float steerAngle;
    bool isOff = false;
    bool isBreakLight = false;
    WheelFrictionCurve driftSideWheelfriction;
    WheelFrictionCurve normalSideWheelfriction;

    private void Awake()
    {
        carRB = GetComponent<Rigidbody>();
        driftSideWheelfriction = RRWheel.sidewaysFriction;
        normalSideWheelfriction = RRWheel.sidewaysFriction;

    }
    private void Start()
    {
        InstantiateParticles();
        // sidewaydrift asign
        driftSideWheelfriction.stiffness = driftStiffness;
        driftSideWheelfriction.extremumSlip = driftExtremumSlip;
        driftSideWheelfriction.extremumValue = driftExtremumValue;


    }


    private void Update()
    {

        speed = RRWheel.rpm * RRWheel.radius * 2f * Mathf.PI / 10f; ;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);

        //ApplyMotor();
        //ApplyWheelsChanges();
        //ApplySteering();
        //ApplyBreaking();
        //CheckParticle();
        //BreakLightController();
        if (Input.GetKeyDown(KeyCode.F))
        {
            LightController();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyDrifting();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            RRWheel.sidewaysFriction = normalSideWheelfriction;
            RLWheel.sidewaysFriction = normalSideWheelfriction;
        }
    }



    private void FixedUpdate()
    {

        ApplyMotor();
        ApplyWheelsChanges();
        ApplySteering();
        ApplyBreaking();
        CheckParticle();

    }
    public float GetSpeedRatio()
    {
        float gas = Math.Clamp(gasInput, 0.5f, 1f);
        return speedClamped * gas / maxSpeed;
    }
    private void BreakLightController()
    {
        backMainL.SetActive(isBreakLight);
        backMainR.SetActive(isBreakLight);
    }
    private void LightController()
    {
        isOff = !isOff;
        frontL.SetActive(isOff);
        frontR.SetActive(isOff);
        backL.SetActive(isOff);
        backR.SetActive(isOff);
    }
    public void UpdateInput(Vector3 vector, float brake)
    {

        gasInput = vector.y;
        steerInput = vector.x;

        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);
        float movingDirection = Vector3.Dot(transform.forward, carRB.velocity);
        if (math.abs(gasInput) > 0 && isEngineRunning == 0)
        {
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
        }
        if (movingDirection < -0.5f && gasInput > 0)
        {
            breakInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            breakInput = Mathf.Abs(gasInput);

        }
        else if (gasInput == 0)
        {
            carRB.velocity = carRB.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
        }
        else
        {

            breakInput = 0;
        }

    }


    private void ApplyDrifting()
    {
        RRWheel.sidewaysFriction = driftSideWheelfriction;
        RLWheel.sidewaysFriction = driftSideWheelfriction;

        RRWheel.brakeTorque = breakPower;
        RLWheel.brakeTorque = breakPower;
    }
    private void ApplyBreaking()
    {
        FRWheel.brakeTorque = breakInput * breakPower * 0.3f;
        FLWheel.brakeTorque = breakInput * breakPower * 0.3f;

        RRWheel.brakeTorque = breakInput * breakPower * 0.7f;
        RLWheel.brakeTorque = breakInput * breakPower * 0.7f;


        if (breakInput > 0)
        {
            isBreakLight = true;
        }
        else
        {
            isBreakLight = false;
        }

        BreakLightController();
    }


    void ApplyWheelsChanges()
    {
        UpdateWheel(RLWheel, RLWheelMesh);
        UpdateWheel(RRWheel, RRWheelMesh);
        UpdateWheel(FLWheel, FLWheelMesh);
        UpdateWheel(FRWheel, FRWheelMesh);
    }

    void ApplySteering()
    {
        steerAngle = steerCurve.Evaluate(speed);
        if (slipAngle < 120)
        {
            steerAngle += Vector3.SignedAngle(transform.forward, carRB.velocity + transform.forward, Vector3.up);
        }
        steerAngle = Math.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);
        FRWheel.steerAngle = steerInput * steerAngle;
        FLWheel.steerAngle = steerInput * steerAngle;
    }
    void ApplyMotor()
    {
        if (isEngineRunning > 1)
        {

            if (speed < maxSpeed)
            {
                if (isRWD)
                {
                    RLWheel.motorTorque = carPower * gasInput;
                    RRWheel.motorTorque = carPower * gasInput;
                }
                if (isFWD)
                {
                    FRWheel.motorTorque = carPower * gasInput;
                    FLWheel.motorTorque = carPower * gasInput;
                }
            }
            else
            {
                RLWheel.motorTorque = 0;
                RRWheel.motorTorque = 0;
                FRWheel.motorTorque = 0;
                FLWheel.motorTorque = 0;
            }
        }
    }
    void UpdateWheel(WheelCollider wheelCollider, MeshRenderer mesh)
    {
        Quaternion quaternion;
        Vector3 position;

        wheelCollider.GetWorldPose(out position, out quaternion);
        mesh.transform.position = position;
        mesh.transform.rotation = quaternion;
    }
    void InstantiateParticles()
    {
        if (smoke)
        {
            FRsmoke = Instantiate(smoke, FRWheel.transform.position - Vector3.up * FRWheel.radius, Quaternion.identity, FRWheel.transform).GetComponent<ParticleSystem>();
            FLsmoke = Instantiate(smoke, FLWheel.transform.position - Vector3.up * FLWheel.radius, Quaternion.identity, FLWheel.transform).GetComponent<ParticleSystem>();
            RRsmoke = Instantiate(smoke, RRWheel.transform.position - Vector3.up * RRWheel.radius, Quaternion.identity, RRWheel.transform).GetComponent<ParticleSystem>();
            RLsmoke = Instantiate(smoke, RLWheel.transform.position - Vector3.up * RLWheel.radius, Quaternion.identity, RLWheel.transform).GetComponent<ParticleSystem>();
        }
        if (tireTrail)
        {
            FRTrail = Instantiate(tireTrail, FRWheel.transform.position - Vector3.up * FRWheel.radius, Quaternion.identity, FRWheel.transform).GetComponent<TrailRenderer>();
            FLTrail = Instantiate(tireTrail, FLWheel.transform.position - Vector3.up * FLWheel.radius, Quaternion.identity, FLWheel.transform).GetComponent<TrailRenderer>();
            RRTrail = Instantiate(tireTrail, RRWheel.transform.position - Vector3.up * RRWheel.radius, Quaternion.identity, RRWheel.transform).GetComponent<TrailRenderer>();
            RLTrail = Instantiate(tireTrail, RLWheel.transform.position - Vector3.up * RLWheel.radius, Quaternion.identity, RLWheel.transform).GetComponent<TrailRenderer>();
        }
    }
    void CheckParticle()
    {
        WheelHit[] wheelHits = new WheelHit[4];

        FRWheel.GetGroundHit(out wheelHits[0]);
        FLWheel.GetGroundHit(out wheelHits[1]);
        RRWheel.GetGroundHit(out wheelHits[2]);
        RLWheel.GetGroundHit(out wheelHits[3]);

        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > skipAllowance))
        {
            RRsmoke.Play();
            RRTrail.emitting = true;
        }
        else
        {
            RRsmoke.Stop();
            RRTrail.emitting = false;
        }
        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > skipAllowance))
        {
            RLsmoke.Play();
            RLTrail.emitting = true;
        }
        else
        {
            RLsmoke.Stop();
            RLTrail.emitting = false;
        }
        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > skipAllowance))
        {
            FRTrail.emitting = true;
        }
        else
        {
            FRTrail.emitting = false;

        }
        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > skipAllowance))
        {
            FLTrail.emitting = true;
        }
        else
        {
            FLTrail.emitting = false;

        }

    }
    
}
