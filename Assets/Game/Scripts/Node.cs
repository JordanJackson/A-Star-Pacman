using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IEquatable<Node>
{
    public int x;
    public int y;

    public int gScore;
    private int fScore;
    private int hScore;

    public Node cameFrom;

    public Node()
    {
        gScore = int.MaxValue;
        fScore = int.MaxValue;
        cameFrom = null;
    }

    public Node(Vector3 pos)
    {
        x = (int)pos.x;
        y = -((int)pos.y + 1);
        gScore = int.MaxValue;
        fScore = int.MaxValue;
        cameFrom = null;
    }

    public Node(Vector3 pos, Node cameFrom)
    {
        x = (int)pos.x;
        y = -((int)pos.y + 1);
        gScore = int.MaxValue;
        fScore = int.MaxValue;
        this.cameFrom = cameFrom;
    }

    public int FScore
    {
        get
        {
            return fScore;
        }
    }

    public int HScore
    {
        get
        {
            return gScore + fScore;
        }
    }

    public void CalculateFScore(Vector3 targetPos)
    {
        int distX = Mathf.Abs(x - (int)targetPos.x);
        int distY = Mathf.Abs(y - -((int)targetPos.y + 1));

        fScore = distX + distY;
    }

    public void CalculateFScore(Node targetNode)
    {
        int distX = Mathf.Abs(x - targetNode.x);
        int distY = Mathf.Abs(y - targetNode.y);

        fScore = distX + distY;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, (-y) - 1.0f, 0.0f);
    }

    public bool Equals(Node other)
    {
        return (x == other.x && y == other.y);
    }
         
}
