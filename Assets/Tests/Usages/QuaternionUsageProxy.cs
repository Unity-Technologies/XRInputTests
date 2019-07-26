using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class QuaternionUsageProxy : MonoBehaviour
{
    public XRNode node;
    public string usageName;

    public Text titleText;
    public Text valueText;

    private void Start()
    {
        if (titleText != null)
            titleText.text = usageName;

        if (valueText != null)
            valueText.text = "No Value";
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion State;
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(new InputFeatureUsage<Quaternion>(usageName), out State))
        {
            valueText.text = State.ToString();
        }
    }
}
