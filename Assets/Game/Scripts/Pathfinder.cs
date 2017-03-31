using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathfinding;

public class Pathfinder : Singleton<Pathfinder> 
{

    public int breakCount = 5000;

    public CollisionMap collisionMap;

    public Direction[] dirMap =
    {
        new Direction(-1, 0),           // left
        new Direction(0, -1),           // down
        new Direction(1, 0),            // right
        new Direction(0, 1)             // up
    };

    public Vector3 generatePath(Vector3 _position, Vector3 _target, ref List<Vector3> _path)
    {
        int count = 0;
        if (collisionMap.CollisionTable == null)
            return Vector3.zero;
        // if _target not walkable, re-compute to closer walkable node
        // if _start not walkable something bad happened, break and debug

        // create start node
        int startX = (int)_position.x;
        int startY = (int)-_position.y;
        Node startNode = new Node(!collisionMap.CheckCollision(startX, startY), startX, startY);
        if (!startNode.walkable)
        {
            Debug.Log("Unwalkable start node.");
            return Vector3.zero;
        }
        // create target node
        int targetX = (int)_target.x;
        int targetY = (int)-_target.y;
        Node targetNode = new Node(!collisionMap.CheckCollision(targetX, targetY), targetX, targetY);
        // if target not walkable, consider moving target to closest walkable node

        Heap<Node> openSet = new Heap<Node>(collisionMap.MapWidth * collisionMap.MapHeight * 10);
        List<Node> closedSet = new List<Node>();
        openSet.Add(startNode);

        // main loop
        while (openSet.Count > 0)
        {
            count++;
            if (count >= breakCount)
            {
                return Vector3.zero;
            }
            // get node in openSet with lowest fCost
            Node current = openSet.RemoveFirst();
            closedSet.Add(current);

            // check if we found target Node
            if (current.Equals(targetNode))
            {
                Debug.Log("Path found: " + new Vector3(current.x, current.y, 0.0f));
                _path = ReconstructPath(current);
                return new Vector3(current.x, current.y, 0.0f);
            }

            for (int i = 0; i < dirMap.Length; i++)
            {
                // create new Nodes based on dir and check for collision
                int neighX = current.x + dirMap[i].x;
                int neighY = current.y + dirMap[i].y;

                Node neighbour = new Node(!collisionMap.CheckCollision(neighX, neighY), neighX, neighY);

                if (!neighbour.walkable)
                {
                    continue;
                }

                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int tempGScore = current.gCost + 1;
                if (tempGScore < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = tempGScore;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }


        // failed
        Debug.Log("No path found");
        return Vector3.zero;
    }

    private List<Vector3> ReconstructPath(Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(new Vector3(endNode.x, endNode.y, 0.0f));
        Node current = endNode.parent;
        while (current != null)
        {
            path.Add(new Vector3(current.x, current.y, 0.0f));
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        return dstX + dstY;
    }

    public class Direction
    {
        public int x;
        public int y;

        public Direction(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < collisionMap.MapWidth; i++)
        {
            for (int j = 0; j < collisionMap.MapHeight; j++)
            {
                if (collisionMap.CheckCollision(i,j))
                {
                    
                    Gizmos.DrawWireCube(new Vector3(i, j * -1, 0.0f), Vector3.one);
                }
            }
        }
    }
}

