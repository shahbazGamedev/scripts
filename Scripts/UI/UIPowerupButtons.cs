using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPowerupButtons : MonoBehaviour {
    static UIPowerupButtons instance;
    public float animTime = 0.15f;
    public float disabledIconScale = 0.5f;
    public GameObject[] buttons;
    RectTransform[] glows;
    RectTransform[][] icons; 
    public Color shieldGlowColor;
    public Color nitroGlowColor;
    public Color fireballGlowColor;
    public Color mineGlowColor;
    public Color defaultGlowColor;
    Color[] glowColors;
    public CarPowerups cp;
    public Image nitroShadow;
	// Use this for initialization
	void Start () {
        instance = this;

        glows = new RectTransform[buttons.Length];
        for(int i=0; i<buttons.Length; i++)
        {
            glows[i] = buttons[i].transform.FindChild("glow").GetComponent<RectTransform>();
            LeanTween.color(glows[i], defaultGlowColor, 0);
        }

        icons = new RectTransform[buttons.Length][];
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform[] powerups = new RectTransform[4];
            powerups[0] = buttons[i].transform.FindChild("icons/nitro").GetComponent<RectTransform>();
            powerups[1] = buttons[i].transform.FindChild("icons/shield").GetComponent<RectTransform>();
            powerups[2] = buttons[i].transform.FindChild("icons/fireball").GetComponent<RectTransform>();
            powerups[3] = buttons[i].transform.FindChild("icons/mine").GetComponent<RectTransform>();
            LeanTween.alpha(powerups[0], 0, 0);
            LeanTween.alpha(powerups[1], 0, 0);
            LeanTween.alpha(powerups[2], 0, 0);
            LeanTween.alpha(powerups[3], 0, 0);



            powerups[0].transform.localScale = disabledIconScale * Vector3.one;


            icons[i] = powerups;
        }

        glowColors = new Color[5];
        glowColors[0] = nitroGlowColor;
        glowColors[1] = shieldGlowColor;
        glowColors[2] = fireballGlowColor;
        glowColors[3] = mineGlowColor;
        glowColors[4] = defaultGlowColor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void usePowerup(int index)
    {
        cp.usePowerup(index);
    }
    
    void _powerupGainAnimation(int buttonIndex, CarPowerups.Powerup collectedPowerup)
    {
        int powerupIndex = (int)collectedPowerup;
        LeanTween.alpha(icons[buttonIndex][powerupIndex], 1, animTime);
        LeanTween.scale(icons[buttonIndex][powerupIndex], Vector3.one, animTime);
        LeanTween.color(glows[buttonIndex], glowColors[powerupIndex], animTime*2);
    }

    void _clearButton(int buttonIndex, CarPowerups.Powerup usedPowerup)
    {
        Debug.Log(buttonIndex + " - " + usedPowerup);
        int powerupIndex = (int)usedPowerup;
        LeanTween.alpha(icons[buttonIndex][powerupIndex], 0, animTime);
        LeanTween.scale(icons[buttonIndex][powerupIndex], Vector3.one * disabledIconScale, animTime);
        LeanTween.color(glows[buttonIndex], glowColors[4], animTime*2);
    }

    public static void clearButton(int buttonIndex, CarPowerups.Powerup usedPowerup)
    {
        instance._clearButton(buttonIndex, usedPowerup);
    }

    public static void powerupGainAnimation(int buttonIndex, CarPowerups.Powerup collectedPowerup)
    {
        instance._powerupGainAnimation(buttonIndex,collectedPowerup);
    }

    void _enableNitroShadow()
    {
        LeanTween.scale(nitroShadow.gameObject, Vector3.one, animTime);
    }

    public static void enableNitroShadow()
    {
        instance._enableNitroShadow();
    }

    void _disableNitroShadow()
    {
        LeanTween.scale(nitroShadow.gameObject, Vector3.one * 2, animTime);
    }

    public static void disableNitroShadow()
    {
        instance._disableNitroShadow();

    }
}
