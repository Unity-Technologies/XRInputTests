using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxisProxy : MonoBehaviour
{


    public string axisId;
    public string axisName;   

    public Text textComponent;
    public Slider sliderComponent;
    public Text valueTextComponent;

    // Update is called once per frame
    void Update()
    {
        if (textComponent != null)
        {
            textComponent.text = axisName;
        }

        float value = Input.GetAxis(axisId);
        if (sliderComponent != null)
        {
            sliderComponent.value = value;
        }

        if (valueTextComponent != null)
        {
            valueTextComponent.text = value.ToString("F");
        }
    }
}
