using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class GazeTogglePosTracking : MonoBehaviour {
    
    private Collider m_Collider;
    public Image targetImage;

    private float m_Timer = 0.0f;
    public float m_MinActiveValue = 0.4f;
    public float m_MaxTimer = 1.5f;

    public Text trackingStatus;

	// Use this for initialization
	void Start () {
        m_Collider = GetComponent<Collider>();

        trackingStatus.text = "disablePosTracking = " + InputTracking.disablePositionalTracking;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Collider != null && !Navigation.loadingScene)
        {
            RaycastHit hitInfo;
            bool hit = m_Collider.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, float.MaxValue);
            float offset = hit ? Time.deltaTime : -Time.deltaTime;

            m_Timer += offset;
            
            if(offset > 0f && m_Timer < m_MinActiveValue)
            {
                m_Timer = m_MinActiveValue;
            }

            m_Timer = Mathf.Clamp(m_Timer, 0.0f, m_MaxTimer);
            
            float t = m_Timer / m_MaxTimer;

            if (targetImage != null)
                targetImage.color = new Color(t, 0, 0);

            if(m_Timer >= m_MaxTimer)
            {
                InputTracking.disablePositionalTracking = !InputTracking.disablePositionalTracking;
                Debug.Log("InputTracking.disablePositionalTracking set to " + InputTracking.disablePositionalTracking);
                trackingStatus.text = "disablePosTracking = " + InputTracking.disablePositionalTracking;

                m_Timer = 0;
            }
        }
	}
}
