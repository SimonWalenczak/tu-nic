using UnityEngine;

namespace Tunic
{
    public class Spaceship : Cockpit
    {
        [SerializeField]
        private Transform planet;

        private void Update()
        {
            if (Spin != Vector2.zero)
            {
                planet.transform.Rotate(Vector3.up, -Spin.x * Time.deltaTime,Space.World);
                planet.transform.Rotate(Vector3.right, -Spin.y * Time.deltaTime, Space.World);
                //transform.RotateAround(planet.transform.position, Vector3.up, Spin.x * Time.deltaTime);
                //transform.RotateAround(planet.transform.position, transform.right, Spin.y * Time.deltaTime);

                Spin = Vector3.Lerp(Spin, Vector3.zero, 10f * Time.deltaTime);
            }
        }
    }
}
