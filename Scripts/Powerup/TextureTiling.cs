using UnityEngine;
using System.Collections;

public class TextureTiling : MonoBehaviour {
    public float speed = 1;
    public float fadeTime = 0.2f;
    bool enabled = false;
    float xPos = 0;
    float yPos = 0;
    float yScale = 1;
    Material m;
	// Use this for initialization
	void Awake () {
        m = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled)
        {
            yPos += Time.fixedDeltaTime * speed;
            m.mainTextureOffset = Vector2.up * yPos;
        }
	}

    public void enable()
    {
        yPos = 0;
        m.mainTextureOffset = Vector2.up * yPos;
        StartCoroutine(appear()); 
    }

    public void disable()
    {
        yPos = 0;
        m.mainTextureOffset = Vector2.up * yPos;
        enabled = false;
        StartCoroutine(disappear());
    }

    IEnumerator disappear()
    {
        float decay = 1 / fadeTime * Time.fixedDeltaTime;
        yScale = 1;

        while (yScale > 0)
        {
            yScale -= decay * 2.5f;
            m.mainTextureScale = Vector2.up * yScale;
            yield return new WaitForEndOfFrame();
        }
        m.mainTextureScale = Vector2.zero;
    }

    IEnumerator appear()
    {
        float decay = 1 / fadeTime * Time.fixedDeltaTime;
        yScale = 0;
        while (yScale < 1)
        {
            yScale += decay;
            m.mainTextureScale = Vector2.up * yScale;
            yield return new WaitForEndOfFrame();
        }
        m.mainTextureScale = Vector2.up;
        enabled = true;
    }
}
