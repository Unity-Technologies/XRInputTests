using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR;

public class GetJoystickNamesToText : MonoBehaviour {
    
    public Text joystickNamesText;

    void Start()
    {
        ShowJoystickNames();
        StartCoroutine("UpdateJoystickNamesEveryDelay", 3);
    }


    IEnumerator UpdateJoystickNamesEveryDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);

        ShowJoystickNames();
        StartCoroutine("UpdateJoystickNamesEveryDelay", delayInSeconds);
    }

	void ShowJoystickNames()
    {
        string displayTextAccumulator = "";
        int joystickNumber = 0;

        foreach (string device in Input.GetJoystickNames())
        {
            displayTextAccumulator += ("<" + joystickNumber + "> " + device + "\n");
            joystickNumber++;
        }
        joystickNamesText.text = displayTextAccumulator;
    }
}
