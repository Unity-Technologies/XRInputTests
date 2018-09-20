using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextTimer : MonoBehaviour
{
    private Text m_TimerText;

    // Use this for initialization
    void Start()
    {
        m_TimerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_TimerText.text = (Time.time % 100f).ToString("00.0");
    }
}
