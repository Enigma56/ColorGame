using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    public GameObject[] tileMaps;
    public GameObject[] coreOrbs;
    public GameObject bouncePlatforms;
    public GameObject magentaObjects;

    public GameObject leftFist;
    public GameObject rightFist;
    [SerializeField] AnimationCurve bounceAnim;

    public SpriteRenderer warning;

    bool slamCycle = true;
    PlayerController pc;
    CameraMovement cm;

    int bossLives = 0;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        cm = FindObjectOfType<CameraMovement>();
        if (AudioSingleton.finalCutscene)
        {
            cm.zoomOut();
            AudioSingleton.finalCutscene = false;
        }
        StartCoroutine(SlamEffect());
        StartCoroutine(SlamFists());
        StartCoroutine(RotateCore());
    }

    IEnumerator SlamEffect()
    {

        while (slamCycle)
        {
            yield return new WaitForSeconds(6.25f);
            warning.enabled = true;
            yield return new WaitForSeconds(0.25f);
            warning.enabled = false;
            yield return new WaitForSeconds(0.25f);
            warning.enabled = true;
            yield return new WaitForSeconds(0.25f);
            AudioSingleton.Play("BossSlam");
            pc.TriggerCycle();
            cm.ScreenShakeLight();
            warning.enabled = false;
        }
        yield return null;
    }

    IEnumerator SlamFists()
    {
        float timer = 0;
        while (timer < 6.9f && slamCycle)
        {
            float normalizedTime = timer / 7f;
            timer += Time.deltaTime;
            float y = bounceAnim.Evaluate(normalizedTime);
            leftFist.transform.localPosition = new Vector3(-10, y * 4f, 0);
            rightFist.transform.localPosition = new Vector3(10, y * 4f, 0);
            yield return null;
        }
        leftFist.transform.localPosition = new Vector3(-10, -4, 0);
        rightFist.transform.localPosition = new Vector3(10, -4, 0);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SlamFists());
    }

    IEnumerator FastSlam()
    {
        float timer = 0;
        coreOrbs[bossLives].GetComponent<SpriteRenderer>().enabled = false;
        while (timer < 1.9f && slamCycle)
        {
            float normalizedTime = timer / 2f;
            timer += Time.deltaTime;
            float y = bounceAnim.Evaluate(normalizedTime);
            leftFist.transform.localPosition = new Vector3(-10, y * 4f, 0);
            rightFist.transform.localPosition = new Vector3(10, y * 4f, 0);
            yield return null;
        }
        leftFist.transform.localPosition = new Vector3(-10, -4, 0);
        rightFist.transform.localPosition = new Vector3(10, -4, 0);
        AudioSingleton.Play("BossSlam");
        cm.ScreenShakeStrong();
        tileMaps[bossLives].SetActive(false);
        if (bossLives == 0)
        {
            bouncePlatforms.SetActive(false);
        }
        bossLives++;
        tileMaps[bossLives].SetActive(true);
        if (bossLives == 2)
        {
            magentaObjects.SetActive(true);
        }
        pc.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        pc.gameObject.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 50; i++)
        {
            pc.gameObject.transform.position = Vector2.MoveTowards(pc.gameObject.transform.position, Vector2.down * 3, 2);
            yield return new WaitForSeconds(0.05f);
        }
        pc.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        pc.gameObject.GetComponent<Collider2D>().enabled = true;
        StartCoroutine(SlamEffect());
        StartCoroutine(SlamFists());
        pc.movementLocked = false;
    }

    IEnumerator RotateCore()
    {
        while (bossLives < 3)
        {
            for (float angle = 0; angle < 120; angle += (bossLives+1))
            {
                for (int orb = 0; orb < 3; orb++)
                {
                    Vector2 pivot = Vector2.up * 14f;
                    coreOrbs[orb].transform.RotateAround(pivot, Vector3.forward, bossLives+1);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void SpikeFalls()
    {
        cm.zoomOut();
        pc.movementLocked = true;
        warning.enabled = false;
        pc.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void SpikeLands()
    {
        if (bossLives != 2)
        {
            StopAllCoroutines();
            StartCoroutine(RotateCore());
            StartCoroutine(FastSlam());
            cm.ScreenShakeStrong();
        }
        else
        {
            cm.ScreenShake(1, 1);
            Invoke(nameof(GameComplete), 1f);
        } 
    }

    void GameComplete()
    {
        AudioSingleton.NextScene();
    }
}
