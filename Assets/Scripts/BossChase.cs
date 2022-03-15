using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : MonoBehaviour
{
    public float startSpeed;
    public float speedIncrement;
    public float timeBeforeStart;
    public GameObject player;
    public int tailDistance;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(startChase(timeBeforeStart));
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.x < player.GetComponent<Rigidbody2D>().position.x - tailDistance)
        {
            rb.position = new Vector2(player.GetComponent<Rigidbody2D>().position.x - tailDistance, rb.position.y);
        }
    }

    IEnumerator startChase(float wait)
    {
        yield return new WaitForSeconds(wait);
        rb.velocity = new Vector2(startSpeed, 0);
        while (true)
        {
            // start speed = 5, increment = 0.4, distance = 225 (double jumps: 4)
            yield return new WaitForSeconds(1f);
            startSpeed += speedIncrement;
            rb.velocity = new Vector2(startSpeed, 0);
        }
    }
}
