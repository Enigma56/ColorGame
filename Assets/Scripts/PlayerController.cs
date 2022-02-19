using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public ColorCycle cycle;
    Rigidbody2D rb2D;
    int jumpNumber;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rb2D.velocity;
        float input = Input.GetAxis("Horizontal");
        float xMovement = input * speed;
        vel.x = xMovement;

        // Trigger jump --> Double jump if cyan
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vel = Jump(vel);
        }

        if (cycle.GetColor() == "yellow")
        {
            speed = 9f;
        }
        else
        {
            speed = 5f;
        }

        // Trigger color change
        if (Input.GetKey(KeyCode.LeftShift))
        {
            cycle.Rotate();
        }

        // Check jump reset
        if (jumpNumber > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Platform"))
                {
                    jumpNumber = 0;
                }
            }
        }

        rb2D.velocity = vel;
    }

Vector2 Jump(Vector2 velocity)
    {
        if (jumpNumber == 0)
        {
            jumpNumber = 1;
            velocity.y = jumpForce;
        }
        else if (jumpNumber == 1 && cycle.GetColor() == "cyan")
        {
            jumpNumber = 2;
            velocity.y = jumpForce;
        }
        return velocity;
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CyanOrb"))
        {
            cycle.PickUpColor("cyan");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("YellowOrb"))
        {
            cycle.PickUpColor("yellow");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("MagentaOrb"))
        {
            cycle.PickUpColor("magenta");
            Destroy(other.gameObject);
        }
    }
}
