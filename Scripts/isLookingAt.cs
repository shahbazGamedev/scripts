using UnityEngine;
using System.Collections;

public class isLookingAt : MonoBehaviour {
    public Transform target;
    public Vector3 angle;
    public float fangle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fangle = Vector3.Angle(transform.forward, target.position - transform.position);
    }
}
