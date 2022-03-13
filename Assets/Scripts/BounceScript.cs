using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    public float jumpBoost = 200;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpBoost, ForceMode2D.Impulse);
        }
    }
}
