using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadGenerator : MonoBehaviour
{
    #region Curve

    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform ControlStartPoint;
    [SerializeField] private Transform EndPoint;
    [SerializeField] private Transform ControlEndPoint;

    [Range(2, 99)] [SerializeField] private int segmentCount = 2;

    [SerializeField] private List<Vector3> points;

    #endregion

    [SerializeField] private float weight;
    [SerializeField] private MeshFilter filter;

    private List<Vector3> vertices;
    private List<int> triangles;

    [SerializeField] private List<Vector3> leftSideVertices;
    [SerializeField] private List<Vector3> rightSideVertices;
    [SerializeField] private List<int> leftSideTriangles;
    [SerializeField] private List<int> rightSideTriangles;

    [SerializeField] private TextMesh textPrefab;

    public void Awake()
    {
        points = new List<Vector3>();
        //GenerateText();
    }

    private void GeneratePointsText()
    {
        int half = leftSideVertices.Count / 2;

        for (int i = 0; i < leftSideVertices.Count; i++)
        {
            TextMesh verticeText = Instantiate(textPrefab, leftSideVertices[i], Quaternion.identity);
            verticeText.transform.localEulerAngles = new Vector3(90, 0, 0);
            verticeText.text = i.ToString();
            if (i >= half)
            {
                verticeText.color = Color.red;
            }
        }
    }

    private void SetupCurvePoints()
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

    private void Update()
    {
        SetupCurvePoints();
        GenerateRoad();
    }

    private void GenerateRoad()
    {
        Mesh mesh = filter.mesh;
        float halfWeight = weight * .5f;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        #region Left Side

        leftSideVertices = new List<Vector3>()
            {
                points[0],
                new Vector3((-halfWeight + points[0].x), points[0].y, points[0].z),
                new Vector3((-halfWeight + points[1].x), points[1].y, points[1].z),
                points[1],

                new Vector3((-halfWeight + points[2].x), points[2].y, points[2].z),
                points[2],
            };

        leftSideTriangles = new List<int>()
            {
                0,1,2
                ,2,3,0

                ,3,2,4
                ,4,5,3
            };

        for (int i = 3; i < points.Count; i++)
        {
            List<Vector3> newVertices = new List<Vector3>()
                {
                     new Vector3((points[i].x -halfWeight), points[i].y, points[i].z),
                     points[i],
                };

            List<int> lastTri = leftSideTriangles.Skip(leftSideTriangles.Count - 6).Take(6).ToList();

            List<int> newTriangles = new List<int>()
                {
                    lastTri[0] + 2,lastTri[1] + 2,lastTri[2] + 2
                    ,lastTri[3] + 2,lastTri[4] + 2,lastTri[5] + 2
                };

            leftSideTriangles.AddRange(newTriangles);
            leftSideVertices.AddRange(newVertices);
        }

        vertices.AddRange(leftSideVertices);
        triangles.AddRange(leftSideTriangles);

        #endregion

        #region Right Side
        
        rightSideVertices = new List<Vector3>();
        rightSideVertices.AddRange(leftSideVertices);
        

        for (int i = 0; i < rightSideVertices.Count; i++)
        {
            Vector3 newVertice = rightSideVertices[i];
            newVertice.x += halfWeight;
            rightSideVertices[i] = newVertice;
        }

        Vector3 firstVertice = rightSideVertices[0];
        rightSideVertices[0] = rightSideVertices[1];
        rightSideVertices[1] = firstVertice;

        rightSideTriangles = new List<int>();
        List<int> lastTrix = leftSideTriangles.Skip(leftSideTriangles.Count - 6).Take(6).ToList();
        List<int> newTrianglesx = new List<int>()
        {
            lastTrix[0] +  4,lastTrix[1] + 4,lastTrix[2] + 4
            ,lastTrix[3] + 4,lastTrix[4] + 4,lastTrix[5] + 4
        };

        rightSideTriangles.AddRange(newTrianglesx);

        for (int i = 6; i < leftSideTriangles.Count; i += 6)
        {
            lastTrix = rightSideTriangles.Skip(rightSideTriangles.Count - 6).Take(6).ToList();
            newTrianglesx = new List<int>()
                {
                    lastTrix[0] +  2,lastTrix[1] + 2,lastTrix[2] + 2
                    ,lastTrix[3] + 2,lastTrix[4] + 2,lastTrix[5] + 2
                };
            rightSideTriangles.AddRange(newTrianglesx);
        }

        vertices.AddRange(rightSideVertices);
        triangles.AddRange(rightSideTriangles);

        #endregion

        Vector2[] uvs = new Vector2[vertices.Count];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(Mathf.Abs(vertices[i].x), Mathf.Abs(vertices[i].z));
        }

        mesh.SetVertices(vertices);
        mesh.triangles = triangles.ToArray();
        mesh.SetUVs(0, uvs.ToArray());
        filter.mesh = mesh;
    }
}
