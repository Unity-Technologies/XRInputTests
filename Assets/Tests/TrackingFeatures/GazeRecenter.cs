using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.UI;

public class GazeRecenter : MonoBehaviour {

    public Image targetImage;
    private Collider m_Collider;

    private float m_Timer = 0.0f;
    public float minActiveValue = 0.4f;
    public float maxTimer = 1.5f;

	// Use this for initialization
	void Start () {
        m_Collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Collider != null && !Navigation.loadingScene)
        {
            RaycastHit hitInfo;
            bool hit = m_Collider.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hitInfo, float.MaxValue);
            float offset = hit ? Time.deltaTime : -Time.deltaTime;

            m_Timer += offset;

            if(offset > 0f && m_Timer < minActiveValue)
            {
                m_Timer = minActiveValue;
            }

            m_Timer = Mathf.Clamp(m_Timer, 0.0f, maxTimer);
            
            float t = m_Timer / maxTimer;

            if (targetImage != null)
                targetImage.color = new Color(t, 0, 0);

            if(m_Timer >= maxTimer)
            {
                InputTracking.Recenter();
                Debug.Log("Orientation Recentered.");

                m_Timer = 0;
            }
        }
	}
}
