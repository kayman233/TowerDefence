using Field;
using UnityEngine;
using Grid = Field.Grid;
namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargettNode(grid.GetTargetNode());
        }

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;

            float distance = (target - m_Transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                return;
            }

            Vector3 dir = (target - m_Transform.position);
            dir.y = 0f;
            Vector3 delta = dir.normalized * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargettNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}