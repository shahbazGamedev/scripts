using UnityEngine;
using System.Collections;

public class FireballRotation : MonoBehaviour {
    public bool xAxis = false,
        yAxis = true,
        zAxis = false;

    public float speed = 5f;
    Vector3 eulerAngles = Vector3.zero;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (xAxis)
            eulerAngles.x += speed;
        if (yAxis)
            eulerAngles.y += speed;
        if (zAxis)
            eulerAngles.z += speed;

        transform.rotation = Quaternion.Euler(eulerAngles);
	}
}
