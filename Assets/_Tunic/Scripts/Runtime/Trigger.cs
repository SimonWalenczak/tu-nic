using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tunic
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] UnityEvent onTriggerEnter, onTriggerExit;

        private void Reset()
        {
            Collider c = GetComponent<Collider>();
            if (c == null) return;
            c.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke();
        }
    }
}
