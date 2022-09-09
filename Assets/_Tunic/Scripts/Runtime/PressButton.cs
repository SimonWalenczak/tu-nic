using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunic
{
	public class PressButton : Selector
	{
		[SerializeField] InputActionReference pressInput;
		
		protected void Awake()
		{
			enabled = false;
			pressInput.action.performed += Press;
		}
		
		protected void OnEnable()
		{
			pressInput.action.Enable();
		}
		
		protected void OnDisable()
		{
			pressInput.action.Disable();
		}
		
		protected override void Select(ISelectable selectable)
		{
			if (!(selectable is WorldButton))return;
			base.Select(selectable);
			enabled = true;
		}
		
		protected override void UnSelect(ISelectable selectable)
		{
			if (!(selectable is WorldButton))return;
			base.UnSelect(selectable);
			enabled = false;
		}
		
		void Press(InputAction.CallbackContext c)
		{
			Debug.Log($"Button pressed to {selectable}");
			if (!(selectable is WorldButton button)) return;
			button.Press();
			//UnSelect(selectable);
		}
		
		protected void OnApplicationQuit()
		{
			pressInput.action.performed -= Press;
		}
    }
}
