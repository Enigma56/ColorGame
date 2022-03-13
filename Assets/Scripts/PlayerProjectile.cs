using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime;
    public float speed;
    public ParticleSystem impact;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Deconstructor), lifetime);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    void Deconstructor()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //impact effect
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        impact.Play();
        Invoke(nameof(Deconstructor),0.5f);
    }
}
