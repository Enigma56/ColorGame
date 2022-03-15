using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMethods : MonoBehaviour
{
    public GameObject PauseUI;

    private void Update()
    {
        //Perhaps a janky way to solve the issue
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.GetComponent<UIFunctionality>().ToggleUIVisibility();
        }
    }

    public void Quit()
    {
        Debug.Log("Quitted!");
        //When running game in Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;

        //Used in actual running game, not editor
        //Application.Quit(0);
    }
}
