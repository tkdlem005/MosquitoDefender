using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private WorldGridManager _gridManager;

    public PathFinding(WorldGridManager gridManager)
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
                return BuildPath(path, current);

            closedSet.Add(current.Position);

            foreach (Vector2Int neighbor in GetNeighbors(current.Position))
            {
                if (closedSet.Contains(neighbor) || !_gridManager.IsWalkable(neighbor))
                    continue;

                TryAddNeighborNode(neighbor, current, end, openSet);
            }
        }

        return path;
    }

    private List<Vector2Int> BuildPath(List<Vector2Int> path, Node current)
    {
        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }

    private void TryAddNeighborNode(Vector2Int neighbor, Node current, Vector2Int end, PriorityQueue<Node> openSet)
    {
        float nextG = current.G + 1;
        float h = GetHeuristic(neighbor, end);

        Node neighborNode = new Node(neighbor, nextG, h, current);

        if (openSet.Contains(neighborNode))
        {
            Node existing = openSet.Find(n => n.Equals(neighborNode));
            if (nextG >= existing.G)
                return;
        }

        openSet.Enqueue(neighborNode);
    }

    // 맨해튼 거리
    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

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

        public Node(Vector2Int posistion, float g, float h, Node parent)
        {
            Position = posistion;
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
            int childIndex = data.Count - 1;

            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;

                if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
                {
                    break;
                }

                T temp = data[childIndex];
                data[childIndex] = data[parentIndex];
                data[parentIndex] = temp;

                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            int lastIndex = data.Count - 1;
            T frontItem = data[0];

            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);
            --lastIndex;

            int parentIndex = 0;

            while (true)
            {
                // 왼쪽 자식
                int childIndex = parentIndex * 2 + 1;

                if (childIndex > lastIndex) 
                    break;

                int rightChild = childIndex + 1;

                if (rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)
                {
                    childIndex = rightChild;
                }

                if (data[parentIndex].CompareTo(data[childIndex]) <= 0) 
                    break;

                T temp = data[childIndex];
                data[childIndex] = data[parentIndex];
                data[parentIndex] = temp;
                
                parentIndex = childIndex;
            }
            return frontItem;
        }

        public bool Contains(T item) => data.Contains(item);

        public T Find(System.Predicate<T> match) => data.Find(match);
    }
}
