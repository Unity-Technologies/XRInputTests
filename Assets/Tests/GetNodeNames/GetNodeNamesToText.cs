using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class GetNodeNamesToText : MonoBehaviour
{
    public Text nodeNamesText;

    void Start()
    {
        ShowNodeNames();
        StartCoroutine("UpdateNodeNamesEveryDelay", 3);
    }

    IEnumerator UpdateNodeNamesEveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        ShowNodeNames();
        StartCoroutine("UpdateNodeNamesEveryDelay", delayInSeconds);
    }

    void ShowNodeNames()
    {
        string displayTextAccumulator = "";
        int nodeNumber = 0;

        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (XRNodeState node in nodeStates)
        {
            displayTextAccumulator += ("<" + nodeNumber + "> " + node.nodeType + " - \"" + InputTracking.GetNodeName(node.uniqueID) + "\"\n");
            nodeNumber++;
        }
        nodeNamesText.text = displayTextAccumulator;
    }
}
