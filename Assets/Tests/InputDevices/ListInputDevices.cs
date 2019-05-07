using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class ListInputDevices : MonoBehaviour
{
    public Text nodeNamesText;

    void Start()
    {
        ShowNodeNames();
        StartCoroutine("UpdateInputDeviceListEveryDelay", 3);
    }

    IEnumerator UpdateInputDeviceListEveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        ShowNodeNames();
        StartCoroutine("UpdateInputDeviceListEveryDelay", delayInSeconds);
    }

    void ShowNodeNames()
    {
        string displayTextAccumulator = "";
        int nodeNumber = 0;

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevices(inputDevices);

        foreach (InputDevice device in inputDevices)
        {  
            displayTextAccumulator += string.Format("<{0}> - Name: \"{1}\" - Role: \"{2}\" - Manufacturer: \"{3}\" Serial Number: \"{4}\"\n", nodeNumber, device.name, device.role, device.manufacturer, device.serialNumber);
            nodeNumber++;
        }
        nodeNamesText.text = displayTextAccumulator;
    }
}
