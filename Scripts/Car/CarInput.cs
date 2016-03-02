using UnityEngine;
using System.Collections;

public class CarInput : MonoBehaviour
{
    public static CarInput instance; // Singleton instance

    // Enumerators
    public enum ControlTypes
    {
        Editor,
        Mobile
    };
    public enum MobilControlTypes
    {
        Accelerometer,
        Screen,
        Buttons
    };

    public ControlTypes controlType;
    public MobilControlTypes mobilControlType;
    Vector3 accelerometerTemp;

    public bool autoAcceleration;

    //public float accelerometerX;
    public bool gasPedal; // true, false (always true when autoAcceleration is true)
    public bool brakePedal; // true, false
    public float steering; // between -1..1
    public float accelerometerMax = 0.5f; // 0..1 : when to reach max
    public float accelerometerMultiplier = 1.5f; // multiply accelerometer with this
    float aimSteering;
    public float steeringSensitivity = 5f;
    bool screenTouching = false; // if player is touching the screen or not (for Mobile/Screen mode)
    float screenWidth;
    float screenMidPoint;
    Touch touch; // to store screen touch
    void Start()
    {
        instance = this; // instance refers to this class

        //inits
        steering = aimSteering = 0;
        gasPedal = false;
        brakePedal = false;
        accelerometerTemp = Vector3.zero;
        screenWidth = Screen.width;
        screenMidPoint = screenWidth / 2;
    }

    void Update()
    {

        if(Input.GetButtonDown("Cancel"))
        {
            Application.LoadLevel(0);
        }
        
        brakePedal = false; // if no inputs for brake are received, this will stay false
        if (autoAcceleration)
        {
            gasPedal = true; // if no inputs for gas are received, this will stay true
        }
        else
        {
            gasPedal = false; // if no inputs for gas are received, this will stay false
        }

        if (controlType == ControlTypes.Mobile)
        {
            if (mobilControlType == MobilControlTypes.Accelerometer) // Acceloremeter
            {
                
                gasPedal = true; // gasPedal is always true in acceloremeter mode

                accelerometerTemp = Input.acceleration;
                steering = accelerometerTemp.x / accelerometerMax; // assign steering value directly from acceloremeter
                steering *= accelerometerMultiplier;
                if (Input.touchCount > 0) // if any touch is made to anywhere on screen
                {
                    brakePedal = true;
                }

            }
            else if (mobilControlType == MobilControlTypes.Screen) // Screen 
            {
                gasPedal = true; // gasPedal is always true in screen mode
                if (Input.touchCount > 0) // if user touches the screen
                {
                    touch = Input.GetTouch(0); // get the last touch and store
                    if (touch.phase != TouchPhase.Ended) // ignore the last frame of an touch event
                    {
                        if (touch.position.x < screenMidPoint) // if position is at *left* half of the screen
                        {
                            aimSteering = -1;
                           
                        }
                        else // if position is at *right* half of the screen
                        {
                            aimSteering = 1;
                        }

                        if(touch.phase == TouchPhase.Began)
                        {
                                steering = 0;
                        }
                    } else
                    {
                        aimSteering = 0;
                    }
                }
                else
                {
                    aimSteering = 0;
                }

                steering = Mathf.Lerp(steering, aimSteering, steeringSensitivity * Time.fixedDeltaTime);
            }
            else if (mobilControlType == MobilControlTypes.Buttons) // Buttons
            {

            }

        }
        else if (controlType == ControlTypes.Editor)
        {
            steering = Input.GetAxis("Horizontal");

            if (!autoAcceleration)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    gasPedal = true;
                }
                else
                {
                    gasPedal = false;
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button2))
            {
                brakePedal = true;
            } else
            {
                brakePedal = false; 
            }

        }


        // fix steering value if it exceeds limits(clamp)
        if (steering > 1)
        {
            steering = 1;
        }
        else if (steering < -1)
        {
            steering = -1;
        } else if(Mathf.Abs(steering) < 0.01f)
        {
            steering = 0;
        }


    }
}
