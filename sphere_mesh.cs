using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sphere_mesh : MonoBehaviour
{
    [Header("Sphere Properties:")]
    [SerializeField] private int long_sect;
    [SerializeField] private int lat_sect;
    [SerializeField] private float radius;
    [Space(20)]
    [Header("Noise Properties:")]
    [SerializeField] private float noise_scale; // scale of 2d perlin noise
    [SerializeField] private float noise_intensity; // multiplier of noise along normals
    [SerializeField] private float noise_random; // random offset of each vertice along normals
    [Space(20)]
    [Header("Material Properties:")]
    [SerializeField] private Material sphere_material;

    private int tex_width = 256;
    private int tex_height = 256;
    private float tex_scale = 20f;

    void Start()
    {
        // Sets up game object with mesh filter and renderer
        this.AddComponent<MeshFilter>();
        this.AddComponent<MeshRenderer>();
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();

        // Creates mesh, applies perlin noise in direction of normals and corrects normals
        mesh_filter.mesh = GenerateSphereMesh(long_sect, lat_sect, radius);
        mesh_filter.mesh = AddPerlinNoiseToSphere(mesh_filter.mesh, noise_scale, noise_intensity, noise_random);
        mesh_filter.mesh = FlipNormals(mesh_filter.mesh);

        // Adds material
        Material mat = mesh_renderer.material;
        mat.mainTexture = GenerateTexture();
        mesh_renderer.material = mat;
    }
    private Mesh GenerateSphereMesh(int longitude, int latitude, float radius)
    {
        Mesh mesh = new Mesh();

        int latSegments = latitude; // Number of latitude subdivisions
        int lonSegments = longitude; // Number of longitude subdivisions

        int numVertices = (latSegments + 1) * (lonSegments + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangles = new int[latSegments * lonSegments * 6];

        float pi = Mathf.PI;
        float twoPi = pi * 2.0f;

        // Generate vertices, normals, and UVs
        int vertIndex = 0;
        for (int lat = 0; lat <= latSegments; lat++)
        {
            float a1 = pi * lat / latSegments;
            float sinA1 = Mathf.Sin(a1);
            float cosA1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= lonSegments; lon++)
            {
                float a2 = twoPi * lon / lonSegments;
                float sinA2 = Mathf.Sin(a2);
                float cosA2 = Mathf.Cos(a2);

                Vector3 vertex = new Vector3(sinA1 * cosA2, cosA1, sinA1 * sinA2) * radius;
                vertices[vertIndex] = vertex;
                normals[vertIndex] = vertex.normalized;
                uv[vertIndex] = new Vector2((float)lon / lonSegments, (float)lat / latSegments);

                vertIndex++;
            }
        }

        // Generate triangles
        int triIndex = 0;
        for (int lat = 0; lat < latSegments; lat++)
        {
            for (int lon = 0; lon < lonSegments; lon++)
            {
                int current = lat * (lonSegments + 1) + lon;
                int next = current + lonSegments + 1;

                // First triangle
                triangles[triIndex++] = current;
                triangles[triIndex++] = next;
                triangles[triIndex++] = current + 1;

                // Second triangle
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;
                triangles[triIndex++] = next + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }
    private Mesh AddPerlinNoiseToSphere(Mesh sphereMesh, float noiseScale, float displacement, float random)
    {
        Vector3[] vertices = sphereMesh.vertices;
        Vector3[] normals = sphereMesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Get vertex position in world space
            Vector3 vertex = vertices[i];
            Vector3 normal = normals[i];

            // Calculate Perlin noise value based on the vertex's position
            float noise;
            if (vertices[i].z > 0.01f || vertices[i].z < -0.01f)
            {
                noise = Mathf.PerlinNoise(vertex.x * (noiseScale + Random.Range(-random, random)), vertex.z * noiseScale + Random.Range(-random, random));
            }
            else
            {
                noise = Mathf.PerlinNoise(vertex.x * noiseScale, vertex.z * noiseScale);
            }
            // Apply the noise as displacement along the vertex normal
            vertices[i] += normal * noise * displacement;
        }

        // Update the mesh with the modified vertices
        sphereMesh.vertices = vertices;
        sphereMesh.RecalculateNormals();  // Recalculate normals after modifying vertices
        sphereMesh.RecalculateBounds();   // Recalculate bounds to avoid issues with mesh rendering

        return sphereMesh;
    }
    private Mesh FlipNormals(Mesh mesh)
    {
        // Get the current mesh data
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        // Flip the normals by negating each one
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        // Reverse the triangle winding order to match the flipped normals
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 1];
            triangles[i + 1] = temp;
        }

        // Assign the flipped normals and triangles back to the mesh
        mesh.normals = normals;
        mesh.triangles = triangles;

        return mesh;
    }
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(tex_width, tex_height);

        for (int y = 0; y < tex_height; y++)
        {
            for (int x = 0; x < tex_width; x++)
            {
                float xCoord = (float)x / tex_width * tex_scale;
                float yCoord = (float)y / tex_height * tex_scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }
}


