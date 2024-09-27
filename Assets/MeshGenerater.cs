using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeshGenerater : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;

    public Vector2 Size;
    [Range(1, 50)]
    public int Resolution ;
    public bool update;
    public bool gizmos;

    List<Vector3> Vertices;
    List<int> Triangles;

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MakeVertices();
        MakeTriangles();
        AssignMesh();
    }
    private void Update()
    {
        if (update)
        {
            MakeVertices();
            MakeTriangles();
            AssignMesh();
        }
    }
    void MakeVertices()
    {
        Vertices = new List<Vector3>();
        float xStep = Size.x / Resolution;
        float yStep = Size.y / Resolution;

        for (int i = 0; i < Math.Pow(Resolution+1,2); i++)
        {
            Vertices.Add(new Vector3(i % (Resolution + 1) * xStep - Size.x / 2 , i / (Resolution + 1)  * yStep - Size.y / 2, 0));
        }
    }
    void MakeTriangles()
    {
        Triangles = new List<int>();
        for (int i = 0;i < Math.Pow(Resolution,2);i++)
        {
            int p = i % (Resolution ) + math.floor(i / (Resolution )).ConvertTo<int>() * (Resolution + 1);
            Triangles.Add(p);
            Triangles.Add(p + Resolution + 1);
            Triangles.Add(p + Resolution + 2);

            Triangles.Add(p + 1);   
            Triangles.Add(p);
            Triangles.Add(p + Resolution +2);

        }
    }
    void AssignMesh()
    {
        mesh.vertices = Vertices.ToArray();
        mesh.triangles = Triangles.ToArray();
    }
    private void OnDrawGizmos()
    {
        if (gizmos)
        {
            foreach (Vector3 v in Vertices)
            {
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
    }
}
