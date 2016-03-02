using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISpeedoMeter : MonoBehaviour {
    Text speedText;
    public float multiplier = 3.5f;
    int speed = 0;
    CarPhysics cp;
	// Use this for initialization
	void Start () {
        cp = Player.instance.cp;
        speedText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        speed = Mathf.RoundToInt(cp.currentVelocity * multiplier);
        speedText.text = speed.ToString();
	}
}
