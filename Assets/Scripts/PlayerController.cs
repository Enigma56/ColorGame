using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float projectileTimer;
    public bool movementLocked;
    public ColorCycle cycle;
    public GameObject projectile;
    public ParticleSystem yellowEffect;
    public ParticleSystem cyanEffect;
    public ParticleSystem magentaEffect;
    public AudioClip deathSound;

    bool checkJump, checkColor, canShoot, active;
    Rigidbody2D rb2D;
    int jumpNumber;
    float lastInput;

    private float _lockTime = 6f;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        canShoot = true;
        checkColor = false;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !movementLocked)
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
            // If player color is yellow --> increase speed
            if (cycle.GetColor() == "yellow")
            {
                speed = 9f;
            }
            else
            {
                speed = 5f;
            }
            // Trigger color change
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TriggerCycle();
            }
            // Track last input in order to fire projectile in the direction the player last moved
            if (input != 0)
            {
                lastInput = input;
            }
            // trigger firing projectile in the direction the player is moving (default right)
            if (Input.GetKey(KeyCode.RightShift) && cycle.GetColor() == "magenta")
            {
                ShootProjectile(lastInput);
            }
            // Check jump reset
            if (checkJump && jumpNumber > 0)
            {
                CheckJump();
            }
            rb2D.velocity = vel;
        }
    }

    public void TriggerCycle()
    {
        AudioSingleton.Play("Cycle");
        cycle.Rotate();
        if (!checkColor)
        {
            checkColor = true;
            Invoke(nameof(CheckColor), 0.5f);
        }
    }


    // Jump function that checks conditions and boosts y velocity
    Vector2 Jump(Vector2 velocity)
    {
        if (jumpNumber == 0)
        {
            jumpNumber = 1;
            velocity.y = jumpForce;
            checkJump = false;
        }
        else if (jumpNumber == 1 && cycle.GetColor() == "cyan")
        {
            jumpNumber = 2;
            velocity.y = jumpForce;
            checkJump = false;
        }
        checkJump = false;
        StartCoroutine(JumpFrames());
        return velocity;
    }

    // Wait a few frames before checking to reset jump so that the player does not gain double/triple jump
    IEnumerator JumpFrames()
    {
        yield return new WaitForSeconds(0.05f);
        checkJump = true;
    }

    // Fire raycasts down to see if player is ontop of a platform, in which case reset jumps
    void CheckJump()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + (Vector3.left * 0.4f), Vector2.down, 0.52f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + (Vector3.right * 0.4f), Vector2.down, 0.52f);
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            jumpNumber = 0;
        }
    }

    // When player shifts color, check to see player color matches platform they are on
    public void CheckColor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.52f);
        checkColor = false;

        //Changed by Charles
        if (hit.collider == null) //guard clause for cleaner code
            return;

        if (!hit.collider.CompareTag(cycle.GetColor()) && !hit.collider.CompareTag("Platform"))
        {
            PlayerDeath();
        }

    }

    // Shoot projectile by instantiating projectile object to left or right of player
    void ShootProjectile(float inputDirection)
    {
        if (!canShoot)
        {
            return;
        }
        canShoot = false;
        if (inputDirection >= 0)
        {
            Instantiate(projectile, transform.position + Vector3.right, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Instantiate(projectile, transform.position + Vector3.left, Quaternion.Euler(0, 0, 180));
        }
        Invoke(nameof(ResetShoot), projectileTimer);
    }

    // wait for (projectileTimer) seconds before allowing the player to shoot again
    void ResetShoot()
    {
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CyanOrb"))
        {
            movementLocked = true;
            cycle.PickUpColor("cyan", true);
            Destroy(other.gameObject);
            Invoke(nameof(FreeMovement), _lockTime);
        }
        else if (other.CompareTag("YellowOrb"))
        {
            movementLocked = true;
            cycle.PickUpColor("yellow", true);
            Destroy(other.gameObject);
            Invoke(nameof(FreeMovement), _lockTime);
        }
        else if (other.CompareTag("MagentaOrb"))
        {
            movementLocked = true;
            cycle.PickUpColor("magenta", true);
            Destroy(other.gameObject);
            Invoke(nameof(FreeMovement), _lockTime);
        }
        else if (other.CompareTag("Boss"))
        {
            PlayerDeath();
        }
        else if (other.CompareTag("AllowMove"))
        {
            movementLocked = false;
        }
        else if (other.CompareTag("StageExit"))
        {
            StageComplete();
        }
        rb2D.velocity = Vector2.zero;
    }

    void FreeMovement()
    {
        movementLocked = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
            return;

        if (other.gameObject.CompareTag("Obstacle"))
        {
            PlayerDeath();
        }
        else if (!other.gameObject.tag.Equals(cycle.GetColor()))
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        active = false;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject obj in cycle.orbs)
        {
            obj.GetComponent<SpriteRenderer>().enabled = false;
        }
        rb2D.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb2D.isKinematic = true;
        AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (cycle.GetColor() == "yellow")
        {
            yellowEffect.transform.position = transform.position;
            yellowEffect.Play();
        }
        else if (cycle.GetColor() == "cyan")
        {
            cyanEffect.transform.position = transform.position;
            cyanEffect.Play();
        }
        else if (cycle.GetColor() == "magenta")
        {
            magentaEffect.transform.position = transform.position;
            magentaEffect.Play();
        }

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void StageComplete()
    {
        if (AudioSingleton.instance != null)
        {
            AudioSingleton.NextScene();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
