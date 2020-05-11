using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetExplode : MonoBehaviour
{
    public Planet planet;
    public float force = 20f;
    public float radius = 5f;
    public bool startExplosion;

    private bool started = false;
    private Mesh[] facePieces = new Mesh[6];
    private MeshFilter[] meshFilters = new MeshFilter[6];
    private GameObject[] planetPieces = new GameObject[6];
    private float duration = 0;

    private void Update()
    {
        if (startExplosion && !started)
        {
            DivideByFaces(planet.GetFaces());
            started = true;
        }
    }

    private void DivideByFaces(TerrainFace[] terrainFaces)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3[] vertices = new Vector3[terrainFaces[i].mesh.vertices.Length + 1];
            int pos = 0;
            foreach(var vertex in terrainFaces[i].mesh.vertices)
            {
                vertices[pos] = vertex;
                pos++;
            }
            vertices[vertices.Length - 1] = planet.transform.position;

            int[] triangles = new int[terrainFaces[i].mesh.triangles.Length + (4*3)];
            pos = 0;
            foreach(int vertexIndex in terrainFaces[i].mesh.triangles)
            {
                triangles[pos] = vertexIndex;
                pos++;
            }
            triangles[pos] = terrainFaces[i].corners[1];
            triangles[pos + 1] = terrainFaces[i].corners[0];
            triangles[pos + 2] = vertices.Length - 1; // center 

            triangles[pos + 3] = terrainFaces[i].corners[2];
            triangles[pos + 4] = terrainFaces[i].corners[1];
            triangles[pos + 5] = vertices.Length - 1; // center 

            triangles[pos + 6] = terrainFaces[i].corners[3];
            triangles[pos + 7] = terrainFaces[i].corners[2];
            triangles[pos + 8] = vertices.Length - 1; // center 

            triangles[pos + 9] = terrainFaces[i].corners[0];
            triangles[pos + 10] = terrainFaces[i].corners[3];
            triangles[pos + 11] = vertices.Length - 1; // center 

            GameObject planetPiece = new GameObject("planetPiece");
            planetPiece.transform.parent = transform;
            planetPiece.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilters[i] = planetPiece.AddComponent<MeshFilter>();
            meshFilters[i].sharedMesh = new Mesh();
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planet.colorSettings.planetMaterial;
            
            facePieces[i] = meshFilters[i].sharedMesh;
            facePieces[i].Clear();
            facePieces[i].vertices = vertices;
            facePieces[i].triangles = triangles;
            facePieces[i].RecalculateNormals();

            planetPieces[i] = planetPiece;

        }

        planet.gameObject.SetActive(false);
        for (int i=0;i<6;i++)
        {

            //planetPiece.AddComponent<MeshCollider>().
            //MeshCollider collider = planetPieces[i].AddComponent<MeshCollider>();
            //collider.convex = true;
            //collider.cookingOptions = MeshColliderCookingOptions.WeldColocatedVertices;
            //collider.sharedMesh = facePieces[i];
            //collider.GetComponent<Mesh>().vertices = facePieces[i].vertices;
            Rigidbody rb = planetPieces[i].AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0.2f;
            rb.AddForce(terrainFaces[i].localUp.normalized * force);
            //rb.AddExplosionForce(force, planet.transform.position, radius);
        }
    }
}
