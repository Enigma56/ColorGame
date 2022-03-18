using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuScript : MonoBehaviour
{
    public VideoClip titleAnimation;
    public VideoPlayer vp;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(Opening());
        }
        else
        {
            StartCoroutine(Closing());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        AudioSingleton.NextScene();
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Opening()
    {
        yield return new WaitForSeconds(14);
        vp.clip = titleAnimation;
        vp.isLooping = true;
    }

    IEnumerator Closing()
    {
        yield return new WaitForSeconds(34);
        SceneManager.LoadScene(0);
    }
}

