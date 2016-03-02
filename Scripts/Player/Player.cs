using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public static Player instance;
    public GameObject car;
    public CarPhysics cp;
    public CarAI cai;

	// Use this for initialization
	void Awake () {
        instance = this;
        cp = car.GetComponent<CarPhysics>();
        cai = car.GetComponent<CarAI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
