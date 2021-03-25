using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;

        public FlowFieldPathfinding(Grid grid, Vector2Int start, Vector2Int target)
        {
            m_Grid = grid;
            m_Start = start;
            m_Target = target;
        }

        public void UpdateField()
        {
            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);
            Node currentNodePath = m_Grid.GetNode(m_Start);

            // clear previous path if it was
            if (startNode.NextNode != null)
            {
                while (currentNodePath != targetNode)
                {
                    // nodes that cannot be occupied will remain so( we only clear them when we release a node - void clearPathCache() )
                    if (currentNodePath.OccupationAvailability == OccupationAvailability.Undefined)
                    {
                        currentNodePath.OccupationAvailability = OccupationAvailability.CanOccupy;
                    }
                    currentNodePath = currentNodePath.NextNode;
                }

                startNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            }

            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.coordinate);
                    float weightToTarget = currentNode.PathWeight + neighbour.weight;
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour.coordinate);
                    }
                }
            }
            
            // update new path
            currentNodePath = m_Grid.GetNode(m_Start);
            while (currentNodePath != targetNode)
            {
                if (currentNodePath.OccupationAvailability != OccupationAvailability.CanNotOccupy)
                {
                    currentNodePath.OccupationAvailability = OccupationAvailability.Undefined;
                }
                currentNodePath = currentNodePath.NextNode;
            }

            startNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            List<Vector2Int> nonDiagonalDirs = new List<Vector2Int>();
            List<Vector2Int> diagonalDirs = new List<Vector2Int>();
            nonDiagonalDirs.Add(Vector2Int.right);
            nonDiagonalDirs.Add(Vector2Int.up);
            nonDiagonalDirs.Add(Vector2Int.left);
            nonDiagonalDirs.Add(Vector2Int.down);
            diagonalDirs.Add(new Vector2Int(-1, -1));
            diagonalDirs.Add(new Vector2Int(-1, 1));
            diagonalDirs.Add(new Vector2Int(1, -1));
            diagonalDirs.Add(new Vector2Int(1, 1));

            foreach (Vector2Int dir in nonDiagonalDirs) // check non diagonals
            {
                if (hasNode(coordinate, dir))
                {
                    yield return new Connection(coordinate + dir, dir.magnitude);
                }
                else // cannot go through diagonal
                {
                    if (dir.x == 0)
                    {
                        diagonalDirs.Remove(new Vector2Int(1, dir.y));
                        diagonalDirs.Remove(new Vector2Int(-1, dir.y));
                    }
                    else
                    {
                        diagonalDirs.Remove(new Vector2Int(dir.x, 1));
                        diagonalDirs.Remove(new Vector2Int(dir.x, -1));
                    }
                }
            }

            foreach (Vector2Int dir in diagonalDirs) // check diagonals
            {
                if (hasNode(coordinate, dir))
                {
                    yield return new Connection(coordinate + dir, dir.magnitude);
                }
            }
        }
        private bool hasNode(Vector2Int coordinate, Vector2Int dir)
        {
            int x = dir.x;
            int y = dir.y;
            Vector2Int newCoordinate = coordinate + new Vector2Int(x, y);
            if (x == -1 && newCoordinate.x < 0)
            {
                return false;
            }
            if (x == 1 && newCoordinate.x >= m_Grid.Width)
            {
                return false;
            }
            if (y == -1 && newCoordinate.y < 0)
            {
                return false;
            }
            if (y == 1 && newCoordinate.y >= m_Grid.Height)
            {
                return false;
            }

            return !m_Grid.GetNode(newCoordinate).IsOccupied;
        }

        public bool CanOccupy(Node node)
        {
            if (node.OccupationAvailability == OccupationAvailability.CanOccupy)
            {
                return true;
            }
            
            if (node.OccupationAvailability == OccupationAvailability.CanNotOccupy)
            {
                return false;
            }

            // bfs for searching the target
            node.IsOccupied = true;
            List<Vector2Int> visited = new List<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Start);
            visited.Add(m_Start);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    if (neighbour.coordinate == m_Target) // reached the target
                    {
                        node.IsOccupied = false;
                        node.OccupationAvailability = OccupationAvailability.CanNotOccupy;
                        return true;
                    }
                    if (!visited.Contains(neighbour.coordinate)) // add only not visited nodes
                    {
                        visited.Add(neighbour.coordinate);
                        queue.Enqueue(neighbour.coordinate);
                    }
                }
            }
            
            // didn't reach the target
            node.IsOccupied = false;
            node.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            return false;
        }

        public void clearPathCache()
        {
            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);
            Node currentNodePath = m_Grid.GetNode(m_Start);
            
            while (currentNodePath != targetNode)
            {
                currentNodePath.OccupationAvailability = OccupationAvailability.CanOccupy;
                currentNodePath = currentNodePath.NextNode;
            }

            startNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
        }
    }
    public class Connection
    {
        public Vector2Int coordinate;
        public float weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            this.coordinate = coordinate;
            this.weight = weight;
        }
    }
}