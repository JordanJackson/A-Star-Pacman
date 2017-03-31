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

    const int halfMaxValue = int.MaxValue / 2;

    public Node cameFrom;

    public Node()
    {
        gScore = halfMaxValue;
        hScore = halfMaxValue;
        cameFrom = null;
    }

    public Node(int x, int y, Node cameFrom)
    {
        this.x = x;
        this.y = y;
        gScore = halfMaxValue;
        hScore = halfMaxValue;
        this.cameFrom = cameFrom;
        if (this.cameFrom != null)
        {
            gScore = cameFrom.gScore + 1;
        }
    }

    public Node(Vector3 pos)
    {
        x = (int)pos.x;
        y = -((int)pos.y);
        gScore = halfMaxValue;
        hScore = halfMaxValue;
        cameFrom = null;
    }

    public Node(Vector3 pos, Node cameFrom)
    {
        x = (int)pos.x;
        y = -((int)pos.y);
        gScore = halfMaxValue;
        hScore = halfMaxValue;
        this.cameFrom = cameFrom;
        if (this.cameFrom != null)
        {
            gScore = cameFrom.gScore + 1;
        }
    }

    public int FScore
    {
        get
        {
            return gScore + hScore;
        }
    }

    public int HScore
    {
        get
        {
            return gScore + fScore;
        }
    }

    public int CalculateHScore(Vector3 targetPos)
    {
        int distX = Mathf.Abs(x - (int)targetPos.x);
        int distY = Mathf.Abs(y - -((int)targetPos.y));

        hScore = distX + distY;
        return distX + distY;
    }

    public int CalculateHScore(Node targetNode)
    {
        int distX = Mathf.Abs(x - targetNode.x);
        int distY = Mathf.Abs(y - targetNode.y);

        hScore = distX + distY;
        return distX + distY;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, 0.0f);
    }

    public bool Equals(Node other)
    {
        return (x == other.x && y == other.y);
    }
         
}
