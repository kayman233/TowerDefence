using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;
        [SerializeField] private float m_NodeSize;
        [SerializeField] private MovementAgent m_MovementAgent;
        [SerializeField] private GameObject m_Cursor;

        private Camera m_Camera;

        private Vector3 m_Offset;

        private float m_Width;
        private float m_Height;

        private void OnValidate()
        {
            m_Camera = Camera.main;

            // Default plane size is 10 by 10
            m_Width = m_GridWidth * m_NodeSize;
            m_Height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(
                m_Width * 0.1f,
                1f,
                m_Height * 0.1f);
            m_Offset = transform.position -
                       (new Vector3(m_Width, 0f, m_Height) * 0.5f);

            // for beauty
            m_Cursor.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f) * m_NodeSize;
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);
                
                // center of the Node (x,y)
                Vector3 centeredPosition = m_Offset + new Vector3(x + 0.5f,0,y + 0.5f) * m_NodeSize;
                
                // set a new target on mouse click
                if (Input.GetMouseButtonDown(0))
                {
                    m_MovementAgent.SetTarget(centeredPosition);
                }
                
                m_Cursor.SetActive(true);
                m_Cursor.transform.position = centeredPosition;
            }
            else
            {
                // if cursor is out of the grid
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(m_Offset, 0.1f);
            for (int i = 0; i < m_GridWidth + 1; i++)
            {
                // start of the segment(vertical)
                Vector3 start = m_Offset + new Vector3(i, 0, 0) * m_NodeSize;
                Vector3 end = start + new Vector3(0, 0, m_Height);
                Gizmos.DrawLine(start, end);
            }
            for (int i = 0; i < m_GridHeight + 1; i++)
            {
                // horizontal
                Vector3 start = m_Offset + new Vector3(0, 0, i) * m_NodeSize;
                Vector3 end = start + new Vector3(m_Width, 0, 0);
                Gizmos.DrawLine(start, end);
            }
        }
    }
}