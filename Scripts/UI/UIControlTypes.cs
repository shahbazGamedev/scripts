using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControlTypes : MonoBehaviour {
   
    Text controlText;

	void Start () {
        controlText = transform.GetChild(0).GetComponent<Text>();
	}
	
	// Update is called once per frame

    public void changeControl()
    {
        if(CarInput.instance.mobilControlType == CarInput.MobilControlTypes.Accelerometer)
        {
            controlText.text = "Control: Screen";
            CarInput.instance.controlType = CarInput.ControlTypes.Mobile;
            CarInput.instance.mobilControlType = CarInput.MobilControlTypes.Screen;

        } else if(CarInput.instance.mobilControlType == CarInput.MobilControlTypes.Screen)
        {
            controlText.text = "Control: Acceleration";
            CarInput.instance.controlType = CarInput.ControlTypes.Mobile;
            CarInput.instance.mobilControlType = CarInput.MobilControlTypes.Accelerometer;
        }

    }
}
