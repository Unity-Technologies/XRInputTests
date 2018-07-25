using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class SupportedAndLoadedDevicesToText : MonoBehaviour {

	public Text loadedDeviceText;
    public Text supportedDeviceText;

    // Use this for initialization
    void Start () {
        string displayTextAccumulator;

        // Loaded device
        loadedDeviceText.text = XRSettings.loadedDeviceName;
        
        // Supported devices (this build)
        displayTextAccumulator = "";
        foreach (string device in XRSettings.supportedDevices)
        {
            displayTextAccumulator += device + ", ";
        }
        supportedDeviceText.text = displayTextAccumulator;
    }
}
