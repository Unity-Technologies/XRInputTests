using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AxisRangeCheck : MonoBehaviour
{
    [Tooltip("If left empty, this will try to find an image on the current gameObject.")]
    public Image panelImage;

    private AxisUsageProxy[] m_Axes;
    private StickUsageProxy[] m_2DAxes;
    private Color m_InitialPanelColor;

    // Start is called before the first frame update
    void Start()
    {
        m_Axes = GetComponentsInChildren<AxisUsageProxy>();
        m_2DAxes = GetComponentsInChildren<StickUsageProxy>();

        if (panelImage == null)
            panelImage = GetComponent<Image>();

        if (panelImage != null)
            m_InitialPanelColor = panelImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        bool rangeErrorFound = false;

        foreach (AxisUsageProxy axis in m_Axes)
        {
            if (axis.currentValue < axis.sliderComponent.minValue
                || axis.currentValue > axis.sliderComponent.maxValue)
            {
                rangeErrorFound = true;
                Debug.LogError("Usage " + axis.usageName + " has value " 
                    + axis.currentValue + ", which is out of the expected range of [" 
                    + axis.sliderComponent.minValue + ", " 
                    + axis.sliderComponent.maxValue + "].");
            }
        }

        foreach (StickUsageProxy axis2D in m_2DAxes)
        {
            if (axis2D.currentXValue < axis2D.horizontalSliderComponent.minValue
                || axis2D.currentXValue > axis2D.horizontalSliderComponent.maxValue)
            {
                rangeErrorFound = true;
                Debug.LogError("Usage " + axis2D.usageName + " horizontal value has value " 
                    + axis2D.currentXValue + ", which is out of the expected range of [" 
                    + axis2D.horizontalSliderComponent.minValue + ", " 
                    + axis2D.horizontalSliderComponent.maxValue + "].");
            }

            if (axis2D.currentYValue < axis2D.verticalSliderComponent.minValue
                || axis2D.currentYValue > axis2D.verticalSliderComponent.maxValue)
            {
                rangeErrorFound = true;
                Debug.LogError("Usage " + axis2D.usageName + " vertical value has value " 
                    + axis2D.currentYValue + ", which is out of the expected range of [" 
                    + axis2D.verticalSliderComponent.minValue + ", " 
                    + axis2D.verticalSliderComponent.maxValue + "].");
            }
        }

        if (panelImage != null)
        {
            if (rangeErrorFound)
                panelImage.color = Color.red;
            else
                panelImage.color = m_InitialPanelColor;
        }

    }
}
