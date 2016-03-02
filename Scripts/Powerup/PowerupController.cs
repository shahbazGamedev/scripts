using UnityEngine;
using System.Collections;

public class PowerupController : MonoBehaviour {
    public CarPowerups.Powerup type;
    public float disappearDuration = 0.25f;
    public float scaleMultiplier = 2;
    public float backwardSpeed = 5f;
    bool goBack = false;

    private bool isGained = false;
    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            CarPowerups cpow = col.GetComponentInParent<CarPowerups>();
            isGained = cpow.gainPowerup(type);
            if (isGained)
            {
                disappear();
                transform.parent.parent = col.transform;
                goBack = true;
                Invoke("destroy", disappearDuration);
            }
        }
    } 

    void Update()
    {
        if(goBack)
        {
            transform.localPosition += Vector3.back * backwardSpeed * Time.deltaTime;
        }
    }

    void disappear()
    {
        collider.enabled = false;
        LeanTween.scale(transform.parent.gameObject, transform.parent.localScale * scaleMultiplier, disappearDuration);
        LeanTween.alpha(gameObject, 0, disappearDuration);
    }

  

    void destroy()
    {
        Destroy(transform.parent.gameObject);
    }
    
}
