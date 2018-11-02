using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class DeviceAtXRNode : MonoBehaviour
{
    public Text title;
    public Text value;
    public XRNode node;

    private InputDevice device;

    private void Start()
    {
        device = new InputDevice();

        title.text = node.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!device.isValid)
            value.text = "-";

        if (InputDevices.TryGetDeviceAtXRNode(node, out device))
            value.text = device.name;
    }
}
