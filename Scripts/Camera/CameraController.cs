using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Utility;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    Camera cam;
    GameObject car;
    Transform aimPosTransform;
    Transform aimLookAtTransform;
    public float positionSmooth = 5f;
    Vector3 aimPos, currPos;
    Vector3 aimLookAt, currLookAt;

    Transform followCarObj;
    Vector3 followRot;
    Vector3 followPos;

    CarPhysics cp;


    public float minDist = 4,
        normalMaxDist = 8;
    public float flipMaxDist = 10f;
    float maxDist;
    public bool flipping = false;
    public float height = 3;

    void Start()
    {
        instance = this;
        cam = GetComponent<Camera>();
        car = Player.instance.car;

        followCarObj = GameObject.Find("FollowCarObj").transform;
        //aimPosTransform = car.transform.FindChild("mainCameraPosition");
        aimLookAtTransform = car.transform.FindChild("mainCameraLookAtPosition");
        cp = car.GetComponent<CarPhysics>();

        currPos = aimPos = followCarObj.position;
        currLookAt = aimLookAt = car.transform.position;

        maxDist = normalMaxDist;

    }

    Vector3 degradeVectorToOne(Vector3 v)
    {
        float max = v.x;

        if (v.z > max)
        {
            max = v.z;
        }
        v.y = 0;
        return v / max;
    }


    float aimRot = 0, currRot = 0;

    float rotationSmooth = 5;
    public float normalRotationSmooth = 5f;
    public float flipRotationSmooth = 0.5f;
    void FixedUpdate()
    {
        followRot = car.transform.rotation.eulerAngles;
        followRot.x = 0;
        followRot.z = 0;

        aimRot = followRot.y;
        currRot = Mathf.LerpAngle(currRot, aimRot, Time.fixedDeltaTime * rotationSmooth);
        followRot.y = currRot;

        followCarObj.position = car.transform.position;
        followCarObj.localRotation = Quaternion.Euler(followRot);

        aimPos = followCarObj.position +
            -followCarObj.forward *
            (minDist +
            (cp.currentVelocity / cp.maxVelocity) * (maxDist - minDist));

        aimPos.y = followCarObj.position.y + height;

        currPos = Vector3.Lerp(currPos, aimPos, positionSmooth * Time.fixedDeltaTime);

        transform.position = currPos;
        transform.LookAt(followCarObj);
    }

    public void enableFlipping()
    {
        rotationSmooth = flipRotationSmooth;
        //maxDist = flipMaxDist;
        StartCoroutine(increaseMaxDist());
    }

    IEnumerator increaseMaxDist()
    {
        float time = 0.75f;
        float amount = (flipMaxDist-normalMaxDist) / time * Time.deltaTime;
        while(maxDist < flipMaxDist)
        {
            maxDist += amount;
            yield return new WaitForEndOfFrame();
        }

        maxDist = flipMaxDist;
    }

    IEnumerator decreaseMaxDist()
    {
        float time = 0.75f;
        float amount = (flipMaxDist - normalMaxDist) / time * Time.deltaTime;
        while (maxDist > normalMaxDist)
        {
            maxDist -= amount;
            yield return new WaitForEndOfFrame();
        }

        maxDist = normalMaxDist;
    }

    public void disableFlipping()
    {
        rotationSmooth = normalRotationSmooth;
        //maxDist = normalMaxDist;
        StartCoroutine(decreaseMaxDist());
    }

    public void enableVignetteEffect()
    {

    }

    IEnumerator increaseVignetteEffect()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator decreaseVignetteEffect()
    {
        yield return new WaitForEndOfFrame();

    }

    public void disableVignetteEffect()
    {
        StartCoroutine(decreaseVignetteEffect());
    }






}
