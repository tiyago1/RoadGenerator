using UnityEngine;

public class CurveProperties 
{
    public Vector3 StartPoint { get; set; }
    public Vector3 ControlStartPoint { get; set; }
    public Vector3 EndPoint { get; set; }
    public Vector3 ControlEndPoint { get; set; }
    public int Segment { get; set; }

    public CurveProperties() { }

    public CurveProperties(Vector3 startPoint, Vector3 controlStartPoint, Vector3 endPoint, Vector3 controlEndPoint, int segment)
    {
        StartPoint = startPoint;
        ControlStartPoint = controlStartPoint;
        EndPoint = endPoint;
        ControlEndPoint = controlEndPoint;
        Segment = segment;
    }
}
