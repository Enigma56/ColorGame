using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSingleton : MonoBehaviour
{
    public Sound[] sounds;
    string activeSound;

    private static AudioSingleton _instance;
    public static AudioSingleton instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioSingleton>();
                if (_instance == null)
                {
                    throw new UnityException("Instance of AudioSingleton not found in scene");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null) {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(_instance);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        //Play(MenuMusic);
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
        _instance.activeSound = name;
    }

    // Stop a Sound by name, ex. Stop("Song1")
    public static void Stop(string name)
    {
        Sound s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public static void NextScene()
    {
        Stop(_instance.activeSound);
        int scene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scene);
        if (scene == 1)
        {
            //Play(IntroStagesMusic);
        }
        else if (scene == 3)
        {
            //Play(CyanStagesMusic);
        }
        else if (scene == 6) // may need to change
        {
            //Play(YellowStagesMusic);
        }
        else if (scene == 10)
        {
            //Play(MagentaStagesMusic);
        }
        else if (scene == 14)
        {
            //Play(BossMusic);
            FindObjectOfType<CameraMovement>().zoomOut();
        }
    }
}
