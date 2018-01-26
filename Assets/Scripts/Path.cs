﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{

    [SerializeField, HideInInspector]
    List<Vector2> points;

    public Path(Vector2 center)
    {
        points = new List<Vector2>()
        {
            center + Vector2.left,
            center + (Vector2.left + Vector2.up) * 0.5f,
            center + (Vector2.right + Vector2.down) * 0.5f,
            center + Vector2.right
        };
        
    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int numSegments
    {
        get
        {
            return (points.Count - 1) / 3;
        }
    }

    public int numPoints
    {
        get
        {
            return points.Count;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(2 * points[points.Count - 1] - points[points.Count - 2]);
        points.Add((anchorPos + points[points.Count - 1]) * 0.5f);
        points.Add(anchorPos);
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[]
        {
            points[i*3],
            points[i*3 + 1],
            points[i * 3 + 2],
            points[i*3+3]
        };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        if (i % 3 == 0)
        {
            if (i + 1 < points.Count)
            {
                points[i + 1] += deltaMove;
            }
            if (i - 1 >= 0)
            {
                points[i - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
            int anchorIndx = nextPointIsAnchor ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
            {
                float dst = (points[anchorIndx] - points[correspondingControlIndex]).magnitude;
                Vector2 dir = (points[anchorIndx] - points[i]).normalized;
                points[correspondingControlIndex] = points[anchorIndx] + dir * dst;
            }
        }

    }

}
