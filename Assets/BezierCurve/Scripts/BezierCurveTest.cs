using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveTest : MonoBehaviour
{
    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform ControlStartPoint;
    [SerializeField] private Transform EndPoint;
    [SerializeField] private Transform ControlEndPoint;

    [Range(2,99)] [SerializeField] private int segmentCount = 2;

    [SerializeField] private List<Vector3> points;

    public void Awake()
    {
        points = new List<Vector3>();

        //BezierCurve.Test();

        //SetupPoints();
        //for (int i = 2; i < 100; i++)
        //{
        //    segmentCount = i;
        //    SetupPoints();
        //}

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetupPoints();
        }
    }

    private void SetupPoints()
    {
        points = BezierCurve.CalculateCurve(StartPoint.position, ControlStartPoint.position, EndPoint.position, ControlEndPoint.position, segmentCount);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(points[i], 0.1f);
        }
    }
}
