using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class AxisUsageProxy : MonoBehaviour
{
    public XRNode node;
    public string usageName;

    public Text textComponent;
    public Slider sliderComponent;
    public Text valueTextComponent;
    
    public float currentValue { get; private set; }

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
        float value;
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (!InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(new InputFeatureUsage<float>(usageName), out value))
            return;

        currentValue = value;

        if (sliderComponent != null)
        {
            sliderComponent.value = value;
        }

        if (valueTextComponent != null)
        {
            valueTextComponent.text = value.ToString("F");
        }
    }
}
