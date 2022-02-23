using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public GameObject[] orbs;
    int currentColorIndex;
    List<string> colors = new List<string>();
    SpriteRenderer spriteRenderer;
    Vector4[] colorVectors = new Vector4[3];

    Vector2 orbPosition1 = new Vector2(0, 2f);  // Directly above, used for 2 orbs and 4 orbs

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
        //StartCoroutine(TestPickup());
        spinning = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Rotate();
        }
    }

    public void PickUpColor(string color)
    {
        currentColorIndex = colors.Count;
        spriteRenderer.color = colorVectors[currentColorIndex];
        colors.Add(color);
        PositionOrbs();
    }

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

    IEnumerator TestPickup()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("cyan");
        PickUpColor("cyan");
        yield return new WaitForSeconds(4f);
        Debug.Log("yellow");
        PickUpColor("yellow");
        yield return new WaitForSeconds(4f);
        Debug.Log("purple");
        PickUpColor("purple");
        yield return new WaitForSeconds(4f);
    }

    public string GetColor()
    {
        if (colors.Count != 0)
        {
            return colors[currentColorIndex];
        }
        return null;
    }

    public void Rotate()
    {
        if (!spinning && colors.Count > 1)
        {
            StartCoroutine(RotateOrbs());
        }
    }

    IEnumerator RotateOrbs()
    {
        spinning = true;
        orbs[currentColorIndex].transform.localPosition = Vector2.zero;
        orbs[currentColorIndex].GetComponent<Renderer>().enabled = true;
        for (int angle = 0; angle < 360/colors.Count; angle+=2)
        {
            for (int orb = 0; orb < colors.Count; orb++)
            {
                Vector2 pivot = GetComponentInParent<Transform>().localPosition + Vector3.up;
                orbs[orb].transform.RotateAround(pivot, Vector3.forward, -2);
            }
            yield return new WaitForEndOfFrame();
        }
        currentColorIndex = (currentColorIndex + 1) % colors.Count;
        orbs[currentColorIndex].GetComponent<Renderer>().enabled = false;
        spriteRenderer.color = colorVectors[currentColorIndex];
        spinning = false;
    }
}
