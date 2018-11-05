using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class ListDevicesAtNode : MonoBehaviour
{
    public Text listText;
    public XRNode node;

    void Start()
    {
        ShowDeviceNames();
        StartCoroutine("UpdateInputDeviceListEveryDelay", 3);
    }

    IEnumerator UpdateInputDeviceListEveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        ShowDeviceNames();
        StartCoroutine("UpdateInputDeviceListEveryDelay", delayInSeconds);
    }

    void ShowDeviceNames()
    {
        string displayTextAccumulator = node + ": ";

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.TryGetDevicesAtXRNode(node, inputDevices);

        foreach (InputDevice device in inputDevices)
        {
            displayTextAccumulator += (device.name + ", ");
        }
        listText.text = displayTextAccumulator;
    }
}
