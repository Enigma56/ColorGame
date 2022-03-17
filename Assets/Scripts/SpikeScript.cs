using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    BossFight bf;
    Rigidbody2D rb;
    public ParticleSystem breakEffect;

    // Start is called before the first frame update
    void Start()
    {
        bf = FindObjectOfType<BossFight>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            rb.gravityScale = 1;
            bf.SpikeFalls();
        }
        if (collision.collider.CompareTag("Boss"))
        {
            bf.SpikeLands();
            //Trigger Effect
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true;
            breakEffect.Play();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

    }
}
