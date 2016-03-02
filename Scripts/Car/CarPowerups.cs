using UnityEngine;
using System.Collections;

public class CarPowerups : MonoBehaviour
{
    public enum Powerup
    {
        Empty = -1,
        Nitro,
        Shield,
        Fireball,
        Mine
    };
    CarPhysics cp;
    CarAI cai;

    int powerupSetLength;
    Powerup[] powerupSet;

    // NITRO VARIABLES
    int nitroMultiplier = 0;
    GameObject nitroParticles;
    ParticleSystem leftNitroParticle,
        rightNitroParticle,
        nitroSparks;
    float leftNitroRate,
        rightNitroRate,
        sparkRate;
    TrailRenderer nitroTrail;

    //SHIELD VARIABLES
    GameObject shield;
    int shieldMultiplier = 0;
    float lastShieldUseTime = 0;
    TextureTiling shieldTextureTiling;

    //FIREBALL VARIABLES
    public GameObject fireballPrefab;
    GameObject fireball;
    FireballController fireballController;
    ParticleSystem fireballStartParticle;

    //MINE VARIABLES
    public GameObject minePrefab;
    GameObject mine;


    // Use this for initialization
    void Start()
    {
        initObjects();
        initSet();
        if (cp.AI)
        {
            // timedNitro();
            //Invoke("timedNitro", Random.Range(5, 30));
        }
    }

    void initObjects()
    {
        cp = GetComponent<CarPhysics>();
        cai = GetComponent<CarAI>();
        powerupSetLength = PowerupSettings.powerupSetLength;
        powerupSet = new Powerup[powerupSetLength];

        nitroParticles = transform.FindChild("Particles/nitroParticleParent").gameObject;
        shield = transform.FindChild("CarShield").gameObject;

        leftNitroParticle = nitroParticles.transform.FindChild("leftNitro").GetComponent<ParticleSystem>();
        rightNitroParticle = nitroParticles.transform.FindChild("rightNitro").GetComponent<ParticleSystem>();
        nitroSparks = nitroParticles.transform.FindChild("nitroSparks").GetComponent<ParticleSystem>();
        nitroTrail = nitroParticles.transform.FindChild("nitroTrail").GetComponent<TrailRenderer>();

        fireballStartParticle = transform.FindChild("Particles/fireballStartWave").GetComponent<ParticleSystem>();

        shieldTextureTiling = shield.GetComponent<TextureTiling>();

    }

    void timedNitro()
    {
        useNitro();
        Debug.Log("timed nitro");
        Invoke("timedNitro", Random.Range(5, 30));
    }

    void Update()
    {
        if (cp.AI)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                useNitro();
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                useShield();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                useFireball();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                useMine();
            }



