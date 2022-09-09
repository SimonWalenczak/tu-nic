using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Tunic
{
    public class Planet : MonoBehaviour
    {
        [SerializeField]
        private int size = 100;

        [SerializeField]
        private int seaLevel = 0;

        [SerializeField]
        private float rotationSpeed = 1f;

        [SerializeField]
        private Terrain[] terrains;

        [SerializeField]
        private Gradient gradient;

        [System.Serializable]
        class Cursor
        {
            public string name;
            public Color color;
            public int seaLevel;
            public GameObject[] props;
            public float score;
        }

        [SerializeField]
        private Cursor[] cursors;

        [SerializeField]
        private Transform water;

        private void Start()
        {
            for (int i = 0; i < terrains.Length; ++i)
            {
                terrains[i].Generate(size);
                terrains[i].GenerateProps(0.002f, 5);
            }

            StartCoroutine(ApplyCursors());
        }

        private IEnumerator ApplyCursors()
        {
            float seaLevel = 0;

            float r = 0f;
            float g = 0f;
            float b = 0f;

            float totalScore = 0f;

            foreach (Cursor cursor in cursors)
            {
                totalScore += cursor.score;

                r += cursor.color.r * cursor.score;
                g += cursor.color.g * cursor.score;
                b += cursor.color.b * cursor.score;

                seaLevel += cursor.seaLevel * cursor.score;
            }

            seaLevel = Mathf.Round(seaLevel / totalScore);

            r /= totalScore;
            g /= totalScore;
            b /= totalScore;

            GradientColorKey[] keys = gradient.colorKeys;

            keys[1].color = new Color(r, g, b);

            gradient.SetKeys(keys, gradient.alphaKeys);

            for (int i = 0; i < terrains.Length; ++i)
                terrains[i].ApplyGradient(gradient);

            water.localScale = seaLevel > 0 ? Vector3.one * (size + seaLevel + 0.1f) : Vector3.zero;

            yield return new WaitForEndOfFrame();
        }

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(2.5f * rotationSpeed * Time.deltaTime, 5f * rotationSpeed * Time.deltaTime, 0);
        }

        private void OnValidate()
        {
            StartCoroutine(ApplyCursors());
        }
    }
}
