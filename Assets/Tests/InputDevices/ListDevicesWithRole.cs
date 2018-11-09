using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class ListDevicesWithRole : MonoBehaviour
{
    public Text listText;
    public InputDeviceRole role;

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
        string displayTextAccumulator = role + ": ";

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithRole(role, inputDevices);

        foreach (InputDevice device in inputDevices)
        {
            displayTextAccumulator += (device.name + ", ");
        }
        listText.text = displayTextAccumulator;
    }
}
