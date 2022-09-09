using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Tunic
{
    [RequireComponent(typeof(MeshFilter))]
    public class Terrain : MonoBehaviour
    {
        private Mesh _mesh;

        int _size;

        int[,] _heightByPosition;
        int[] _heightByIndex;

        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.name = "Terrain";

            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void Generate(int size, float minNoise = 5f, float maxNoise = 10f, float minModifier = 15f, float maxModifier = 25f)
        {
            _size = size;

            _heightByPosition = new int[size, size];

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            List<int> heightByIndex = new List<int>();

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

            float half = _size / 2f;

            for (int i = 0; i < _size; ++i)
            {
                for (int j = 0; j < _size; ++j)
                {
                    float x = -half + i;
                    float z = -half + j;

                    float delta = Random.Range(scale / 10, scale / 9);

                    float noise = Mathf.PerlinNoise(i / (scale + delta), j / (scale + delta));

                    float xScore = Mathf.Abs(half - i);
                    float zScore = Mathf.Abs(half - j);

                    float heightModifier = 1f - Mathf.Max(xScore, zScore) / half;

                    int height = Mathf.RoundToInt(noise * scaleModifier * heightModifier);

                    _heightByPosition[i, j] = height;

                    vertices.Add(new Vector3(x, height, z));
                    vertices.Add(new Vector3(x + 1f, height, z));
                    vertices.Add(new Vector3(x, height, z + 1f));
                    vertices.Add(new Vector3(x + 1f, height, z + 1f));

                    heightByIndex.Add(height);
                    heightByIndex.Add(height);
                    heightByIndex.Add(height);
                    heightByIndex.Add(height);
                }
            }

            for (int i = 0; i < _size; ++i)
            {
                int line = i * _size;

                for (int j = 0; j < _size; ++j)
                {
                    int index = (line + j) * 4;

                    triangles.Add(index);
                    triangles.Add(index + 3);
                    triangles.Add(index + 1);

                    triangles.Add(index);
                    triangles.Add(index + 2);
                    triangles.Add(index + 3);

                    int height = GetHeight(i, j);

                    int west = GetHeight(i - 1, j);

                    if (west >= 0 && west > height)
                    {
                        int otherIndex = ((i - 1) * _size + j) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index]);
                        vertices.Add(vertices[index + 2]);
                        vertices.Add(vertices[otherIndex + 1]);
                        vertices.Add(vertices[otherIndex + 3]);

                        heightByIndex.Add(height);
                        heightByIndex.Add(height);
                        heightByIndex.Add(west);
                        heightByIndex.Add(west);

                        addTriangles(current);
                    }

                    int north = GetHeight(i, j + 1);

                    if (north >= 0 && north > height)
                    {
                        int otherIndex = (i * _size + j + 1) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 2]);
                        vertices.Add(vertices[index + 3]);
                        vertices.Add(vertices[otherIndex]);
                        vertices.Add(vertices[otherIndex + 1]);

                        heightByIndex.Add(height);
                        heightByIndex.Add(height);
                        heightByIndex.Add(north);
                        heightByIndex.Add(north);

                        addTriangles(current);
                    }

                    int east = GetHeight(i + 1, j);

                    if (east >= 0 && east > height)
                    {
                        int otherIndex = ((i + 1) * _size + j) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 3]);
                        vertices.Add(vertices[index + 1]);
                        vertices.Add(vertices[otherIndex + 2]);
                        vertices.Add(vertices[otherIndex]);

                        heightByIndex.Add(height);
                        heightByIndex.Add(height);
                        heightByIndex.Add(east);
                        heightByIndex.Add(east);

                        addTriangles(current);
                    }

                    int south = GetHeight(i, j - 1);

                    if (south >= 0 && south > height)
                    {
                        int otherIndex = (i * _size + j - 1) * 4;

                        int current = vertices.Count;

                        vertices.Add(vertices[index + 1]);
                        vertices.Add(vertices[index]);
                        vertices.Add(vertices[otherIndex + 3]);
                        vertices.Add(vertices[otherIndex + 2]);

                        heightByIndex.Add(height);
                        heightByIndex.Add(height);
                        heightByIndex.Add(south);
                        heightByIndex.Add(south);

                        addTriangles(current);
                    }
                }
            }

            _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            _mesh.Clear();

            _mesh.vertices = vertices.ToArray();
            _mesh.triangles = triangles.ToArray();

            _mesh.RecalculateNormals();

            _heightByIndex = heightByIndex.ToArray();
        }

        private int GetHeight(int i, int j)
        {
            if (i >= _size || i < 0 || j >= _size || j < 0)
                return -1;

            return _heightByPosition[i, j];
        }

        public void ApplyGradient(Gradient gradient)
        {
            int length = _mesh.vertices.Length;

            Color32[] colors = new Color32[length];

            for (int i = 0; i < length; ++i)
            {
                int height = _heightByIndex[i];

                colors[i] = gradient.Evaluate(height / 15f);
            }

            _mesh.colors32 = colors;
        }

        public void DestroyProps()
        {
            foreach (Transform child in transform)
            {
                child.DOScale(Vector3.zero, 0.25f);

                Destroy(child.gameObject, 0.25f);
            }
        }

        public void GenerateProps(float freqency, GameObject[] props, int minHeight = 0)
        {
            float half = _size / 2f;

            for (int i = 0; i < _size; ++i)
            {
                for (int j = 0; j < _size; ++j)
                {
                    if (Random.Range(0f, 1f) > freqency)
                        continue;

                    float x = -half + i;
                    float z = -half + j;

                    int height = _heightByPosition[i, j];

                    if (height < minHeight)
                        continue;

                    GameObject prop = Instantiate(props[Random.Range(0, props.Length)], transform);

                    prop.transform.localPosition = new Vector3(x, height, z);

                    Vector3 scale = prop.transform.localScale;

                    prop.transform.localScale = Vector3.zero;

                    prop.transform.DOScale(scale, 0.25f);
                }
            }
        }
    }
}
