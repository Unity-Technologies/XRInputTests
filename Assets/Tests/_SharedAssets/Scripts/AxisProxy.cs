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

    private void Start()
    {
        if (textComponent != null)
        {
            textComponent.text = axisName;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
