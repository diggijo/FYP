using System;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    internal List<Point> points;
    internal Point point;
    public Line()
    {
        points = new List<Point>();
    }

    public void AddPoint(float trolleyPos, float hoistPos, DateTime dt, char mode)
    {
        point = new Point(trolleyPos, hoistPos, dt, mode);
        points.Add(point);
    }

    // Nested class to represent a point with its datetime
    public class Point
    {
        public float Trolley_Position { get; }
        public float Hoist_Position { get; }
        public DateTime DateTime { get; }

        public char Mode { get; }

        public Point(float trolleyPos, float hoistPos, DateTime dateTime, char mode)
        {
            Trolley_Position = trolleyPos;
            Hoist_Position = hoistPos;
            DateTime = dateTime;
            Mode = mode;
        }
    }
}
