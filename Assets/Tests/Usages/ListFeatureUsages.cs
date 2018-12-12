using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class ListFeatureUsages : MonoBehaviour
{
    public XRNode node;
    public Text listText;

    void Start()
    {
        ShowFeatures();
        StartCoroutine("UpdateFeatureNamesEveryDelay", 3);
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
                    displayTextAccumulator += ("<" + nodeNumber + "> " + feature.name + " - \"" + feature.type.ToString() + "\"\n");
                    nodeNumber++;
                }
            else
                displayTextAccumulator = "No Features were found!";
        }

        listText.text = displayTextAccumulator;
    }
}
