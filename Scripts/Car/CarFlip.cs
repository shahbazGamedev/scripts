using UnityEngine;
using System.Collections;

public class CarFlip : MonoBehaviour {
    public Vector3 pos;
    public Vector3 force;
    public float radius;
    CarPhysics cp;
	// Use this for initialization
	void Start () {
        cp = GetComponent<CarPhysics>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            cp.carRb.AddForceAtPosition(force, cp.carRb.position + pos, ForceMode.Force);
        }
    }
}
