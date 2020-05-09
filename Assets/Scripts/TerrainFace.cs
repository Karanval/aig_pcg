using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    public Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    Vector3 origin;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp, Vector3 center)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        origin = center;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        //perpendicular
        axisB = Vector3.Cross(localUp, axisA);
    }
    //public void ConstructMesh()
    //{
    //    Vector3[] vertices = new Vector3[(1+resolution) * (1+resolution)];
    //    int[] triangles = new int[(resolution) * (resolution) * 6];
    //    int triIndex = 0;
    //    Vector2[] uv = (mesh.uv.Length == vertices.Length) ? mesh.uv : new Vector2[vertices.Length];
    //    int i = -1;
    //    // getting each vertex of triangles in mesh
    //    for (int y = 0; y < resolution + 1; y++)
    //    {
    //        for (int x = 0; x < resolution + 1; x++)
    //        {
    //            //same as i++ if i outside of for loops
    //            i++;

    //            Vector2 percent = new Vector2(x, y) / (resolution - 1);
    //            Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
    //            //same distance to the center so the cube becomes a sphere
    //            Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
    //            //vertices[i] = pointOnUnitCube;
    //            float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
    //            vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
    //            //x axis for biones
    //            uv[i].y = unscaledElevation;

    //            if (x != resolution && y != resolution)
    //            {
    //                triangles[triIndex] = i;
    //                triangles[triIndex + 1] = i + resolution + 1;
    //                triangles[triIndex + 2] = i + resolution;

    //                triangles[triIndex + 3] = i;
    //                triangles[triIndex + 4] = i + 1;
    //                triangles[triIndex + 5] = i + resolution + 1;
    //                triIndex += 6;
    //            }
    //        }
    //    }

    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.RecalculateNormals();
    //    //mesh.uv = uv;
    //}
    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (mesh.uv.Length== vertices.Length)? mesh.uv: new Vector2[vertices.Length];

        // getting each vertex of triangles in mesh
        for (int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < resolution; x++)
            {
                //same as i++ if i outside of for loops
                int i = x + y * resolution;

                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                //same distance to the center so the cube becomes a sphere
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //vertices[i] = pointOnUnitCube;
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < normals.Length;i++)
        {
            normals[i] = (vertices[i] - origin).normalized;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
       // mesh.RecalculateNormals();
        mesh.normals = normals;
        mesh.uv = uv;
    }

    // Separate from mesh generator so it is not slow to update colors
    public void ConstructUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                //uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                //x axis for biones
                uv[i].y = unscaledElevation;
                uv[i].x = shapeGenerator.GetScaledElevation(unscaledElevation);
            }
        }
        mesh.uv = uv;
    }
}
