using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMobileForSceneChange {

    // Mobile input data
    private bool m_Dragging;
    private Vector2 m_TouchPoint, m_StartDrag, m_EndDrag;

	public void UpdateInputMobile(ref bool m_FlagMoveToPreviousScene, ref bool m_FlagMoveToNextScene)
    {
        Vector2 input_position = Vector2.zero;
            float swipe_threshold = 1f;
            bool input_down = false;
            bool input_state = false;
            bool input_up = false;
            
            //Touch-based input if controller is not connected.
            if (Input.touchSupported)
            {
                input_down = Input.GetTouch(0).phase == TouchPhase.Began;
                input_up = Input.GetTouch(0).phase == TouchPhase.Ended;
                input_state = Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary;
                input_position = Input.GetTouch(0).position;
                swipe_threshold = 10.0f;
            }
            else
            {
                input_position = Input.mousePosition;
                input_down = Input.GetMouseButtonDown(0);
                input_up = Input.GetMouseButtonUp(0);
                input_state = Input.GetMouseButton(0);
                swipe_threshold = 6f;
            }
            
            if (input_down)
            {
                m_TouchPoint = input_position;
            }
            else if (input_state)
            {
                if (input_position != m_TouchPoint && !m_Dragging)
                {
                    m_Dragging = true;
                    m_StartDrag = m_EndDrag = input_position;
                }
                else
                {
                    m_EndDrag = input_position;
                }
            }
            else if (input_up)
            {
                if (m_Dragging)
                {
                    m_Dragging = false;

                    if (Mathf.Abs(m_StartDrag.x - m_EndDrag.x) > swipe_threshold)
                    {
                        if (m_StartDrag.x < m_EndDrag.x)
                        {
                            m_FlagMoveToPreviousScene = true;
                        }
                        else if (m_StartDrag.x > m_EndDrag.x)
                        {
                            m_FlagMoveToNextScene = true;
                        }
                    }
                }

            }
    }
}
