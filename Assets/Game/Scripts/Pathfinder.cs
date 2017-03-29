using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathfinding;

public class Pathfinder : Singleton<Pathfinder> 
{
    public CollisionMap collisionMap;

    public Direction[] dirMap =
    {
        new Direction(-1, 0),           // left
        new Direction(0, -1),           // down
        new Direction(1, 0),            // right
        new Direction(0, 1)             // up
    };

    private void Start()
    {
        if (collisionMap.CollisionTable == null)
        {
            Debug.Log("Null Collision Table");
        }
        Debug.Log("Collision Check: " + collisionMap.CheckCollision(0, 1));
    }

    private void Update()
    {
        if (collisionMap.CollisionTable == null)
        {
            Debug.Log("Null Collision Table in Update.");
        }
        else
        {
            Debug.Log("Collision Table no longer null.");
            Debug.Log("Collision Check: " + collisionMap.CheckCollision(0, 1));
        }
    }

    public Vector3 generatePath(Vector3 _position, Vector3 _target, List<Vector3> _path)
    {
        if (collisionMap.CollisionTable == null)
            return Vector3.zero;
        // if _target not walkable, re-compute to closer walkable node
        // if _start not walkable something bad happened, break and debug

        // create start node and initialize f and g costs
        Node startNode = new Node(_position);
        Node targetNode = new Node(_target);
        startNode.gScore = 0;
        startNode.CalculateFScore(_target);

        if (collisionMap.CheckCollision(startNode.x, startNode.y))
        {
            Debug.Log("Unwalkable start node at " + startNode.x + ", " + startNode.y +  ", Ghost out of position.");
            return Vector3.zero;
        }

        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();


        // add start to open set
        openSet.Add(startNode);
        _path.Add(_position);

        // main loop
        while (openSet.Count > 0)
        {
            // get node in openSet with lowest fCost
            Node current = FindLowestInList(openSet);

            if (current.Equals(targetNode))
            {
                // calculate path and return
                _path = ReconstructPath(current);
                return current.ToVector3();
            }

            openSet.Remove(current);
            closedSet.Add(current);

            for (int i = 0; i < dirMap.Length; i++)
            {
                // create new Nodes based on dir and check for collision
            }

            break;
        }



        return Vector3.zero;
    }

    private Node FindLowestInList(List<Node> nodeList)
    {
        int currentIndex = 0;
        int currentBest = int.MaxValue;

        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].FScore < currentBest)
            {
                currentIndex = i;
                currentBest = nodeList[i].FScore;
            }
        }

        return nodeList[currentIndex];
    }

    private List<Vector3> ReconstructPath(Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(endNode.ToVector3());
        Node current = endNode.cameFrom;
        while (current != null)
        {
            path.Add(current.ToVector3());
            current = current.cameFrom;
        }

        path.Reverse();
        return path;
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
}

