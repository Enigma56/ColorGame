using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagentaInteractions : MonoBehaviour
{
    public bool key;
    public bool breakable;
    public ParticleSystem breakEffect;
    public GameObject door;
    public GameObject Lock;
    bool doorOpen;

    // Start is called before the first frame update
    void Start()
    {
        doorOpen = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile")){
            if (breakable)
            {
                StartCoroutine(wallShatter());
                AudioSingleton.Play("WallBreak");

            }
            if (key)
            {
                if (!doorOpen)
                {
                    StartCoroutine(UnlockDoor());
                    AudioSingleton.Play("DoorOpen");
                    GetComponentInChildren<KeyIndicator>().triggerEffect();
                }
            }
        }
    }

    IEnumerator wallShatter()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        breakEffect.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }


    // Make door disappear for 6.5 seconds with other effects
    IEnumerator UnlockDoor()
    {
        Vector3 doorScale = new Vector3(1.15f, 1, 1);
        for (int i = 2; i < 90; i += 2)
        {
            Lock.transform.rotation = Quaternion.Euler(0, 0, i);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        for (float i = 1; i > -0.1; i -= 0.1f)
        {
            doorScale.y = i;
            door.transform.localScale = doorScale;
            yield return new WaitForSeconds(0.02f);

        }
        doorOpen = true;
        door.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<Collider2D>().enabled = false;
        Lock.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(6.2f);
        door.GetComponent<SpriteRenderer>().enabled = true;
        door.GetComponent<Collider2D>().enabled = true;
        Lock.GetComponent<SpriteRenderer>().enabled = true;
        for (float i = 0; i < 1.1; i += 0.1f)
        {
            doorScale.y = i;
            door.transform.localScale = doorScale;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 90; i > -2; i -= 2)
        {
            Lock.transform.rotation = Quaternion.Euler(0, 0, i);
            yield return new WaitForEndOfFrame();
        }
        doorOpen = false;
    }
}
