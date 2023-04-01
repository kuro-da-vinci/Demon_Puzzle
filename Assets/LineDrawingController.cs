using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawingController : MonoBehaviour
{
    LineRenderer m_line;
    /// <summary>•`‰æ’†ƒtƒ‰ƒO</summary>
    bool m_isPainting;

    void Start()
    {
        Camera.main.orthographic = true;
        m_line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (m_isPainting)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                m_line.positionCount++;
                m_line.SetPosition(m_line.positionCount - 1, pos);
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_isPainting = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_line.positionCount = 0;
                m_isPainting = true;
            }
        }
    }
}