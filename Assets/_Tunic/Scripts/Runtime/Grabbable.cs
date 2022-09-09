using DG.Tweening;
using UnityEngine;

namespace Tunic
{
    public class Grabbable : MonoBehaviour
    {
        Rigidbody Myrigidbody;
        new Collider collider;

        private void Start()
        {
            Myrigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }
        public void Grab(Transform parent)
        {
            transform.SetParent(parent);
            if (parent != null)
            {
                transform.DOLocalMove(new Vector3(0, 0.5f, 1.5f), .2f);
                transform.DORotateQuaternion(parent.rotation, .2f);
                Myrigidbody.isKinematic = true;
                collider.enabled = false;
            }

            Debug.Log("GRAB");
        }

        public void Drop()
        {
            transform.SetParent(null);
            Myrigidbody.isKinematic = false;
            collider.enabled = true;
            Debug.Log("DROP");
        }

        public void Throw()
        {
            transform.SetParent(null);
            collider.enabled = true;
            Myrigidbody.isKinematic = false;
            Myrigidbody.AddForce(transform.forward * 100);
            Debug.Log("THROW");
        }

        public void SetAsHat(Transform parent)
        {
            transform.position = parent.position;
            transform.DORotateQuaternion(parent.rotation, .4f);
            transform.SetParent(parent);
            Myrigidbody.isKinematic = true;
            Debug.Log("HIT");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.TryGetComponent<Politician>(out Politician _politician)) return;

            _politician.SetHat(this);
        }
    }
}