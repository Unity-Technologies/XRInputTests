using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class ListInputDevicesRoles : MonoBehaviour
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
            displayTextAccumulator += ("<" + nodeNumber + "> " + " - Name: \"" + device.name + "\" - Characteristics: \"" + device.characteristics);

            displayTextAccumulator += "\"\n";
            nodeNumber++;
        }
        nodeNamesText.text = displayTextAccumulator;
    }
}
