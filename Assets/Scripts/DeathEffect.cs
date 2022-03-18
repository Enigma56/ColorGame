using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathEffect : MonoBehaviour
{
    public ParticleSystem deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        DeathAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeathAnimation()
    {
        deathEffect.Play();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
