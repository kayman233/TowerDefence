using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private FlowFieldPathfinding m_Pathfinding;

        private int m_Width;
        private int m_Height;

        private Vector3 m_Offset;
        private float m_NodeSize;

        private Vector2Int m_StartCoordinate;
        private  Vector2Int m_TargetCoordinate;
        
        private Node m_SelectedNode = null;

        public int Width => m_Width;

        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int start, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;

            m_Offset = offset;
            m_NodeSize = nodeSize;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                    // default cache values
                    m_Nodes[i, j].OccupationAvailability = OccupationAvailability.CanOccupy; 
                }
            }
            GetNode(start).OccupationAvailability = OccupationAvailability.CanNotOccupy;
            GetNode(target).OccupationAvailability = OccupationAvailability.CanNotOccupy;
            
            m_Pathfinding = new FlowFieldPathfinding(this, start, target);
            m_Pathfinding.UpdateField();
        }
        
        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }
        
        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }
        
        public void UnselectNode()
        {
            m_SelectedNode = null;
        }
        
        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }
            
            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }

        public bool TryOccupyNode(Vector2Int coordinate, bool occupy) // returns success of the operation 
        {
            Node node = GetNode(coordinate.x, coordinate.y);
            return TryOccupyNode(node, occupy);
        }
        
        public bool TryOccupyNode(Node node, bool occupy) // returns success of the operation 
        {
            if (!occupy || m_Pathfinding.CanOccupy(node))
            {
                if (!node.IsOccupied)
                {
                    node.OccupationAvailability = OccupationAvailability.CanNotOccupy;
                }
                else
                {
                    node.OccupationAvailability = OccupationAvailability.CanOccupy;
                    // clear cache in Path
                    m_Pathfinding.clearPathCache();
                }
                node.IsOccupied = !node.IsOccupied;
                return true;
            }
            return false;
        }

        public Node GetNodeAtPoint(Vector3 point)
        {
            Vector3 difference = point - m_Offset;

            int x = (int) (difference.x / m_NodeSize);
            int y = (int) (difference.z / m_NodeSize);

            return GetNode(x, y);
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            float sqrRadius = radius * radius;
            List<Node> reachableNodes = new List<Node>();
            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    if (IsInCircle(m_Nodes[i, j], point, sqrRadius))
                    {
                        reachableNodes.Add(m_Nodes[i, j]);
                    }
                }
            }

            return reachableNodes;
        }

        private bool IsInCircle(Node node, Vector3 point, float sqrRadius)
        {
            float xDist = Math.Abs(point.x - node.Position.x) - m_NodeSize * 0.5f;
            float zDist = Math.Abs(point.z - node.Position.z) - m_NodeSize * 0.5f;
            
            if (xDist < 0) { xDist = 0; } // if point coordinate is in node
            if (zDist < 0) { zDist = 0; }
            
            return (xDist * xDist + zDist * zDist) < sqrRadius;
        }
    }
}