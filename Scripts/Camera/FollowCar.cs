using UnityEngine;
using System.Collections;

public class FollowCar : MonoBehaviour {
    Transform car;
    Vector3 rot;
	// Use this for initialization
	void Start () {
        car = Player.instance.car.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rot = car.rotation.eulerAngles;
        Debug.Log(rot.x);
        rot.x = 0;
        rot.z = 0;
        rot.y += 720;
        //transform.rotation.eulerAngles = rot;
        transform.rotation = Quaternion.Euler(rot);
        transform.position = car.position;
	}
}
