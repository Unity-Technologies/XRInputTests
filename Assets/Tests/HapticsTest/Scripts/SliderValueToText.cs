using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderValueToText : MonoBehaviour
{
    public Text targetText;
    private Slider m_Slider;

    // Use this for initialization
    void OnEnable()
    {
        m_Slider = GetComponent<Slider>();
        ChangeText(m_Slider.value);

        // Do all future updates based off of the Slider changed event
        m_Slider.onValueChanged.AddListener(ChangeText);
    }

    // Update is called once per frame
    void ChangeText(float value)
    {
        targetText.text = value.ToString();
    }
}
