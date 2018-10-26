using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class DiscreteUsageProxy : MonoBehaviour
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
        uint DiscreteState;

        if (InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(new InputUsage<uint>(usageName), out DiscreteState))
        {
            valueText.text = "0x" + DiscreteState.ToString("X8");
        }
    }
}
