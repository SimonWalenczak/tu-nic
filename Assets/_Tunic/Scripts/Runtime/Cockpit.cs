using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunic
{
    public class Cockpit : MonoBehaviour
    {
        private PlayerInputs inputs;

        [SerializeField]
        protected float speed = 20f;

        [SerializeField]
        private bool invertY = false;

        [SerializeField]
        protected Vector2 Spin;

        bool drag;

        private void Awake()
        {
            inputs = new PlayerInputs();

            inputs.Player.Look.performed += Look;
            inputs.Player.Drag.started += OnDragStart;
            inputs.Player.Drag.canceled += OnDragEnd;
        }

        private void Look(InputAction.CallbackContext ctx)
        {
            if (drag)
            {
                Vector2 delta = ctx.ReadValue<Vector2>();

                if (invertY)
                    delta.y *= -1f;

                Spin = delta * speed;
            }
        }

        private void OnDragStart(InputAction.CallbackContext ctx)
        {
            drag = true;
        }

        private void OnDragEnd(InputAction.CallbackContext ctx)
        {
            drag = false;
        }

        private void OnEnable()
        {
            inputs.Enable();
        }

        private void OnDisable()
        {
            inputs.Disable();
        }
    }
}
