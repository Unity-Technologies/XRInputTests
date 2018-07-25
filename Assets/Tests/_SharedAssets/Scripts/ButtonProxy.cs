using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProxy : MonoBehaviour
{
    public string buttonId;
    public string buttonName;

    public Text textComponent;
    public Image imageComponent;

    private void Start()
    {
        if (textComponent != null)
        {
            textComponent.text = buttonName;
        }
    }

    // Update is called once per frame
    void Update ()
    {

        if(imageComponent != null)
        {
            if (Input.GetButton(buttonId))
            {
                imageComponent.color = Color.green;
            }
            else
            {
                imageComponent.color = Color.red;
            }
        }             
	}
}
