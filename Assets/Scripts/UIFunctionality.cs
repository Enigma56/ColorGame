using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIFunctionality : MonoBehaviour
{
    // Start is called before the first frame update
    public bool UIVisibility;
    public Button ExitButton;
    void Start()
    {
        UIVisibility = false;
        gameObject.SetActive(UIVisibility);


    }
    
    //Potentially add time scale changes to pause the game entirely 
    public void ToggleUIVisibility()
    {
        if (UIVisibility)
        {
            UIVisibility = false;
            gameObject.SetActive(UIVisibility);
        }
        else
        {
            UIVisibility = true;
            gameObject.SetActive(UIVisibility);
        }
    }
}
