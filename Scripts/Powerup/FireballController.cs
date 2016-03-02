using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour
{

    public GameObject[] wayPoints { get; set; }
    public int closestWayPointIndex;
    public bool isFollowingPlayer;

    public GameObject owner;
    Vector3 aimLookAt;
    public Transform target;
    Collider collider;
    GameObject outerLightning;
    GameObject innerBall;
    GameObject playerDetector;
    ParticleSystem waves;
    ParticleSystem explosion;
    bool targetFound = false;
    ParticleSystem.EmissionModule em;
    void Start()
    {
        if (PowerupSettings.fireballDuration != 0)
        {
            Invoke("destroyFireball", PowerupSettings.fireballDuration);
        }

        collider = GetComponent<Collider>();
        outerLightning = transform.FindChild("outerLightning").gameObject;
        innerBall = transform.FindChild("innerBall").gameObject;
        waves = transform.FindChild("fireballWaves").GetComponent<ParticleSystem>();
        explosion = transform.FindChild("fireballExplosion").GetComponent<ParticleSystem>();
        em = waves.emission;
        Invoke("enableCollider", 0.05f);
    }

    void enableCollider()
    {
        collider.enabled = true;
    }


    void FixedUpdate()
    {
            if (isFollowingPlayer)
            {
                aimLookAt = target.position;
                aimLookAt.y += PowerupSettings.fireballPositionY - 0.15f;
            }
            else
            {
                aimLookAt = wayPoints[closestWayPointIndex].transform.position;
                aimLookAt.y += PowerupSettings.fireballPositionY;
            }

            transform.LookAt(aimLookAt);

            Vector3 increment = transform.forward * PowerupSettings.fireballSpeed * Time.fixedDeltaTime;

            transform.position += increment;
    }

    void hideFireball()
    {
        outerLightning.SetActive(false);
        innerBall.SetActive(false);

        
        em.enabled = false;
        collider.enabled = false;
        Invoke("destroyFireball", 1f);
    }

    void destroyFireball()
    {
        
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.gameObject != owner)
        {
            if (col.tag == "WayPoint")
            {
                closestWayPointIndex++;
                if (closestWayPointIndex == wayPoints.Length)
                {
                    closestWayPointIndex = 0;
                }
            }
            else if (col.tag == "Player")
            {
                //explode
                explosion.Emit(1);
                CarPhysics cp = col.transform.parent.GetComponent<CarPhysics>();
                cp.StartCoroutine(cp.backFlip());
                hideFireball();
            }
            else if (col.tag == "Shield")
            {
                // just destroy
                hideFireball();
            }
        }
    }

}
