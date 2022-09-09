using UnityEngine;

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
        private Gradient[] gradients;

        [SerializeField]
        private Transform water;

        bool dirty = false;

        private void Start()
        {
            for (int i = 0; i < terrains.Length; ++i)
            {
                terrains[i].Generate(size);
                terrains[i].GenerateProps(0.05f, seaLevel);
            }
        }

        public void applyGradients()
        {
            for (int i = 0; i < terrains.Length; ++i)
                terrains[i].ApplyGradient(gradients[i]);
        }

        private void setSeaLevel(int level)
        {
            water.localScale = level > 0 ? Vector3.one * (size + level + 0.1f) : Vector3.zero;
        }

        private void Update()
        {
            if (dirty)
            {
                applyGradients();
                setSeaLevel(seaLevel);

                dirty = false;
            }

            transform.rotation *= Quaternion.Euler(2.5f * rotationSpeed * Time.deltaTime, 5f * rotationSpeed * Time.deltaTime, 0);
        }

        private void OnValidate()
        {
            dirty = true;
        }
    }
}
