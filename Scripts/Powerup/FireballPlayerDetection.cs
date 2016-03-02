using UnityEngine;
using System.Collections;

public class FireballPlayerDetection : MonoBehaviour {
    FireballController f;
	// Use this for initialization
	void Start () {
        f = transform.parent.GetComponent<FireballController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.transform.parent.gameObject != f.owner)
            {
                f.isFollowingPlayer = true;
                f.target = other.transform.parent;
                Destroy(gameObject);
            }
        }
    }
}
