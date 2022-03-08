using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagentaInteractions : MonoBehaviour
{
    public bool key;
    public bool breakable;
    public GameObject door;
    bool doorOpen;

    // Start is called before the first frame update
    void Start()
    {
        doorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile")){
            if (breakable)
            {
                //trigger break effect
                Destroy(gameObject);
                //trigger projectile effect
                Destroy(collision.collider.gameObject);
                
            }
            if (key)
            {
                //trigger effect
                if (!doorOpen)
                {
                    StartCoroutine(UnlockDoor());
                }
                //trigger projectile effect
                Destroy(collision.collider.gameObject);
            }
        }
    }

    IEnumerator UnlockDoor()
    {
        doorOpen = true;
        door.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(6f);
        door.GetComponent<SpriteRenderer>().enabled = true;
        door.GetComponent<Collider2D>().enabled = true;
        doorOpen = false;
    }
}
