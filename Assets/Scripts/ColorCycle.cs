using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public bool startCyan, startYellow, startMagenta;
    public GameObject[] orbs;
    List<string> colors = new List<string>();
    SpriteRenderer spriteRenderer;

    int currentColorIndex;

    Vector4[] colorVectors = new Vector4[3];
    Vector2 orbPosition1 = new Vector2(0, 2f);  // Directly above, used for 2 orbs
    Vector2 orbPosition2 = new Vector2(Mathf.Sqrt(3)/2, 1.5f);
    Vector2 orbPosition3 = new Vector2(-Mathf.Sqrt(3)/2, 1.5f);

    bool spinning;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorVectors[0] = new Vector4(0, 1, 1, 1);
        colorVectors[1] = new Vector4(1, 1, 0, 1);
        colorVectors[2] = new Vector4(1, 0, 1, 1);
        if (startCyan) { PickUpColor("cyan"); }
        if (startYellow) { PickUpColor("yellow"); }
        if (startMagenta) { PickUpColor("magenta"); }

        spinning = false;
    }

    // Function when player touches a color
    public void PickUpColor(string color) //TODO: Change player color on pickup of an orb
    {
        currentColorIndex = colors.Count;
        spriteRenderer.color = colorVectors[currentColorIndex];
        colors.Add(color);
        PositionOrbs();
    }

    // determine how to position the orbs when the player picks up a new color
    void PositionOrbs() {
        for (int active = 0; active < currentColorIndex; active++)
        {
            orbs[active].GetComponent<Renderer>().enabled = true;
        }
        switch (colors.Count)
        {
            case 1:
                break;
            case 2:
                orbs[0].transform.localPosition = orbPosition1;
                break;
            case 3:
                orbs[0].transform.localPosition = orbPosition2;
                orbs[1].transform.localPosition = orbPosition3;
                break;
            default:
                break;
        }
    }

    // Get the current player color
    public string GetColor()
    {
        if (colors.Count != 0)
            return colors[currentColorIndex];
        
        return null;
    }

    // Function to trigger rotation coroutine, ensuring it happens once
    public void Rotate()
    {
        if (!spinning && colors.Count > 1)
        {
            StartCoroutine(RotateOrbs());
        }
    }

    // Coroutine to rotate orbs around the player and change their color
    IEnumerator RotateOrbs()
    {
        var colorStart = orbs[currentColorIndex].GetComponent<SpriteRenderer>().color;
        spinning = true;
        orbs[currentColorIndex].transform.localPosition = Vector2.zero;
        orbs[currentColorIndex].GetComponent<Renderer>().enabled = true;
        
        int angleChange = 2; //handles speed of rotation
        for (int angle = 0; angle < 360/colors.Count; angle+=angleChange)
        {
            for (int orb = 0; orb < colors.Count; orb++)
            {
                Vector2 pivot = GetComponentInParent<Transform>().localPosition + Vector3.up;
                orbs[orb].transform.RotateAround(pivot, Vector3.forward, -angleChange); 
            }
            yield return new WaitForEndOfFrame();
        }
        
        currentColorIndex = (currentColorIndex + 1) % colors.Count; 
        orbs[currentColorIndex].GetComponent<Renderer>().enabled = false; 
        //spriteRenderer.color = colorVectors[currentColorIndex];
        
        var colorEnd = orbs[currentColorIndex].GetComponent<SpriteRenderer>().color;
        StartCoroutine(ColorChangeAnimation(colorStart, colorEnd, .25f));
        
        spinning = false;
    }
    
    IEnumerator ColorChangeAnimation(Color start, Color end, float duration) {
        for (float t = 0f; t<duration; t += Time.deltaTime) {
            float normalizedTime = t/duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            spriteRenderer.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        spriteRenderer.color = end; //without this, the value will end at something like 0.9992367
    }
}
