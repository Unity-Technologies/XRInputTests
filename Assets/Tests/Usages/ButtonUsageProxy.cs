using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ButtonUsageProxy : MonoBehaviour
{
    public XRNode node;
    public string usageName;

    public Text textComponent;
    public Image imageComponent;

    private void Start()
    {
        if (textComponent != null)
        {
            textComponent.text = usageName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool buttonState;

        if (imageComponent != null &&
            InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(new InputFeatureUsage<bool>(usageName), out buttonState))
        {
            if (buttonState)
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
