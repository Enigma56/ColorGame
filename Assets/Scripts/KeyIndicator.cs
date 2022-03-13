using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIndicator : MonoBehaviour
{
    [SerializeField] float effectTimer = 6.5f;
    [SerializeField] AnimationCurve bounceAnim;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }


    public void triggerEffect()
    {
        StartCoroutine(ActiveRoutine());
    }

    IEnumerator ActiveRoutine()
    {
        spriteRenderer.enabled = true;
        float timer = 0;
        while (timer < effectTimer) // / effectTimer)
        {
            float normalizedTime = timer / effectTimer;
            timer += Time.deltaTime;
            float scale = bounceAnim.Evaluate(normalizedTime);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        transform.localScale = new Vector3(0, 0, 0);
        spriteRenderer.enabled = false;
    }
}
