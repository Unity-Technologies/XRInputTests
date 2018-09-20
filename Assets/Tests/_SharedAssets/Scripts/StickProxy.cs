using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickProxy : MonoBehaviour
{
    public string horizontalAxisId;
    public string verticalAxisId;
    public string axisName;

    public Text textComponent;
    public Slider horizontalSliderComponent;
    public Slider verticalSliderComponent;
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
        float horizontalValue = Input.GetAxis(horizontalAxisId);
        if (horizontalSliderComponent != null)
        {
            horizontalSliderComponent.value = horizontalValue;
        }

        float verticalValue = Input.GetAxis(verticalAxisId);
        if (verticalSliderComponent != null)
        {
            verticalSliderComponent.value = verticalValue;
        }

        if (valueTextComponent != null)
        {
            valueTextComponent.text = string.Format("[{0},{1}]", horizontalValue.ToString("F"), verticalValue.ToString("F"));
        }
    }
}
