using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    readonly ShapeGenerator shapeGenerator;
    public Mesh mesh;
    public Vector3[] pointsOnUnitSphere;
    public int[] corners;
    int resolution;
    public Vector3 localUp;
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
        pointsOnUnitSphere = new Vector3[resolution * resolution];
        corners = new int[4];
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

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
                pointsOnUnitSphere[i] = pointOnUnitSphere;
                //vertices[i] = pointOnUnitCube;
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                // 0 or vertex value
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
        corners[0] = 0;
        corners[1] = 0 + resolution - 1;
        corners[2] = vertices.Length - 1;
        corners[3] = vertices.Length - resolution;
        Vector3[] normals = new Vector3[vertices.Length];
        for (int i = 0; i < normals.Length;i++)
        {
            normals[i] = (vertices[i] - origin).normalized;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        //mesh.normals = normals;
    }

    // Separate from mesh generator so it is not slow to update colors
    public void ConstructUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = (mesh.uv.Length == resolution * resolution) ? mesh.uv : new Vector2[resolution * resolution];

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
                uv[i].x = 1;
            }
        }
        mesh.uv = uv;
    }

    public void ConstructUVsFromNoise(ColorGenerator colorGenerator, NoiseAnimator noiseAnimator)
    {
        Vector2[] uv = (mesh.uv.Length == resolution * resolution) ? mesh.uv : new Vector2[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;

                //uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointsOnUnitSphere[i]);
                //x axis for biones
                uv[i].y = unscaledElevation;
                if (noiseAnimator.settings.noiseAnimatorType == NoiseAnimator.NoiseAnimatorType.All)
                {
                    uv[i].x = noiseAnimator.Evaluate(pointsOnUnitSphere[i]);
                } 
                else if(noiseAnimator.settings.noiseAnimatorType == NoiseAnimator.NoiseAnimatorType.Ocean)
                {
                    if (unscaledElevation < 0)
                    {
                        uv[i].x = noiseAnimator.Evaluate(pointsOnUnitSphere[i]);
                    } else
                    {
                        uv[i].x = 1;
                    }
                }
                else if (noiseAnimator.settings.noiseAnimatorType == NoiseAnimator.NoiseAnimatorType.Terrain)
                {
                    if (unscaledElevation > 0)
                    {
                        uv[i].x = noiseAnimator.Evaluate(pointsOnUnitSphere[i]);
                    }
                    else
                    {
                        uv[i].x = 1;
                    }
                }
            }
        }
        mesh.uv = uv;
    }

}
