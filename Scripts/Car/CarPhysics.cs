using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarPhysics : MonoBehaviour
{
    public bool AI = false;
    CarAI carAI;
    public Transform[] wheelTransforms;
    public WheelCollider[] wheelColliders;
    public Transform carBody;
    public float carBodyRotateSensivity = 5f;

    public float motorTorque;
    public float brakeTorque;
    float forceMotor = -1, forceBrake = -1;
    public float maxMotorTorque = 2000f;
    public float minBrakeTorque = 500f;
    public float maxBrakeTorque = 10000f;
    public float minSteerAngle = 3f;
    public float maxSteerAngle = 15f;
    public float currentMaxSteerAngle;
    public float steeringSensivity = 200f;
    public float maxVelocity = 50f;

    public float currentVelocity;
    public Rigidbody carRb;
    public float steerAngle;

    bool gasPedal, brakePedal;
    float steering;

    bool applyForceAtLateUpdate;
    float forceAmount;

    public float normalForceAppPointDist,
        shieldedForceAppPointDist;

    bool isLocalPlayer = false;

    void Start()
    {
        carRb = wheelColliders[0].attachedRigidbody;
        carAI = GetComponent<CarAI>();
        isLocalPlayer = (gameObject == Player.instance.car);
    }

    void Update()
    {
        getInputs();
        applyMotorTorquesToWheels();
        steerFrontWheels();
        rotateWheelTransforms();
    }

    void getInputs()
    {
        if (!AI) // if not AI, get inputs from user
        {
            gasPedal = CarInput.instance.gasPedal;
            brakePedal = CarInput.instance.brakePedal;
            steering = CarInput.instance.steering;
        }
        else // if AI, get inputs from AI Script
        {
            gasPedal = true;
            brakePedal = false;
            steering = carAI.steering;
        }

        if (gasPedal)
        {
            motorTorque = maxMotorTorque;
        }
        else
        {
            motorTorque = 0;
            brakeTorque = maxBrakeTorque;
        }

        if (brakePedal)
        {
            brakeTorque = maxBrakeTorque;
        }
        else
        {
            brakeTorque = 0;
        }


        if(forceMotor != -1)
        {
            motorTorque = forceMotor;
        }

        if(forceBrake != -1)
        {
            brakeTorque = forceBrake;
        }

        currentMaxSteerAngle = Mathf.Clamp(
            steeringSensivity / currentVelocity, 
            minSteerAngle, maxSteerAngle);
        steerAngle = currentMaxSteerAngle * steering;


        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(backFlip());
        }
    }

    void applyMotorTorquesToWheels()
    {
        currentVelocity = carRb.velocity.magnitude;

        if (currentVelocity > maxVelocity)
        {
            motorTorque = 0;
            if (currentVelocity > maxVelocity + 1)
            {
                brakeTorque = maxBrakeTorque;
            } else
            {
                brakeTorque = 0;
            }
        } 

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = motorTorque;
            wheelColliders[i].brakeTorque = brakeTorque;
        }

    }

    void rotateWheelTransforms()
    {
        float rpm = Mathf.Abs(wheelColliders[0].rpm);
        for (int i = 0; i < 4; i++)
        {
            wheelTransforms[i].transform.Rotate(rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }
    }

    void steerFrontWheels()
    {
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = steerAngle;
            wheelTransforms[i].transform.localRotation = Quaternion.Euler(
                wheelTransforms[i].transform.localRotation.eulerAngles.x,
                steerAngle*3,
                wheelTransforms[i].transform.localRotation.eulerAngles.z);
        }
    }


    void LateUpdate()
    {
        if (applyForceAtLateUpdate)
        {
            if (currentVelocity < maxVelocity && currentVelocity > 0)
            {
                carRb.AddForce(transform.forward * forceAmount);
            }
            applyForceAtLateUpdate = false;
        }
        else
        {
            forceAmount = 0;
        }
    }


    public void addForwardForce(float force)
    {
        applyForceAtLateUpdate = true;
        forceAmount = force;
    }

    public void setForceAppPointDistance(float value)
    {
        foreach(WheelCollider wc in wheelColliders)
        {
            wc.forceAppPointDistance = value;
        }
    }




    public void applyForce(string axis, float additionPosToCenter, float forceAmount)
    {
        Vector3 forcePosition;
        Vector3 force;
        if(axis == "Horizontal")
        {
            forcePosition = transform.position + transform.right * additionPosToCenter;
            force = transform.right * forceAmount;
        }
        else
        {
            forcePosition = transform.position + transform.forward * additionPosToCenter;
            force = transform.up * forceAmount;
        }

        carRb.AddForceAtPosition(force, forcePosition, ForceMode.Force);


    }

    public IEnumerator backFlip()
    {
        if (isLocalPlayer)
        {
            CameraController.instance.enableFlipping();
        }
        for (int i=5; i<14; i++)
        {
            applyForce("Vertical",
               -PowerupSettings.fireballFlarebackForcePoint,
               PowerupSettings.fireballExplosionForce/(i/5));
            carRb.velocity *= 0.96f;
            yield return new WaitForSeconds(0.1f);
        }
        if (isLocalPlayer)
        {
            yield return new WaitForSeconds(0.2f);
            CameraController.instance.disableFlipping();
        }
      

    }


}







