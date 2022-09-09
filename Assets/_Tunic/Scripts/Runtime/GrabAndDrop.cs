using System.Linq;
using UnityEngine;

namespace Tunic
{
    public class GrabAndDrop : MonoBehaviour
    {
        public Grabbable _grabbable;
        public Grabbable _selected;
        Collider trigger;


        public float ClickDuration = 0.7f;
        bool clicking = false;
        float totalDownTime = 0;

        private void Start()
        {
            trigger = GetComponents<Collider>().Where(c => c.isTrigger).FirstOrDefault();
        }


        void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                totalDownTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (_grabbable == null)
                {
                    _grabbable = _selected;
                    _grabbable.Grab(transform);
                    _selected = null;
                    trigger.enabled = false;
                }
                else
                {
                    if (totalDownTime >= ClickDuration)
                        _grabbable.Throw();
                    else _grabbable.Drop();
                    trigger.enabled = true;
                    _grabbable = null;
                    _selected = null;
                }
                totalDownTime = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger " + other.gameObject.GetComponents<Component>().Length, other.gameObject);
            if (!other.TryGetComponent<Grabbable>(out _selected))
            {
                if (!other.TryGetComponent<Politician>(out Politician politician))
                {
                    _selected = politician.Hat;
                    return;
                }
            }
            Debug.Log(_selected, _selected);
            if (_grabbable == _selected) return;
            enabled = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Grabbable>(out _selected)) return;
            enabled = false;
            Debug.Log(_selected, _selected);
        }
    }
}