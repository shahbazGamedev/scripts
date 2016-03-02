using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {

    // Use this for initialization
    public float steeringSensivity = 0.05f;
    public GameObject [] waypoints;
    private float carAngle, carAngle2;
    private Vector3 turnDirection;



    private Vector3 inComingNodeDist;
    private float steeringValue=0;
    public int wpIndex = 0;
    private bool triggerCont = true;
    private float angleValue;

    Vector3 carTempPosition;

    public float steering;
    
	void Start () {
	    
	}
	
	// Update is called once per frame
    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "WayPoint")
        {
            triggerCont = true;
            //wpIndex++;
            wpIndex = int.Parse(col.name.Substring(8)) + 1;
            Debug.Log("Waypoint Index: " + wpIndex);
            if (wpIndex == waypoints.Length)
            {
                wpIndex = 0;
            }


        }
        else
            triggerCont = false;



    }

	void Update () {
        CarInput.instance.gasPedal = true;

        if (triggerCont)
        {
            turnDirection = Vector3.Cross(transform.forward, waypoints[wpIndex].transform.position - transform.position);
            angleValue = Vector3.Angle(transform.forward, waypoints[wpIndex].transform.position - transform.position);
        }
   
       // Debug.Log("Turn Direction: " + turnDirection);
       // Debug.Log("Angle Value: " + angleValue + " Waypoint Number: " + wpIndex);

        
        //angleValue = Mathf.Clamp(angleValue, -30, 30);
        //turnDirection.y = Mathf.Clamp(turnDirection.y, -0.5f, 0.5f);
        
        if (turnDirection.y < 0)
        {
            steeringValue = -1 *  angleValue * steeringSensivity;
            steering = steeringValue;

            

        }
        else
        {
            steeringValue =  1 *  angleValue * steeringSensivity;
            steering = steeringValue;



        }
        steering = Mathf.Clamp(steering, -1, 1);

        
	}
}
