using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private NavGridManager _gridManager;

    public Pathfinding(NavGridManager gridManager)
    {
        _gridManager = gridManager;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new();

        if (!_gridManager.IsWalkable(end))
            return path;

        HashSet<Vector2Int> closedSet = new();
        PriorityQueue<Node> openSet = new();

        Node startNode = new Node(start, 0, GetHeuristic(start, end), null);
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (current.Position == end)
            {
                while (current != null)
                {
                    path.Add(current.Position);
                    current = current.Parent;
                }
                path.Reverse();
                return path;
            }

            closedSet.Add(current.Position);

            foreach (Vector2Int neighbor in GetNeighbors(current.Position))
            {
                if (closedSet.Contains(neighbor) || !_gridManager.IsWalkable(neighbor))
                    continue;

                float tentativeG = current.G + 1;

                Node neighborNode = new Node(neighbor, tentativeG, GetHeuristic(neighbor, end), current);

                if (openSet.Contains(neighborNode))
                {
                    Node existing = openSet.Find(n => n.Equals(neighborNode));
                    if (tentativeG >= existing.G)
                        continue;
                }

                openSet.Enqueue(neighborNode);
            }
        }

        return path;
    }

    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        Vector2Int[] directions = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = pos + dir;
            yield return neighbor;
        }
    }

    private class Node : System.IEquatable<Node>, System.IComparable<Node>
    {
        public Vector2Int Position;
        public float G;
        public float H;
        public float F => G + H;
        public Node Parent;

        public Node(Vector2Int pos, float g, float h, Node parent)
        {
            Position = pos;
            G = g;
            H = h;
            Parent = parent;
        }

        public bool Equals(Node other) => other != null && Position.Equals(other.Position);

        public override bool Equals(object obj) => Equals(obj as Node);

        public override int GetHashCode() => Position.GetHashCode();

        public int CompareTo(Node other) => F.CompareTo(other.F);
    }

    private class PriorityQueue<T> where T : System.IComparable<T>
    {
        private List<T> data = new();

        public int Count => data.Count;

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0) break;
                (data[ci], data[pi]) = (data[pi], data[ci]);
                ci = pi;
            }
        }

        public T Dequeue()
        {
            int li = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);
            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li) break;
                int rc = ci + 1;
                if (rc <= li && data[rc].CompareTo(data[ci]) < 0) ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0) break;
                (data[pi], data[ci]) = (data[ci], data[pi]);
                pi = ci;
            }
            return frontItem;
        }

        public bool Contains(T item) => data.Contains(item);

        public T Find(System.Predicate<T> match) => data.Find(match);
    }
}
