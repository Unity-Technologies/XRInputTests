using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSprite : MonoBehaviour {

    public Transform[] m_GazeableTransforms;
    public float m_Angle;
    private MeshRenderer m_MeshRenderer;
    private Material m_Material;
    public Color m_Color;

    public float m_Distance = 2.0f;
    

	// Use this for initialization
	void Start () {
        MeshRenderer m_MeshRenderer = GetComponent<MeshRenderer>();
        if(m_MeshRenderer != null)
        {
            m_Material = m_MeshRenderer.material;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (m_Material != null)
        {
            float bestAngle = float.MaxValue;

            foreach (Transform t in m_GazeableTransforms)
            {
                Vector3 dir = (t.transform.position - Camera.main.transform.position).normalized;
                float angle = Mathf.Abs(Vector3.Angle(dir, Camera.main.transform.forward));

                if (angle < bestAngle)
                {
                    bestAngle = angle;
                }
            }
            
            float delta = m_Angle - bestAngle;
            float lerpFactor = 0.0f;
            if(delta > 0f)
            {
                lerpFactor = 1.0f - (bestAngle / m_Angle);
            }
            
            m_Material.SetColor("_TintColor", Color.Lerp(Color.clear, m_Color, lerpFactor));

            if (m_MeshRenderer != null)
            {
                m_MeshRenderer.enabled = lerpFactor > 0f;
            }
        }

        
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * m_Distance;
        transform.forward = -Camera.main.transform.forward;
    }
}
