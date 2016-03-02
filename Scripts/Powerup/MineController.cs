using UnityEngine;
using System.Collections;

public class MineController : MonoBehaviour {
    Collider collider;
    // Use this for initialization
    void Start () {
        if (PowerupSettings.mineDuration != 0)
        {
            Invoke("destroyMine", PowerupSettings.mineDuration);
        }

        collider = GetComponent<Collider>();
        Invoke("enableCollider", 0.05f);
    }

    void enableCollider()
    {
        collider.enabled = true;
    }

    void destroyMine()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // explode
            destroyMine();
        } else if(other.tag == "Shield")
        {
            // just destroy
            destroyMine();
        }
    }


}
