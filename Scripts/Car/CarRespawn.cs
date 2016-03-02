using UnityEngine;
using System.Collections;

public class CarRespawn : MonoBehaviour {
    bool unwantedRotation, unwantedSpeed, unwantedPosition;
    bool respawnable;
    float timeRemaining;

    public float timeThreshold = 3f;
    public float speedThreshold = 5f;
    public float rotationThreshold = 60f;
    public float positionThreshold = 15f;
    CarPhysics cp;
    CarAI cai;
    GameObject[] wayPoints;
    GameObject nearestWaypoint;
    GameObject carBody;
	// Use this for initialization
	void Start () {
        timeRemaining = timeThreshold;
        cp = GetComponent<CarPhysics>();
        cai = GetComponent<CarAI>();
        carBody = transform.FindChild("Body").gameObject;
        wayPoints = cai.waypoints;
	}
	
	// Update is called once per frame
	void Update () {
        checkSpeed();
        checkRotation();
        checkPosition();
        if(unwantedRotation || unwantedSpeed || unwantedPosition)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining < 0)
            {
                respawn();
            }
        } else
        {
            timeRemaining = timeThreshold;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            respawn();
        }

	}

    void respawn()
    {
        nearestWaypoint = findNearestWaypoint();
        cp.carRb.isKinematic = true;
        cp.transform.position = nearestWaypoint.transform.position + Vector3.up;
        cp.transform.rotation = nearestWaypoint.transform.rotation;
        cp.carRb.isKinematic = false;
        StartCoroutine(hideAndShow());
        timeRemaining = timeThreshold;
    }

    IEnumerator hideAndShow()
    {
        yield return new WaitForSeconds(0.15f);
        carBody.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        carBody.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        carBody.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        carBody.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        carBody.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        carBody.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        carBody.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        carBody.SetActive(true);

    }


    GameObject findNearestWaypoint()
    {
        Vector3 myPos = transform.position;
        GameObject g = wayPoints[0];
        float minDistance = Vector3.Distance(myPos, g.transform.position);
        float dist;
        for(int i=1; i<wayPoints.Length; i++)
        {
            dist = Vector3.Distance(myPos, wayPoints[i].transform.position);
            if (dist < minDistance)
            {
                g = wayPoints[i];
                minDistance = dist;
            }
        }
        return g;
    }

    void checkPosition()
    {
        if( Mathf.Abs(cp.transform.position.y) > positionThreshold)
        {
            unwantedPosition = true;
        } else
        {
            unwantedPosition = false;
        }
    }

    void checkRotation()
    {
        Vector3 angles = cp.transform.rotation.eulerAngles;

        if ( (angles.x > rotationThreshold && angles.x < 360-rotationThreshold ) ||
             (angles.z > rotationThreshold && angles.z < 360 - rotationThreshold) )
        {
            unwantedRotation = true;
        } else
        {
            unwantedRotation = false;
        }
    }

    void checkSpeed()
    {
        if(cp.currentVelocity < speedThreshold)
        {
            unwantedSpeed = true;
        } else
        {
            unwantedSpeed = false;
        }
    }
}
