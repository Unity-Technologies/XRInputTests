using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class ListFeatureUsages : MonoBehaviour
{
    public XRNode node;
    public Text listText;

    public float delayTime = 1f;

    void Start()
    {
        ShowFeatures();
        StartCoroutine("UpdateFeatureNamesEveryDelay", delayTime);
    }

    IEnumerator UpdateFeatureNamesEveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        ShowFeatures();
        StartCoroutine("UpdateFeatureNamesEveryDelay", delayInSeconds);
    }

    void ShowFeatures()
    {
        string displayTextAccumulator = "";
        int nodeNumber = 0;

        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (device == null)
            displayTextAccumulator = "Device not found at node " + node.ToString();
        else
        {
            List<InputFeatureUsage> features = new List<InputFeatureUsage>();

            if (device.TryGetFeatureUsages(features))
                foreach (InputFeatureUsage feature in features)
                {
                    displayTextAccumulator += ("<" + nodeNumber + "> " + feature.name + " ");

                    // Can't use switch statement because types are not considered constant
                    if (feature.type == typeof(bool))
                        displayTextAccumulator += TextAccumBool(device, feature);
                    if (feature.type == typeof(float))
                        displayTextAccumulator += TextAccumFloat(device, feature);
                    if (feature.type == typeof(Vector2))
                        displayTextAccumulator += TextAccumVector2(device, feature);
                    if (feature.type == typeof(Vector3))
                        displayTextAccumulator += TextAccumVector3(device, feature);
                    if (feature.type == typeof(uint))
                        displayTextAccumulator += TextAccumUint(device, feature);

                    displayTextAccumulator += ("\n");
                    nodeNumber++;
                }
            else
                displayTextAccumulator = "No Features were found!";
        }

        listText.text = displayTextAccumulator;
    }
    
    string TextAccumBool(InputDevice device, InputFeatureUsage feature)
    {
        bool value;
        device.TryGetFeatureValue(feature.As<bool>(), out value);
        return value.ToString();
    }
    string TextAccumFloat(InputDevice device, InputFeatureUsage feature)
    {
        float value;
        device.TryGetFeatureValue(feature.As<float>(), out value);
        return value.ToString();
    }
    string TextAccumVector2(InputDevice device, InputFeatureUsage feature)
    {
        Vector2 value;
        device.TryGetFeatureValue(feature.As<Vector2>(), out value);
        return value.ToString();
    }
    string TextAccumVector3(InputDevice device, InputFeatureUsage feature)
    {
        Vector3 value;
        device.TryGetFeatureValue(feature.As<Vector3>(), out value);
        return value.ToString();
    }
    string TextAccumUint(InputDevice device, InputFeatureUsage feature)
    {
        uint value;
        device.TryGetFeatureValue(feature.As<uint>(), out value);
        return value.ToString();
    }
}
