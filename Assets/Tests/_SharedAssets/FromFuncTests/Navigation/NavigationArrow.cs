using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    private Material m_Material;
    private Collider m_Collider;

    private float m_Timer = 0.0f;
    public float minActiveValue = 0.4f;
    public float maxTimer = 1.5f;

    public float m_SceneIncrement = 1.0f;

    // Use this for initialization
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            m_Material = meshRenderer.material;
        }
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
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

            m_Material.SetFloat("_LerpFactor", t);

            if(m_Timer >= maxTimer)
            {
                if (Mathf.Sign(m_SceneIncrement) > 0.0f)
                {
                    Navigation.NextScene();
                }
                else
                {
                    Navigation.PreviousScene();
                }

                m_Timer = 0;
            }
        }
    }
}
