using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunic
{
    public class Terrain : MonoBehaviour
    {
        private Mesh mesh;

        [SerializeField]
        private int size = 100;

        [SerializeField]
        private int cubeSize = 1;

        [SerializeField]
        private float minNoise;

        [SerializeField]
        private float maxNoise;

        [SerializeField]
        private float minModifier;

        [SerializeField]
        private float maxModifier;

        int[,] heights;

        List<Vector3> vertices;
        List<int> triangles;
        List<Color32> colors;

        [SerializeField]
        Gradient gradient;

        private void Start()
        {
            heights = new int[size, size];

            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color32>();

            mesh = new Mesh();
            mesh.name = "Terrain";

            GetComponent<MeshFilter>().mesh = mesh;

            Generate();
        }

        [ContextMenu("Regenerate")]
        private void Regenerate()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            Generate();
        }

        private int getHeight(int x, int z)
        {
            if (x >= size || x < 0 || z >= size || z < 0)
                return -1;

            return heights[x, z];
        }

        private Color32 colorByHeight(int height)
        {
            return gradient.Evaluate(height / 15f);
        }

        private void Generate()
        {
            void addTriangles(int index)
            {
                triangles.Add(index + 1);
                triangles.Add(index);
                triangles.Add(index + 2);

                triangles.Add(index + 1);
                triangles.Add(index + 2);
                triangles.Add(index + 3);
            }

            float scale = Random.Range(minNoise, maxNoise);
            float scaleModifier = Random.Range(minModifier, maxModifier);

            float half = size / 2f;

            for (int x = 0; x < size; ++x)
            {
                for (int z = 0; z < size; ++z)
                {
                    float delta = Random.Range(scale / 10, scale / 9);

                    float noise = Mathf.PerlinNoise(x / (scale + delta), z / (scale + delta));

                    float xScore = Mathf.Abs(half - x);
                    float zScore = Mathf.Abs(half - z);

                    float heightModifier = 1f - Mathf.Max(xScore, zScore) / half;

                    int height = Mathf.RoundToInt(noise * scaleModifier * heightModifier);

                    heights[x, z] = height;

                    vertices.Add(new Vector3(x, height, z));
                    vertices.Add(new Vector3(x + cubeSize, height, z));
                    vertices.Add(new Vector3(x, height, z + cubeSize));
                    vertices.Add(new Vector3(x + cubeSize, height, z + cubeSize));

                    colors.Add(colorByHeight(height));
                    colors.Add(colorByHeight(height));
                    colors.Add(colorByHeight(height));
                    colors.Add(colorByHeight(height));
                }
            }

            for (int x = 0; x < size; ++x)
            {
                int line = x * size;

                for (int z = 0; z < size; ++z)
                {
                    int index = (line + z) * 4;

                    triangles.Add(index);
                    triangles.Add(index + 3);
                    triangles.Add(index + 1);

                    triangles.Add(index);
                    triangles.Add(index + 2);
                    triangles.Add(index + 3);

                    int height = getHeight(x, z);

                    int west = getHeight(x - 1, z);

                    if (west >= 0 || west > height)
                    {
                        int otherIndex = ((x - 1) * size + z) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index]);
                        vertices.Add(vertices[index + 2]);
                        vertices.Add(vertices[otherIndex + 1]);
                        vertices.Add(vertices[otherIndex + 3]);

                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(west));
                        colors.Add(colorByHeight(west));

                        addTriangles(current);
                    }

                    int north = getHeight(x, z + 1);

                    if (north >= 0 || north > height)
                    {
                        int otherIndex = (x * size + z + 1) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 2]);
                        vertices.Add(vertices[index + 3]);
                        vertices.Add(vertices[otherIndex]);
                        vertices.Add(vertices[otherIndex + 1]);

                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(north));
                        colors.Add(colorByHeight(north));

                        addTriangles(current);
                    }

                    int east = getHeight(x + 1, z);

                    if (east >= 0 && east > height)
                    {
                        int otherIndex = ((x + 1) * size + z) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 3]);
                        vertices.Add(vertices[index + 1]);
                        vertices.Add(vertices[otherIndex + 2]);
                        vertices.Add(vertices[otherIndex]);

                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(east));
                        colors.Add(colorByHeight(east));

                        addTriangles(current);
                    }

                    int south = getHeight(x, z - 1);

                    if (south >= 0 && south > height)
                    {
                        int otherIndex = (x * size + z - 1) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 1]);
                        vertices.Add(vertices[index]);
                        vertices.Add(vertices[otherIndex + 3]);
                        vertices.Add(vertices[otherIndex + 2]);

                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(height));
                        colors.Add(colorByHeight(south));
                        colors.Add(colorByHeight(south));

                        addTriangles(current);
                    }
                }
            }

            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            mesh.Clear();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.colors32 = colors.ToArray();

            mesh.RecalculateNormals();
        }
    }
}
