﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    public static List<Vector3> CalculateCurve(Vector3 startPoint, Vector3 controlStartPoint, Vector3 endPoint, Vector3 controlEndPoint, int segment)
    {
        List<Vector3> points = new List<Vector3>();

        float t = 0;
        float percentageT;
        float finalT;
        CalculateTValues(segment, out percentageT, out finalT);

        Vector3 p0 = startPoint;
        Vector3 p1 = controlStartPoint;
        Vector3 p2 = controlEndPoint;
        Vector3 p3 = endPoint;
   
        int counter = 0;
        List<float> segments = FindSegmentPoints(segment);

        while (t < 1.5f)
        {
            Vector3 a = Vector3.Lerp(p0, p1, t);
            Vector3 b = Vector3.Lerp(p1, p2, t);
            Vector3 c = Vector3.Lerp(p2, p3, t);

            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);

            Vector3 point = Vector3.Lerp(d, e, t);

            if (counter < segments.Count)
            {
                if (Mathf.Approximately(t, segments[counter]))
                {
                    points.Add(point);
                    counter++;
                }
            }

            t += 0.01f;
        }

        return points;
    }

    private static void CalculateTValues(int segmentCount, out float percentageT, out float finalT)
    {
        double b = 0.01;
        percentageT = (float)((100 / segmentCount) * b);
        finalT = percentageT * segmentCount;
    }

    public static List<float> FindSegmentPoints(int segment)
    {
        List<float> result = new List<float>();
        int percentage = 100 / segment;
        int counter = 0;
        for (int x = 0; x <= 100; x += percentage)
        {
            result.Add(x * 0.01f);
            counter++;
        }

        return result;
    }

    public static void Test()
    {
        string result = String.Empty;
        for (int i = 4; i < 100; i++)
        {
            int percentage = 100 / i;

            for (int x = 0; x <= 100; x += percentage)
            {
                result += x + "-";
            }

            result = String.Empty;
        }
    }
}