            if(Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                usePowerup(0);
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                usePowerup(1);

            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                usePowerup(2);
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                CarInput.instance.brakePedal = true;
            }

        }


        if (nitroMultiplier > 0)
        {
            cp.addForwardForce(PowerupSettings.nitroForce
                * nitroMultiplier
                * Time.fixedDeltaTime);
        }

    }

    void initSet()
    {
        for (int i = 0; i < powerupSetLength; i++)
        {
            powerupSet[i] = Powerup.Empty;
        }
    }

    int findMinEmptyIndex()
    {
        for (int i = 0; i < powerupSetLength; i++)
        {
            if (powerupSet[i] == Powerup.Empty)
            {
                return i;
            }
        }
        return -1;
    }

    public bool gainPowerup(Powerup collectedPowerup)
    {
        int minEmptyIndex = findMinEmptyIndex();
        if (minEmptyIndex != -1)
        {
            powerupSet[minEmptyIndex] = collectedPowerup;
            if(!cp.AI)
            UIPowerupButtons.powerupGainAnimation(minEmptyIndex, collectedPowerup);
            return true;
        }
        return false;

    }

    public void usePowerup(int powerupIndex)
    {
        if (powerupSet[powerupIndex] != Powerup.Empty)
        {
            if (powerupSet[powerupIndex] == Powerup.Shield)
            {
                useShield();
            }
            else if (powerupSet[powerupIndex] == Powerup.Nitro)
            {
                useNitro();
            }
            else if (powerupSet[powerupIndex] == Powerup.Fireball)
            {
                useFireball();
            }
            else if (powerupSet[powerupIndex] == Powerup.Mine)
            {
                useMine();
            }
            UIPowerupButtons.clearButton(powerupIndex, powerupSet[powerupIndex]);
            powerupSet[powerupIndex] = Powerup.Empty;
        }
    }
    void useFireball()
    {
        fireballStartParticle.enableEmission = true;
        fireballStartParticle.Emit(1);

        Vector3 tempPos = fireballStartParticle.transform.localPosition;
        tempPos.z = 0;
        fireballStartParticle.transform.localPosition = tempPos;

        LeanTween.moveLocalZ(fireballStartParticle.gameObject,
            PowerupSettings.fireballSpawnDistanceFromOwner,
            PowerupSettings.fireballFireAnimationDuration);

        Invoke("spawnFireball", PowerupSettings.fireballFireAnimationDuration);
    }

    void spawnFireball()
    {
        Vector3 pos = transform.position + transform.forward *
           PowerupSettings.fireballSpawnDistanceFromOwner;
        pos.y = transform.position.y + PowerupSettings.fireballPositionY;

        fireball = Instantiate(fireballPrefab, pos, Quaternion.identity) as GameObject;

        fireballController = fireball.GetComponent<FireballController>();
        fireballController.owner = gameObject;
        fireballController.closestWayPointIndex = cai.wpIndex;
        fireballController.wayPoints = GetComponent<CarAI>().waypoints;

        cp.applyForce("Vertical",
            PowerupSettings.fireballFlarebackForcePoint,
            PowerupSettings.fireballFlarebackForce);
    }

    void useMine()
    {
        Vector3 pos = transform.position + transform.forward * -1 *
            PowerupSettings.mineSpawnDistanceFromOwner;

        pos.y = transform.position.y + PowerupSettings.mineSpawnPositionY;

        mine = Instantiate(minePrefab, pos, Quaternion.identity) as GameObject;


    }

    void useShield()
    {
        enableShield();
        CancelInvoke("disableShield");
        Invoke("disableShield", PowerupSettings.shieldDuration * shieldMultiplier);
    }

    void enableShield()
    {
        // enable shield
        shieldMultiplier++;
        if (!shield.activeSelf)
        {
            shield.SetActive(true);
            shieldTextureTiling.enable();
            cp.setForceAppPointDistance(cp.shieldedForceAppPointDist);
            Debug.Log("Shield Enabled.");
        }
    }

    void disableShield()
    {
        // disable shield
        if (shield.activeSelf)
        {
            shieldMultiplier = 0;
            //cp.setForceAppPointDistance(cp.normalForceAppPointDist);
            shieldTextureTiling.disable();
            Invoke("disableShieldObj", shieldTextureTiling.fadeTime);
            Debug.Log("Shield Disabled.");
        }
    }

    void disableShieldObj()
    {
        if (shield.activeSelf)
        {
            shield.SetActive(false);
        }
    }

    void useNitro()
    {

        enableNitro();
        Invoke("disableNitro", PowerupSettings.nitroDuration);
    }

    void enableNitro()
    {
        CancelInvoke("decreaseSpeedAfterNitro");
        cp.maxVelocity += PowerupSettings.nitroMaxVelocityBoost;
        nitroMultiplier++;
        if (!nitroParticles.activeSelf)
        {
            //CameraController.instance.enableVignetteEffect();
            nitroParticles.SetActive(true);
            leftNitroParticle.enableEmission = true;
            rightNitroParticle.enableEmission = true;
            nitroSparks.enableEmission = true;

            if (nitroMultiplier == 1)
            {
                nitroTrail.transform.parent = nitroParticles.transform;
                nitroTrail.transform.localPosition = Vector3.back * 4.5f;
                UIPowerupButtons.enableNitroShadow();
            }
        }
        Debug.Log("Nitro Enabled.");
    }

    void disableNitro()
    {
        cp.maxVelocity -= PowerupSettings.nitroMaxVelocityBoost;
        nitroMultiplier--;
        Debug.Log("Nitro Disabled.");

        if (nitroMultiplier == 0)
        {
            decreaseSpeedAfterNitro();
            if (nitroParticles.activeSelf)
            {
                CameraController.instance.disableVignetteEffect();
                leftNitroParticle.enableEmission = false;
                rightNitroParticle.enableEmission = false;
                nitroSparks.enableEmission = false;
                nitroTrail.transform.parent = null;
                UIPowerupButtons.disableNitroShadow();

                Invoke("disableNitroParticleParent", 0.25f);
            }
        }
    }

    void disableNitroParticleParent()
    {
        nitroParticles.SetActive(false);
    }

    void decreaseSpeedAfterNitro()
    {
        if (cp.currentVelocity > cp.maxVelocity)
        {
            cp.addForwardForce(PowerupSettings.nitroForce * Time.fixedDeltaTime * -10);
            Invoke("decreaseSpeedAfterNitro", Time.fixedDeltaTime);
        }
    }


}
